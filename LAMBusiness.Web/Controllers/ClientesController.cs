namespace LAMBusiness.Web.Controllers
{
    using Helpers;
    using LAMBusiness.Contextos;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
                return RedirectToAction("Index", "Home");
            }

            var clientes = _context.Clientes
                .Include(c => c.ClienteContactos)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados);

            var filtro = new Filtro<List<Cliente>>()
            {
                Datos = await clientes.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await clientes.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Cliente>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Cliente> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
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
                                                p.Municipios.Estados.Nombre.Contains(w) ||
                                                p.Municipios.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(c => c.ClienteContactos)
                                .Include(c => c.Municipios.Estados)
                                .Include(c => c.Municipios)
                                .Where(c => c.RFC.Contains(w) ||
                                                c.Nombre.Contains(w) ||
                                                c.Municipios.Estados.Nombre.Contains(w) ||
                                                c.Municipios.Nombre.Contains(w));
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

            filtro.Registros = await query.CountAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoLectura;

            filtro.Datos = await query.OrderBy(p => p.RFC)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Cliente>>>(ViewData, filtro)
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

            TempData["toast"] = "Falta información en algún campo, verifique.";
            await ValidarDatosDelCliente(clienteViewModel);

            if (ModelState.IsValid)
            {
                var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, true);
                _context.Add(cliente);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del cliente fueron almacenados correctamente.";
                    await BitacoraAsync("Alta", cliente);
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID });
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del cliente no fueron almacenados.";
                    await BitacoraAsync("Alta", cliente, excepcion);
                }
            }

            clienteViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            clienteViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)clienteViewModel.EstadoID);

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

            TempData["toast"] = "Falta información en algún campo.";
            await ValidarDatosDelCliente(clienteViewModel);

            if (ModelState.IsValid)
            {
                var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, false);
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del cliente fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", cliente);
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID });
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del cliente no fueron actualizados.";
                    await BitacoraAsync("Actualizar", cliente, excepcion);
                }
                return RedirectToAction(nameof(Index));
            }

            clienteViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            clienteViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)clienteViewModel.EstadoID);

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

            if (cliente.ClienteContactos.Count > 0)
            {
                TempData["toast"] = $"El cliente no se puede eliminar, porque tiene {cliente.ClienteContactos.Count} contacto(s) asignado(s).";
                ModelState.AddModelError(string.Empty, $"El cliente no se puede eliminar, porque tiene {cliente.ClienteContactos.Count} contacto(s) asignado(s).");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del cliente fueron eliminados correctamente.";
                await BitacoraAsync("Baja", cliente);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del cliente no fueron eliminados.";
                await BitacoraAsync("Baja", cliente, excepcion);
            }

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
                    await BitacoraAsync("Alta", clienteContacto, cliente.ClienteID);
                    return RedirectToAction(nameof(Details), new { id = clienteContacto.ClienteID });

                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del contacto no fueron almacenados.";
                    await BitacoraAsync("Alta", clienteContacto, cliente.ClienteID, excepcion);
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
                    await BitacoraAsync("Actualizar", contacto, contacto.ClienteID);
                    return RedirectToAction(nameof(Details), new { id = contacto.ClienteID });

                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del contacto no fueron actualizados.";
                    await BitacoraAsync("Actualizar", contacto, contacto.ClienteID, excepcion);
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

            try
            {
                _context.ClienteContactos.Remove(contacto);
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del contacto fueron eliminados correctamente.";
                await BitacoraAsync("Baja", contacto, contacto.ClienteID);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del contacto no fueron eliminados.";
                await BitacoraAsync("Baja", contacto, contacto.ClienteID);
            }

            return RedirectToAction(nameof(Details), new { id = contacto.ClienteID });
        }

        private async Task BitacoraAsync(string accion, Cliente cliente, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                cliente, cliente.ClienteID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, ClienteContacto contacto, Guid clienteId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                contacto, clienteId.ToString(), directorioBitacora, excepcion);
        }

        private async Task ValidarDatosDelCliente(ClienteViewModel cliente)
        {
            cliente.RFC = cliente.RFC.Trim().ToUpper();

            var existeRFC = await _context.Clientes
                .AnyAsync(c => c.RFC == cliente.RFC &&
                               c.ClienteID != cliente.ClienteID);
            if (existeRFC)
            {
                TempData["toast"] = "RFC previamente asignado a otro cliente.";
                ModelState.AddModelError("RFC", "RFC previamente asignado.");
            }
        }
    }
}
