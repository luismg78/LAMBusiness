namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;

    public class ProveedoresController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("76BE358D-AA92-48E2-AB1A-5EE557441067");

        public ProveedoresController(DataContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IGetHelper getHelper,
            IConfiguration configuration)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
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

            var proveedores = _context.Proveedores
                .Include(c => c.ProveedorContactos)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados);

            var filtro = new Filtro<List<Proveedor>>()
            {
                Datos = await proveedores.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await proveedores.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Proveedor>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Proveedor> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
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

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(p => p.RFC)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoLectura;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Proveedor>>>(ViewData, filtro)
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

            var proveedor = await _context.Proveedores
                .Include(c => c.ProveedorContactos)
                .Include(c => c.Municipios.Estados)
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ProveedorID == id);
            if (proveedor == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            await ValidarDatosDelProveedor(proveedorViewModel);

            if (ModelState.IsValid)
            {
                var proveedor = await _converterHelper.ToProveedorAsync(proveedorViewModel, true);
                _context.Add(proveedor);
                try
                {
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", proveedor);
                    TempData["toast"] = "Los datos del proveedor fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = proveedor.ProveedorID });
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del proveedor no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", proveedor, excepcion);
                }
            }

            proveedorViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            proveedorViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)proveedorViewModel.EstadoID);

            return View(proveedorViewModel);
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
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var proveedor = await _context.Proveedores
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(c => c.ProveedorID == id);
            if (proveedor == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var proveedorViewModel = await _converterHelper.ToProveedorViewModelAsync(proveedor);

            return View(proveedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProveedorViewModel proveedorViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != proveedorViewModel.ProveedorID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            await ValidarDatosDelProveedor(proveedorViewModel);
            
            if (ModelState.IsValid)
            {
                var proveedor = await _converterHelper.ToProveedorAsync(proveedorViewModel, false);
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del proveedor fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", proveedor);
                    return RedirectToAction(nameof(Details), new { id = proveedor.ProveedorID });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProveedorExists(proveedorViewModel.ProveedorID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del proveedor no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", proveedor, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del proveedor no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", proveedor, excepcion);
                }
            }

            return View(proveedorViewModel);
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
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var proveedor = await _getHelper.GetProveedorByIdAsync((Guid)id);
            if (proveedor == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            if (proveedor.ProveedorContactos.Count > 0)
            {
                TempData["toast"] = $"El proveedor no se puede eliminar, porque tiene {proveedor.ProveedorContactos.Count} contacto(s) asignado(s).";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", proveedor);
                TempData["toast"] = "Los datos del proveedor fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del proveedor no fueron eliminados.";
                await BitacoraAsync("Baja", proveedor, excepcion);
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

        private bool ProveedorExists(Guid id)
        {
            return _context.Proveedores.Any(e => e.ProveedorID == id);
        }

        public async Task<IActionResult> GetSupplierByRFCAsync(string rfc)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (rfc == null || rfc == "")
            {
                return null;
            }

            var proveedor = await _getHelper.GetProveedorByRFCAsync(rfc);
            if (proveedor != null)
            {
                return Json(
                    new
                    {
                        proveedor.ProveedorID,
                        proveedor.RFC,
                        proveedor.Nombre,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Proveedor inexistente" });

        }

        public async Task<IActionResult> GetSuppliersAsync(Filtro<List<Proveedor>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return new EmptyResult(); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return new EmptyResult();
            }

            filtro = await _getHelper.GetProveedoresByPatternAsync(filtro);

            if (filtro.Registros == 0)
            {
                return new EmptyResult();
            }

            return new PartialViewResult
            {
                ViewName = "_GetProveedores",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Proveedor>>>(ViewData, filtro)
            };
        }

        //Contactos

        public async Task<IActionResult> AddContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var proveedor = await _getHelper.GetProveedorByIdAsync(proveedorContacto.ProveedorID);

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                if (proveedor == null)
                {
                    TempData["toast"] = "Identificacor incorrecto, verifique.";
                    return RedirectToAction(nameof(Index));
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
                    TempData["toast"] = "Los datos del contacto fueron almacenados correctamente.";
                    await BitacoraAsync("Alta", proveedorContacto, proveedorContacto.ProveedorID);
                    return RedirectToAction(nameof(Details), new { id = proveedorContacto.ProveedorID });

                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del contacto no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", proveedorContacto, proveedorContacto.ProveedorID, excepcion);
                }
            }

            proveedorContacto.Proveedor = proveedor;
            return View(proveedorContacto);
        }

        public async Task<IActionResult> EditContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var contacto = await _getHelper.GetContactoProveedorByIdAsync((Guid)id);

            return View(contacto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContacto(ProveedorContacto proveedorContacto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

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
                    TempData["toast"] = "Los datos del contacto fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", contacto, contacto.ProveedorID);
                    return RedirectToAction(nameof(Details), new { id = contacto.ProveedorID });

                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del contacto no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", contacto, contacto.ProveedorID, excepcion);
                }
            }

            proveedorContacto.Proveedor = await _getHelper
                .GetProveedorByIdAsync(proveedorContacto.ProveedorID);

            return View(proveedorContacto);
        }

        public async Task<IActionResult> DeleteContacto(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var contacto = await _getHelper.GetContactoProveedorByIdAsync((Guid)id);
            if (contacto == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.ProveedorContactos.Remove(contacto);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", contacto, contacto.ProveedorID);
                TempData["toast"] = "Los datos del contacto fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del contacto no fueron eliminados.";
                await BitacoraAsync("Baja", contacto, contacto.ProveedorID, excepcion);
            }

            return RedirectToAction(nameof(Details), new { id = contacto.ProveedorID });
        }

        private async Task BitacoraAsync(string accion, Proveedor proveedor, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                proveedor, proveedor.ProveedorID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, ProveedorContacto contacto, Guid proveedorId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                contacto, proveedorId.ToString(), directorioBitacora, excepcion);
        }

        private async Task ValidarDatosDelProveedor(ProveedorViewModel proveedor)
        {
            proveedor.RFC = proveedor.RFC.Trim().ToUpper();

            var existeRFC = await _context.Proveedores
                .AnyAsync(p => p.RFC == proveedor.RFC &&
                               p.ProveedorID != proveedor.ProveedorID);
            if (existeRFC)
            {
                TempData["toast"] = "RFC previamente asignado a otro proveedor.";
                ModelState.AddModelError("RFC", "RFC previamente asignado.");
            }
        }
    }
}
