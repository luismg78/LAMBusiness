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

    public class MarcasController : Controller
    {
        private readonly DataContext _context;

        public MarcasController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dataContext = _context.Marcas
                .OrderBy(p => p.MarcaNombre);

            return View(dataContext);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarcaID,MarcaNombre,MarcaDescripcion")] Marca almacen)
        {
            if (ModelState.IsValid)
            {
                almacen.MarcaID = Guid.NewGuid();
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

            var almacen = await _context.Marcas.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MarcaID,MarcaNombre,MarcaDescripcion")] Marca almacen)
        {
            if (id != almacen.MarcaID)
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
                    if (!MarcaExists(almacen.MarcaID))
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

            var almacen = await _context.Marcas
                               .FirstOrDefaultAsync(m => m.MarcaID == id);

            if (almacen == null)
            {
                return NotFound();
            }

            _context.Marcas.Remove(almacen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(Guid id)
        {
            return _context.Marcas.Any(e => e.MarcaID == id);
        }
    }
}
