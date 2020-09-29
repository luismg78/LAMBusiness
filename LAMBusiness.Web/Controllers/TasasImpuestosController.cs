namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;

    public class TasasImpuestosController : Controller
    {
        private readonly DataContext _context;

        public TasasImpuestosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasaImpuesto = _context.TasasImpuestos
                .Include(t => t.Productos)
                .OrderBy(t => t.Tasa);

            return View(tasaImpuesto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TasaID,Tasa,Porcentaje,TasaDescripcion")] TasaImpuesto tasaImpuesto)
        {
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
