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
    using Shared.Movimiento;

    public class EntradasController : Controller
    {
        private readonly DataContext _context;

        public EntradasController(DataContext context)
        {
            _context = context;
        }

        // GET: Entradas
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Entradas.Include(e => e.Proveedores);
            return View(await dataContext.ToListAsync());
        }

        // GET: Entradas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(m => m.EntradaID == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // GET: Entradas/Create
        public IActionResult Create()
        {
            ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ProveedorID", "Colonia");
            return View();
        }

        // POST: Entradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntradaID,ProveedorID,UsuarioID,Fecha,Folio,Observaciones,Aplicado,FechaCreacion,FechaActualizacion")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                entrada.EntradaID = Guid.NewGuid();
                _context.Add(entrada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ProveedorID", "Colonia", entrada.ProveedorID);
            return View(entrada);
        }

        // GET: Entradas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ProveedorID", "Colonia", entrada.ProveedorID);
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EntradaID,ProveedorID,UsuarioID,Fecha,Folio,Observaciones,Aplicado,FechaCreacion,FechaActualizacion")] Entrada entrada)
        {
            if (id != entrada.EntradaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.EntradaID))
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
            ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ProveedorID", "Colonia", entrada.ProveedorID);
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(m => m.EntradaID == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entrada = await _context.Entradas.FindAsync(id);
            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntradaExists(Guid id)
        {
            return _context.Entradas.Any(e => e.EntradaID == id);
        }
    }
}
