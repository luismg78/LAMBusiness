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

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            var dataContext = _context.Almacenes
                .OrderBy(p => p.AlmacenNombre);

            return View(dataContext);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }
            
            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            IQueryable<Almacen> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
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

            var almacenes = await query.OrderBy(m => m.AlmacenNombre)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Almacen>>(ViewData, almacenes)
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

            if (ModelState.IsValid)
            {
                almacen.AlmacenID = Guid.NewGuid();
                _context.Add(almacen);
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del almacén fueron almacenados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["toast"] = "Falta información en algún campo.";
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del almacén fueron actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmacenExists(almacen.AlmacenID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "Error al actualizar la información.";
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
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
                    ModelState.AddModelError(string.Empty, $"El almacen no puede ser eliminado, tiene {existencias} producto(s) en existencia(s).");
                    return RedirectToAction(nameof(Index));
                }
            }

            _context.Almacenes.Remove(almacen);
            await _context.SaveChangesAsync();
            TempData["toast"] = "Los datos del almacén fueron eliminados correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool AlmacenExists(Guid id)
        {
            return _context.Almacenes.Any(e => e.AlmacenID == id);
        }
    }
}
