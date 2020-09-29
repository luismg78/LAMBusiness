namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;

    public class ProductosController : Controller
    {
        private readonly DataContext _context;

        public ProductosController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dataContext = _context.Productos
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades);

            return View(dataContext);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(m => m.ProductoID == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult Create()
        {
            ViewData["TasaID"] = new SelectList(_context.TasasImpuestos.OrderBy(t => t.Tasa), "TasaID", "Tasa");
            ViewData["UnidadID"] = new SelectList(_context.Unidades.OrderBy(u => u.UnidadNombre), "UnidadID", "UnidadNombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoID,Codigo,ProductoNombre,ProductoDescripcion,UnidadID,TasaID,Existencia,PrecioCosto,PrecioVenta,ExistenciaMaxima,Activo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.ProductoID = Guid.NewGuid();
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TasaID"] = new SelectList(_context.TasasImpuestos.OrderBy(t => t.Tasa), "TasaID", "Tasa", producto.TasaID);
            ViewData["UnidadID"] = new SelectList(_context.Unidades.OrderBy(u => u.UnidadNombre), "UnidadID", "UnidadNombre", producto.UnidadID);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["TasaID"] = new SelectList(_context.TasasImpuestos, "TasaID", "Tasa", producto.TasaID);
            ViewData["UnidadID"] = new SelectList(_context.Unidades, "UnidadID", "UnidadDescripcion", producto.UnidadID);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductoID,Codigo,ProductoNombre,ProductoDescripcion,UnidadID,TasaID,Existencia,PrecioCosto,PrecioVenta,ExistenciaMaxima,Activo")] Producto producto)
        {
            if (id != producto.ProductoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoID))
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
            ViewData["TasaID"] = new SelectList(_context.TasasImpuestos, "TasaID", "Tasa", producto.TasaID);
            ViewData["UnidadID"] = new SelectList(_context.Unidades, "UnidadID", "UnidadDescripcion", producto.UnidadID);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(m => m.ProductoID == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(Guid id)
        {
            return _context.Productos.Any(e => e.ProductoID == id);
        }
    }
}
