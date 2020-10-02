namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using LAMBusiness.Shared.Contacto;
    using LAMBusiness.Web.Data;
    using LAMBusiness.Web.Models.ViewModels;
    using LAMBusiness.Web.Helpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using LAMBusiness.Shared.Catalogo;

    public class ClientesController : Controller
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ClientesController(DataContext context, 
            IGetHelper getHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Clientes.Include(c => c.Municipios);
            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public async Task<IActionResult> Create()
        {
            var clienteViewModel = new ClienteViewModel()
            {
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(0)
            };

            return View(clienteViewModel);
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clienteViewModel)
        {
            if (ModelState.IsValid)
            {
                var cliente = await _converterHelper.ToClienteAsync(clienteViewModel, true);
                _context.Add(cliente);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = cliente.ClienteID});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            clienteViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            clienteViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)clienteViewModel.EstadoID);

            return View(clienteViewModel);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["MunicipioID"] = new SelectList(_context.Municipios, "MunicipioID", "MunicipioDescripcion", cliente.MunicipioID);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClienteID,RFC,Nombre,Contacto,Domicilio,Colonia,CodigoPostal,MunicipioID,Telefono,TelefonoMovil,Email,FechaRegistro,Activo")] Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteID))
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
            ViewData["MunicipioID"] = new SelectList(_context.Municipios, "MunicipioID", "MunicipioDescripcion", cliente.MunicipioID);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Municipios)
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddMunicipios(short? id) 
        {
            if(id == null)
            {
                return null;
            }

            var municipios = await _getHelper.GetMunicipiosByEstadoIdAsync((short)id);

            return new PartialViewResult
            {
                ViewName = "_AddComboMunicipios",
                ViewData = new ViewDataDictionary
                            <List<Municipio>>(ViewData, municipios)
            };
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }
    }
}
