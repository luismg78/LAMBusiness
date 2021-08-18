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
    using Shared.Catalogo;

    public class TasasImpuestosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("1B7183E2-E51B-4091-A99A-9BB38D462D81");

        public TasasImpuestosController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var tasaImpuesto = _context.TasasImpuestos
                .Include(t => t.Productos)
                .OrderBy(t => t.Tasa);

            return View(tasaImpuesto);
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

            IQueryable<TasaImpuesto> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.TasasImpuestos
                                    .Include(t => t.Productos)
                                    .Where(t => t.Tasa.Contains(w) ||
                                           t.TasaDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Include(t => t.Productos)
                                    .Where(t => t.Tasa.Contains(w) ||
                                           t.TasaDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.TasasImpuestos.Include(t => t.Productos);
            }

            var tasas = await query.OrderBy(t => t.Tasa)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<TasaImpuesto>>(ViewData, tasas)
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
        public async Task<IActionResult> Create([Bind("TasaID,Tasa,Porcentaje,TasaDescripcion")] TasaImpuesto tasaImpuesto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                tasaImpuesto.TasaID = Guid.NewGuid();
                _context.Add(tasaImpuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasaImpuesto);
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
                return NotFound();
            }

            var tasaImpuesto = await _context.TasasImpuestos.FindAsync(id);
            if (tasaImpuesto == null)
            {
                return NotFound();
            }
            return View(tasaImpuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TasaID,Tasa,Porcentaje,TasaDescripcion")] TasaImpuesto tasaImpuesto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != tasaImpuesto.TasaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasaImpuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasaImpuestoExists(tasaImpuesto.TasaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tasaImpuesto);
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
                return NotFound();
            }

            var tasaImpuesto = await _context.TasasImpuestos
                .Include(t => t.Productos)
                .FirstOrDefaultAsync(t => t.TasaID == id);
            if (tasaImpuesto == null)
            {
                return NotFound();
            }

            if (tasaImpuesto.Productos.Count > 0)
            {
                ModelState.AddModelError(string.Empty, $"La Tasa de impuesto no se puede eliminar, porque está asignado a {tasaImpuesto.Productos.Count} producto(s).");
                return RedirectToAction(nameof(Index));
            }

            _context.TasasImpuestos.Remove(tasaImpuesto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool TasaImpuestoExists(Guid id)
        {
            return _context.TasasImpuestos.Any(e => e.TasaID == id);
        }
    }
}
