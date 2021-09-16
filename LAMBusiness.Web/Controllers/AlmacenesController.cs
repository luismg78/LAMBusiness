namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Shared.Aplicacion;
    using Shared.Catalogo;

    public class AlmacenesController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("DA183D55-101E-4A06-9EC3-A1ED5729F0CB");

        public AlmacenesController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            var almacenes = _context.Almacenes;

            var filtro = new Filtro<List<Almacen>>()
            {
                Datos = await almacenes.OrderBy(p => p.AlmacenNombre).Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoImprimir,
                Registros = await almacenes.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Almacen>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }
            
            IQueryable<Almacen> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Almacenes
                                    .Where(p => p.AlmacenNombre.Contains(w) ||
                                           p.AlmacenDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.AlmacenNombre.Contains(w) ||
                                                p.AlmacenDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Almacenes;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.AlmacenNombre)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoImprimir;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Almacen>>>(ViewData, filtro)
            };
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlmacenID,AlmacenNombre,AlmacenDescripcion")] Almacen almacen)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            if (ModelState.IsValid)
            {
                try
                {
                    almacen.AlmacenID = Guid.NewGuid();
                    _context.Add(almacen);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", almacen);
                    TempData["toast"] = "Los datos del almacén fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del almacén no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", almacen, excepcion);
                }
            }

            return View(almacen);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
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

            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlmacenID,AlmacenNombre,AlmacenDescripcion")] Almacen almacen)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != almacen.AlmacenID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del almacén fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", almacen);
                    return RedirectToAction(nameof(Index)); 
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!AlmacenExists(almacen.AlmacenID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del almacén no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", almacen, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del almacén no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", almacen, excepcion);
                }
            }

            return View(almacen);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
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

            var almacen = await _context.Almacenes
                               .FirstOrDefaultAsync(m => m.AlmacenID == id);

            if (almacen == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            if (almacen.Existencias != null)
            {
                decimal existencias = almacen.Existencias.Sum(s => s.ExistenciaEnAlmacen);

                if (existencias > 0)
                {
                    TempData["toast"] = $"El almacen no puede ser eliminado, tiene {existencias} producto(s) en existencia(s).";
                    return RedirectToAction(nameof(Index));
                }
            }

            try
            {
                _context.Almacenes.Remove(almacen);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", almacen);
                TempData["toast"] = "Los datos del almacén fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del almacén no fueron eliminados.";
                await BitacoraAsync("Baja", almacen, excepcion);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAlmacenByNameAsync(string almacenNombre)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return new EmptyResult();
            }

            if (almacenNombre == null || almacenNombre == "")
            {
                return new EmptyResult();
            }

            var almacen = await _getHelper.GetAlmacenByNombreAsync(almacenNombre.Trim().ToUpper());
            if (almacen != null)
            {
                return Json(
                    new
                    {
                        almacen.AlmacenID,
                        almacen.AlmacenNombre,
                        almacen.AlmacenDescripcion,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Almacén inexistente" });

        }

        public async Task<IActionResult> GetAlmacenesListAsync(Filtro<List<Almacen>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            filtro = await _getHelper.GetAlmacenesByPatternAsync(filtro);

            return new PartialViewResult
            {
                ViewName = "_GetAlmacenes",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Almacen>>>(ViewData, filtro)
            };
        }

        private bool AlmacenExists(Guid id)
        {
            return _context.Almacenes.Any(e => e.AlmacenID == id);
        }
    
        private async Task BitacoraAsync(string accion, Almacen almacen, string excepcion = "" )
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");
            
            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                almacen, almacen.AlmacenID.ToString(), directorioBitacora, excepcion);
        }
    }
}
