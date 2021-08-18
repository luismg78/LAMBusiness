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

    public class SalidasTipoController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("06239203-ACED-4600-9F70-784BF73281EC");

        public SalidasTipoController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var salidaTipo = _context.SalidasTipo
                .OrderBy(e => e.SalidaTipoDescripcion);

            return View(salidaTipo);
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

            IQueryable<SalidaTipo> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.SalidasTipo
                                    .Where(e => e.SalidaTipoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(e => e.SalidaTipoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.SalidasTipo;
            }

            var salidasTipo = await query.OrderBy(m => m.SalidaTipoDescripcion)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<SalidaTipo>>(ViewData, salidasTipo)
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
        public async Task<IActionResult> Create([Bind("SalidaTipoID,SalidaTipoDescripcion")] SalidaTipo salidaTipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                salidaTipo.SalidaTipoID = Guid.NewGuid();
                _context.Add(salidaTipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(salidaTipo);
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

            var salidaTipo = await _context.SalidasTipo.FindAsync(id);
            if (salidaTipo == null)
            {
                return NotFound();
            }

            return View(salidaTipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SalidaTipoID,SalidaTipoDescripcion")] SalidaTipo salidaTipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != salidaTipo.SalidaTipoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salidaTipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalidaTipoExists(salidaTipo.SalidaTipoID))
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
            return View(salidaTipo);
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

            var salidaTipo = await _context.SalidasTipo
                               .FirstOrDefaultAsync(m => m.SalidaTipoID == id);

            if (salidaTipo == null)
            {
                return NotFound();
            }

            _context.SalidasTipo.Remove(salidaTipo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SalidaTipoExists(Guid id)
        {
            return _context.SalidasTipo.Any(e => e.SalidaTipoID == id);
        }
    }
}
