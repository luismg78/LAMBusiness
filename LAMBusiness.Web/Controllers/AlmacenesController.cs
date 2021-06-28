namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Data;
    using Shared.Catalogo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class AlmacenesController : Controller
    {
        private readonly DataContext _context;

        public AlmacenesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dataContext = _context.Almacenes
                .OrderBy(p => p.AlmacenNombre);

            return View(dataContext);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlmacenID,AlmacenNombre,AlmacenDescripcion")] Almacen almacen)
        {
            if (ModelState.IsValid)
            {
                almacen.AlmacenID = Guid.NewGuid();
                _context.Add(almacen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(almacen);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlmacenID,AlmacenNombre,AlmacenDescripcion")] Almacen almacen)
        {
            if (id != almacen.AlmacenID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmacenExists(almacen.AlmacenID))
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
            return View(almacen);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _context.Almacenes
                               .FirstOrDefaultAsync(m => m.AlmacenID == id);

            if (almacen == null)
            {
                return NotFound();
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
            return RedirectToAction(nameof(Index));
        }

        private bool AlmacenExists(Guid id)
        {
            return _context.Almacenes.Any(e => e.AlmacenID == id);
        }
    }
}
