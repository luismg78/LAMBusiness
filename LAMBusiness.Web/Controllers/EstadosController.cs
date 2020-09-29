namespace LAMBusiness.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Shared.Catalogo;
    using Data;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System.Collections.Generic;

    public class EstadosController : Controller
    {
        private readonly DataContext _context;

        public EstadosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var estados = _context.Estados
                .Include(e => e.Municipios)
                .OrderBy(e => e.EstadoDescripcion);

            return View(estados);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            IQueryable<Estado> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Estados.Include(e => e.Municipios)
                                    .Where(e => e.EstadoClave.Contains(w) ||
                                           e.EstadoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(e => e.EstadoClave.Contains(w) ||
                                                e.EstadoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Estados.Include(e => e.Municipios);
            }

            var estados = await query.OrderBy(m => m.EstadoDescripcion)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Estado>>(ViewData, estados)
            };

        }
        
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("EstadoID,EstadoClave,EstadoDescripcion")] Estado estado)
        {
            if (id != estado.EstadoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoExists(estado.EstadoID))
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
            return View(estado);
        }

        private bool EstadoExists(short id)
        {
            return _context.Estados.Any(e => e.EstadoID == id);
        }
    }
}
