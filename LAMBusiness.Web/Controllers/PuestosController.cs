namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using LAMBusiness.Shared.Catalogo;
    using LAMBusiness.Web.Data;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class PuestosController : Controller
    {
        private readonly DataContext _context;

        public PuestosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Puestos.Include(p => p.Colaboradores));
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            IQueryable<Puesto> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Puestos.Include(m => m.Colaboradores)
                                    .Where(p => p.PuestoNombre.Contains(w) ||
                                           p.PuestoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.PuestoNombre.Contains(w) ||
                                                p.PuestoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Puestos.Include(m => m.Colaboradores);
            }

            var puestos = await query.OrderBy(m => m.PuestoNombre)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Puesto>>(ViewData, puestos)
            };

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                .FirstOrDefaultAsync(m => m.PuestoID == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PuestoID,PuestoNombre,PuestoDescripcion")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                puesto.PuestoID = Guid.NewGuid();
                _context.Add(puesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }
            return View(puesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PuestoID,PuestoNombre,PuestoDescripcion")] Puesto puesto)
        {
            if (id != puesto.PuestoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoExists(puesto.PuestoID))
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
            return View(puesto);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                               .Include(p => p.Colaboradores)
                               .FirstOrDefaultAsync(m => m.PuestoID == id);

            if (puesto == null)
            {
                return NotFound();
            }

            if(puesto.Colaboradores.Count > 0)
            {
                ModelState.AddModelError(string.Empty, $"El puesto no puede ser eliminado, tiene {puesto.Colaboradores.Count} colaborador(es)");
                return RedirectToAction(nameof(Index));
            }

            _context.Puestos.Remove(puesto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestoExists(Guid id)
        {
            return _context.Puestos.Any(e => e.PuestoID == id);
        }
    }
}
