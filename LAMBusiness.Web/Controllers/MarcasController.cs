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

    public class MarcasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("7FCF442E-5377-47E0-B5F5-1C6A5067609C");

        public MarcasController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var dataContext = _context.Marcas
                .OrderBy(p => p.MarcaNombre);

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

            IQueryable<Marca> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Marcas
                                    .Where(p => p.MarcaNombre.Contains(w) ||
                                           p.MarcaDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.MarcaNombre.Contains(w) ||
                                                p.MarcaDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Marcas;
            }

            var almacenes = await query.OrderBy(m => m.MarcaNombre)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Marca>>(ViewData, almacenes)
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
        public async Task<IActionResult> Create([Bind("MarcaID,MarcaNombre,MarcaDescripcion")] Marca marca)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                marca.MarcaID = Guid.NewGuid();
                _context.Add(marca);
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos de la marca fueron almacenados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["toast"] = "Falta información en algún campo.";
            return View(marca);
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

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MarcaID,MarcaNombre,MarcaDescripcion")] Marca marca)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != marca.MarcaID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del almacén fueron actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.MarcaID))
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

            return View(marca);
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

            var marca = await _context.Marcas
                               .FirstOrDefaultAsync(m => m.MarcaID == id);

            if (marca == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();
            TempData["toast"] = "Los datos de la marca fueron eliminados correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(Guid id)
        {
            return _context.Marcas.Any(e => e.MarcaID == id);
        }
    }
}
