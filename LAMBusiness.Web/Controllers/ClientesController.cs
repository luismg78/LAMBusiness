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

    public class ClientesController : Controller
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ClientesController(DataContext context, 
            IGetHelper getHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Clientes
                .Include(c => c.ClienteContactos)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.ClienteContactos)
                .Include(c => c.Municipios.Estados)
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public async Task<IActionResult> Create()
        {
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
            if (ModelState.IsValid)
            {
                var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, true);
                _context.Add(cliente);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            clienteViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)clienteViewModel.EstadoID);

            return View(clienteViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(c => c.ClienteID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            var clienteViewModel = await _converterHelper.ToClienteViewModelAsync(cliente);

            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClienteViewModel clienteViewModel)
        {
            if (id != clienteViewModel.ClienteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, false);
                    _context.Update(cliente);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(clienteViewModel.ClienteID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cliente Inexistente.");
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(clienteViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _getHelper.GetClienteByIdAsync((Guid)id);
            if (cliente == null)
            {
                return NotFound();
            }

            if(cliente.ClienteContactos.Count > 0)
            {
                ModelState.AddModelError(string.Empty, $"El cliente no se puede eliminar, porque tiene {cliente.ClienteContactos.Count} contacto(s) asignado(s).");
                return RedirectToAction(nameof(Index));
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddMunicipios(short? id) 
        {
            if(id == null)
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
            if(id == null)
            {
                return NotFound();
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
            var cliente = await _getHelper.GetClienteByIdAsync(clienteContacto.ClienteID);

            if (ModelState.IsValid)
            {
                if (cliente == null)
                {
                    ModelState.AddModelError(string.Empty, "Contacto no ingresado, cliente inexistente.");
                    return View();
                }

                try
                {
                    clienteContacto.ClienteContactoID = Guid.NewGuid();
                    clienteContacto.NombreContacto = clienteContacto.NombreContacto.Trim().ToUpper();
                    clienteContacto.PrimerApellidoContacto = clienteContacto.PrimerApellidoContacto.Trim().ToUpper();
                    clienteContacto.SegundoApellidoContacto = clienteContacto.SegundoApellidoContacto.Trim().ToUpper();
                    clienteContacto.EmailContacto = clienteContacto.EmailContacto.Trim().ToLower();

                    _context.Add(clienteContacto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = clienteContacto.ClienteID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteContacto.Cliente = cliente;
            return View(clienteContacto);
        }

        public async Task<IActionResult> EditContacto(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _getHelper.GetContactoClienteByIdAsync((Guid)id);
                
            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContacto(ClienteContacto clienteContacto)
        {
            if (ModelState.IsValid)
            {
                var contacto = await _getHelper.GetContactoClienteByIdAsync(clienteContacto.ClienteContactoID);

                if (contacto == null)
                {
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
                    return RedirectToAction(nameof(Details), new { id = contacto.ClienteID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteContacto.Cliente = await _getHelper
                .GetClienteByIdAsync(clienteContacto.ClienteID);

            return View(clienteContacto);
        }

        public async Task<IActionResult> DeleteContacto(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _getHelper.GetContactoClienteByIdAsync((Guid)id);
            if (contacto == null)
            {
                return NotFound();
            }

            _context.ClienteContactos.Remove(contacto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),new { id = contacto.ClienteID });
        }
    }
}
