namespace LAMBusiness.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;

    public class GenerosController : Controller
    {
        private readonly DataContext _context;

        public GenerosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Generos.OrderBy(g => g.GeneroDescripcion));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GeneroID,GeneroDescripcion")] Genero genero)
        {
            if (id != genero.GeneroID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroExists(genero.GeneroID))
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
            return View(genero);
        }

        private bool GeneroExists(string id)
        {
            return _context.Generos.Any(e => e.GeneroID == id);
        }
    }
}
