namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Microsoft.Extensions.Configuration;

    public class ClientesController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("9B4E6E0C-6F34-4513-AACF-4A9A516CEDF6");

        public ClientesController(DataContext context, 
            IGetHelper getHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            var dataContext = _context.Clientes
                .Include(c => c.ClienteContactos)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }
            
            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            IQueryable<Cliente> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Clientes
                                    .Include(c => c.ClienteContactos)
                                    .Include(c => c.Municipios.Estados)
                                    .Include(c => c.Municipios)
                                    .Where(p => p.RFC.Contains(w) ||
                                                p.Nombre.Contains(w) ||
                                                p.Municipios.Estados.EstadoDescripcion.Contains(w) ||
                                                p.Municipios.MunicipioDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(c => c.ClienteContactos)
                                .Include(c => c.Municipios.Estados)
                                .Include(c => c.Municipios)
                                .Where(c => c.RFC.Contains(w) ||
                                                c.Nombre.Contains(w) ||
                                                c.Municipios.Estados.EstadoDescripcion.Contains(w) ||
                                                c.Municipios.MunicipioDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Clientes
                    .Include(c => c.ClienteContactos)
                    .Include(c => c.Municipios.Estados)
                    .Include(c => c.Municipios);
            }

            var clientes = await query.OrderBy(p => p.RFC)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Cliente>>(ViewData, clientes)
            };
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var cliente = await _context.Clientes
                .Include(c => c.ClienteContactos)
                .Include(c => c.Municipios.Estados)
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var clienteViewModel = new ClienteViewModel()
            {
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(0)
            };

            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clienteViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, true);
                _context.Add(cliente);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del cliente fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID});
                }
                catch (Exception)
                {
                    TempData["toast"] = "[Error] Los datos del cliente no fueron almacenados.";
                }
            }

            clienteViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            clienteViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)clienteViewModel.EstadoID);

            TempData["toast"] = "Falta información en algún campo, verifique.";
            return View(clienteViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var cliente = await _context.Clientes
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(c => c.ClienteID == id);
            if (cliente == null)
            {
                TempData["toast"] = "Cliente inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            var clienteViewModel = await _converterHelper.ToClienteViewModelAsync(cliente);

            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClienteViewModel clienteViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != clienteViewModel.ClienteID)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, false);
                    _context.Update(cliente);

                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del cliente fueron actualizados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(clienteViewModel.ClienteID))
                    {
                        TempData["toast"] = "Cliente inexistente (identificador incorrecto).";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del cliente no fueron actualizados.";
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            return View(clienteViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var cliente = await _getHelper.GetClienteByIdAsync((Guid)id);
            if (cliente == null)
            {
                TempData["toast"] = "Cliente inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            if(cliente.ClienteContactos.Count > 0)
            {
                TempData["toast"] = $"El cliente no se puede eliminar, porque tiene {cliente.ClienteContactos.Count} contacto(s) asignado(s).";
                ModelState.AddModelError(string.Empty, $"El cliente no se puede eliminar, porque tiene {cliente.ClienteContactos.Count} contacto(s) asignado(s).");
                return RedirectToAction(nameof(Index));
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            
            TempData["toast"] = "Los datos del cliente fueron eliminados correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddMunicipios(short? id) 
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (id == null)
            {
                return null;
            }

            var municipios = await _getHelper.GetMunicipiosByEstadoIdAsync((short)id);

            return new PartialViewResult
            {
                ViewName = "_AddComboMunicipios",
                ViewData = new ViewDataDictionary
                            <List<Municipio>>(ViewData, municipios)
            };
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }

        //Contactos

        public async Task<IActionResult> AddContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var contacto = new ClienteContacto()
            {
                ClienteID = (Guid)id,
                Cliente = await _context.Clientes.FindAsync((Guid)id)
            };

            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContacto(ClienteContacto clienteContacto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id = clienteContacto.ClienteID });
            }

            var cliente = await _getHelper.GetClienteByIdAsync(clienteContacto.ClienteID);

            if (ModelState.IsValid)
            {
                if (cliente == null)
                {
                    TempData["toast"] = "Contacto no ingresado, cliente inexistente.";
                    ModelState.AddModelError(string.Empty, "Contacto no ingresado, cliente inexistente.");
                    return View();
                }

                try
                {
                    clienteContacto.ClienteContactoID = Guid.NewGuid();
                    clienteContacto.NombreContacto = clienteContacto.NombreContacto.Trim().ToUpper();
                    clienteContacto.PrimerApellidoContacto = clienteContacto.PrimerApellidoContacto.Trim().ToUpper();
                    clienteContacto.SegundoApellidoContacto = clienteContacto.SegundoApellidoContacto == null ? "" : clienteContacto.SegundoApellidoContacto.Trim().ToUpper();
                    clienteContacto.EmailContacto = clienteContacto.EmailContacto.Trim().ToLower();

                    _context.Add(clienteContacto);

                    await _context.SaveChangesAsync();
                    
                    TempData["toast"] = "Los datos del contacto fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = clienteContacto.ClienteID });

                }
                catch (Exception)
                {
                    TempData["toast"] = "[Error] Los datos del contacto no fueron almacenados.";
                    //ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteContacto.Cliente = cliente;

            TempData["toast"] = "Falta información en algún campo.";
            return View(clienteContacto);
        }

        public async Task<IActionResult> EditContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var contacto = await _getHelper.GetContactoClienteByIdAsync((Guid)id);
                
            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContacto(ClienteContacto clienteContacto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id = clienteContacto.ClienteID });
            }

            if (ModelState.IsValid)
            {
                var contacto = await _getHelper.GetContactoClienteByIdAsync(clienteContacto.ClienteContactoID);

                if (contacto == null)
                {
                    TempData["toast"] = "Actualización no realizada, contacto inexistente.";
                    ModelState.AddModelError(string.Empty,
                        "Actualización no realizada, contacto inexistente.");
                    clienteContacto.Cliente = await _getHelper
                        .GetClienteByIdAsync(clienteContacto.ClienteID);

                    return View(clienteContacto);
                }

                try
                {
                    contacto.NombreContacto = clienteContacto.NombreContacto.Trim().ToUpper();
                    contacto.PrimerApellidoContacto = clienteContacto.PrimerApellidoContacto.Trim().ToUpper();
                    contacto.SegundoApellidoContacto = clienteContacto.SegundoApellidoContacto.Trim().ToUpper();
                    contacto.EmailContacto = clienteContacto.EmailContacto.Trim().ToLower();
                    contacto.TelefonoMovilContacto = clienteContacto.TelefonoMovilContacto;

                    _context.Update(contacto);

                    await _context.SaveChangesAsync();
                    
                    TempData["toast"] = "Los datos del contacto fueron actualizados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = contacto.ClienteID });

                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del contacto no fueron actualizados.";
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteContacto.Cliente = await _getHelper
                .GetClienteByIdAsync(clienteContacto.ClienteID);
            
            TempData["toast"] = "Falta información en algún campo.";
            return View(clienteContacto);
        }

        public async Task<IActionResult> DeleteContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var contacto = await _getHelper.GetContactoClienteByIdAsync((Guid)id);
            if (contacto == null)
            {
                TempData["toast"] = "Contacto inexistente, identificador incorrecto.";
                return RedirectToAction(nameof(Details), new { id });
            }

            _context.ClienteContactos.Remove(contacto);
            await _context.SaveChangesAsync();
                
            TempData["toast"] = "Los datos del contacto fueron eliminados correctamente.";
            return RedirectToAction(nameof(Details),new { id = contacto.ClienteID });
        }
    }
}
