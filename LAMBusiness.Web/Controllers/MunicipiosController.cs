namespace LAMBusiness.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;

    public class MunicipiosController : Controller
    {
        private readonly DataContext _context;

        public MunicipiosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var municipios = _context.Municipios
                .Include(m => m.Estados)
                .OrderBy(m => m.EstadoID)
                .ThenBy(m => m.MunicipioDescripcion);
            return View(municipios);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip) {
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