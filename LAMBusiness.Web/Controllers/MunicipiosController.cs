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

    public class MunicipiosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("46FDCC81-6AC7-4BE4-84F3-4ABAE3A40EBB");

        public MunicipiosController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var municipios = _context.Municipios
                .Include(m => m.Estados)
                .OrderBy(m => m.EstadoID)
                .ThenBy(m => m.MunicipioDescripcion);
            return View(municipios);
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

            IQueryable<Municipio> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Municipios.Include(m => m.Estados)
                                    .Where(m => m.MunicipioDescripcion.Contains(w) ||
                                           m.Estados.EstadoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(m => m.MunicipioDescripcion.Contains(w) ||
                                                m.Estados.EstadoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if(query == null)
            {
                query = _context.Municipios.Include(m => m.Estados);
            }

            var municipios = await query.OrderBy(m => m.EstadoID)
                .ThenBy(m => m.MunicipioDescripcion)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Municipio>>(ViewData, municipios)
            };

        }
        
        public async Task<IActionResult> Edit(int? id)
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

            var municipio = await _context.Municipios
                .Include(m => m.Estados)
                .FirstOrDefaultAsync(m => m.MunicipioID == id);

            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MunicipioID,EstadoID,MunicipioClave,MunicipioDescripcion")] Municipio municipio)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != municipio.MunicipioID)
            {
                return NotFound();
            }

            var municipioUpdate = await _context.Municipios
                        .Include(m => m.Estados)
                        .FirstOrDefaultAsync(m => m.MunicipioID == municipio.MunicipioID);

            if (ModelState.IsValid)
            {
                
                municipioUpdate.MunicipioDescripcion = municipio.MunicipioDescripcion.Trim().ToUpper();
                try
                {
                    _context.Update(municipioUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MunicipioExists(municipio.MunicipioID))
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
            
            return View(municipioUpdate);
        }

        private bool MunicipioExists(int id)
        {
            return _context.Municipios.Any(e => e.MunicipioID == id);
        }       
    }
}