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

    public class ProveedoresController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;

        public ProveedoresController(DataContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IGetHelper getHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Proveedores
                .Include(c => c.ProveedorContactos)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            IQueryable<Proveedor> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Proveedores
                                    .Include(c => c.ProveedorContactos)
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
                                .Include(c => c.ProveedorContactos)
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
                query = _context.Proveedores
                    .Include(c => c.ProveedorContactos)
                    .Include(c => c.Municipios.Estados)
                    .Include(c => c.Municipios);
            }

            var proveedores = await query.OrderBy(p => p.RFC)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Proveedor>>(ViewData, proveedores)
            };
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .Include(c => c.ProveedorContactos)
                .Include(c => c.Municipios.Estados)
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ProveedorID == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        public async Task<IActionResult> Create()
        {
            var proveedorViewModel = new ProveedorViewModel()
            {
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(0)
            };

            return View(proveedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorViewModel proveedorViewModel)
        {
            if (ModelState.IsValid)
            {
                var proveedor = await _converterHelper.ToProveedorAsync(proveedorViewModel, true);
                _context.Add(proveedor);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = proveedor.ProveedorID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            proveedorViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            proveedorViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)proveedorViewModel.EstadoID);

            return View(proveedorViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(c => c.ProveedorID == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            var proveedorViewModel = await _converterHelper.ToProveedorViewModelAsync(proveedor);

            return View(proveedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProveedorViewModel proveedorViewModel)
        {
            if (id != proveedorViewModel.ProveedorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var proveedor = await _converterHelper.ToProveedorAsync(proveedorViewModel, false);
                    _context.Update(proveedor);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = proveedor.ProveedorID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedorViewModel.ProveedorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Proveedor Inexistente.");
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(proveedorViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _getHelper.GetProveedorByIdAsync((Guid)id);
            if (proveedor == null)
            {
                return NotFound();
            }

            if (proveedor.ProveedorContactos.Count > 0)
            {
                ModelState.AddModelError(string.Empty, $"El proveedor no se puede eliminar, porque tiene {proveedor.ProveedorContactos.Count} contacto(s) asignado(s).");
                return RedirectToAction(nameof(Index));
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddMunicipios(short? id)
        {
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

        private bool ProveedorExists(Guid id)
        {
            return _context.Proveedores.Any(e => e.ProveedorID == id);
        }

        //Contactos

        public async Task<IActionResult> AddContacto(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = new ProveedorContacto()
            {
                ProveedorID = (Guid)id,
                Proveedor = await _context.Proveedores.FindAsync((Guid)id)
            };

            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContacto(ProveedorContacto proveedorContacto)
        {
            var proveedor = await _getHelper.GetProveedorByIdAsync(proveedorContacto.ProveedorID);

            if (ModelState.IsValid)
            {
                if (proveedor == null)
                {
                    ModelState.AddModelError(string.Empty, "Contacto no ingresado, proveedor inexistente.");
                    return View();
                }

                try
                {
                    proveedorContacto.ProveedorContactoID = Guid.NewGuid();
                    proveedorContacto.NombreContacto = proveedorContacto.NombreContacto.Trim().ToUpper();
                    proveedorContacto.PrimerApellidoContacto = proveedorContacto.PrimerApellidoContacto.Trim().ToUpper();
                    proveedorContacto.SegundoApellidoContacto = proveedorContacto.SegundoApellidoContacto.Trim().ToUpper();
                    proveedorContacto.EmailContacto = proveedorContacto.EmailContacto.Trim().ToLower();

                    _context.Add(proveedorContacto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = proveedorContacto.ProveedorID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            proveedorContacto.Proveedor = proveedor;
            return View(proveedorContacto);
        }

        public async Task<IActionResult> EditContacto(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _getHelper.GetContactoProveedorByIdAsync((Guid)id);

            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContacto(ProveedorContacto proveedorContacto)
        {
            if (ModelState.IsValid)
            {
                var contacto = await _getHelper.GetContactoProveedorByIdAsync(proveedorContacto.ProveedorContactoID);
                
                if (contacto == null)
                {
                    ModelState.AddModelError(string.Empty, 
                        "Actualización no realizada, contacto inexistente.");
                    proveedorContacto.Proveedor = await _getHelper
                        .GetProveedorByIdAsync(proveedorContacto.ProveedorID);

                    return View(proveedorContacto);
                }

                try
                {
                    contacto.NombreContacto = proveedorContacto.NombreContacto.Trim().ToUpper();
                    contacto.PrimerApellidoContacto = proveedorContacto.PrimerApellidoContacto.Trim().ToUpper();
                    contacto.SegundoApellidoContacto = proveedorContacto.SegundoApellidoContacto.Trim().ToUpper();
                    contacto.EmailContacto = proveedorContacto.EmailContacto.Trim().ToLower();
                    contacto.TelefonoMovilContacto = proveedorContacto.TelefonoMovilContacto;

                    _context.Update(contacto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = contacto.ProveedorID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            proveedorContacto.Proveedor = await _getHelper
                .GetProveedorByIdAsync(proveedorContacto.ProveedorID);

            return View(proveedorContacto);
        }

        public async Task<IActionResult> DeleteContacto(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _getHelper.GetContactoProveedorByIdAsync((Guid)id);
            if (contacto == null)
            {
                return NotFound();
            }

            _context.ProveedorContactos.Remove(contacto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = contacto.ProveedorID });
        }
    }
}
