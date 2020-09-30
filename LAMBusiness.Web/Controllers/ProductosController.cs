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
    using LAMBusiness.Web.Helpers;
    using LAMBusiness.Web.Models.ViewModels;

    public class ProductosController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductosController(DataContext context, 
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            var dataContext = _context.Productos
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .Include(p => p.Paquete);

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

            var productoViewModel = await _converterHelper.ToProductosViewModelAsync(producto);

            return View(productoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var productoViewModel = new ProductoViewModel() { 
                UnidadesDDL = await _combosHelper.GetComboUnidadesAsync(),
                TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync()
            };

            if(productoViewModel.TasasImpuestosDDL.Count() <= 1)
            {
                ModelState.AddModelError(string.Empty, "No existe tasa de impuestos para asignar a productos.");
                return RedirectToAction(nameof(Create), "TasasImpuestos");
            }

            return View(productoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel productoViewModel)
        {
            await ValidateFieldsAsync(productoViewModel);
            if (ModelState.IsValid)
            {
                var producto = await _converterHelper.ToProductoAsync(productoViewModel, true);
                _context.Add(producto);

                bool paqueteExists = PaqueteExists(producto.ProductoID);

                if(producto.Paquete != null)
                {
                    if (paqueteExists)
                        _context.Update(producto.Paquete);
                    else
                        _context.Add(producto.Paquete);
                }
                else
                {
                    if (paqueteExists)
                    {
                        _context.Remove(producto.Paquete);
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = producto.ProductoID});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            productoViewModel.TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync();
            productoViewModel.UnidadesDDL = await _combosHelper.GetComboUnidadesAsync();
            return View(productoViewModel);
        }

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

            var productoViewModel = await _converterHelper.ToProductosViewModelAsync(producto);

            return View(productoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductoViewModel productoViewModel)
        {
            if (id != productoViewModel.ProductoID)
            {
                return NotFound();
            }

            await ValidateFieldsAsync(productoViewModel);

            if (ModelState.IsValid)
            {
                try
                {
                    var producto = await _converterHelper.ToProductoAsync(productoViewModel, false);
                    var paquete = producto.Paquete;
                    _context.Update(producto);

                    bool paqueteExists = PaqueteExists(producto.ProductoID);

                    if (paquete != null)
                    {
                        if (paqueteExists)
                            _context.Update(paquete);
                        else
                            _context.Add(paquete);
                    }
                    else
                    {
                        if (paqueteExists)
                        {
                            paquete = await _context.Paquetes
                                .FirstOrDefaultAsync(p => p.ProductoID == producto.ProductoID);
                            _context.Remove(paquete);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = producto.ProductoID});

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProductoExists(productoViewModel.ProductoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                        return View();
                    }
                }
            }
            
            productoViewModel.TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync();
            productoViewModel.UnidadesDDL = await _combosHelper.GetComboUnidadesAsync();

            return View(productoViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
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

            var paquete = await _context.Paquetes.FindAsync(producto.ProductoID);
            if (paquete != null)
            {
                _context.Paquetes.Remove(paquete);
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaqueteExists(Guid id)
        {
            return _context.Paquetes.Any(e => e.ProductoID == id);
        }

        private bool ProductoExists(Guid id)
        {
            return _context.Productos.Any(e => e.ProductoID == id);
        }

        private async Task ValidateFieldsAsync(ProductoViewModel productoViewModel)
        {
            //validate if register exist with that value
            var codeexist = await _context.Productos
                .FirstOrDefaultAsync(p => p.Codigo == productoViewModel.Codigo && 
                                     p.ProductoID != productoViewModel.ProductoID);
            if (codeexist != null)
            {
                ModelState.AddModelError("Codigo", "Ya existe un producto con este código.");
            }
            //validate field Tasa de Impuesto 
            if (productoViewModel.TasaID == Guid.Empty)
            {
                ModelState.AddModelError("TasaID", "El campo Tasa de Impuesto es requerido");
            }
            //validate field Unidad
            if (productoViewModel.UnidadID == Guid.Empty)
            {
                ModelState.AddModelError("UnidadID", "El campo Unidad es requerido");
            }
            //if field unidad have value, then, validate fields CodigoPieza and Cantidad
            if (productoViewModel.UnidadID == Guid.Parse("6C9C7801-D654-11E9-8B00-8CDCD47D68A1") ||
                productoViewModel.UnidadID == Guid.Parse("95B850EC-D654-11E9-8B00-8CDCD47D68A1"))
            {
                if (string.IsNullOrEmpty(productoViewModel.CodigoPieza))
                {
                    ModelState.AddModelError("CodigoPieza", "El campo Unidad es requerido");
                }
                //if field CodigoPieza have value, then, validate if product exist
                var codepieceexist = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Codigo == productoViewModel.CodigoPieza);
                if (codepieceexist == null)
                {
                    ModelState.AddModelError("CodigoPieza", "Código de producto inexistente.");
                }
                else {
                    //validate that producto be piece
                    if (codepieceexist.UnidadID != Guid.Parse("401B9552-D654-11E9-8B00-8CDCD47D68A1") &&
                        codepieceexist.UnidadID != Guid.Parse("826671FC-D654-11E9-8B00-8CDCD47D68A1"))
                    {
                        ModelState.AddModelError("CodigoPieza", "El producto no puede ser caja.");
                    }
                    else
                    {
                        //validate package and piece be whole pieces
                        if (productoViewModel.UnidadID == Guid.Parse("6C9C7801-D654-11E9-8B00-8CDCD47D68A1"))
                        {
                            if (codepieceexist.UnidadID != Guid.Parse("401B9552-D654-11E9-8B00-8CDCD47D68A1"))
                            {
                                ModelState.AddModelError("CodigoPieza", "No coincide con la unidad del paquete.");
                            }
                        }
                        //validate package and piece be bulk pieces
                        else if (productoViewModel.UnidadID == Guid.Parse("95B850EC-D654-11E9-8B00-8CDCD47D68A1"))
                        {
                            if (codepieceexist.UnidadID != Guid.Parse("826671FC-D654-11E9-8B00-8CDCD47D68A1"))
                            {
                                ModelState.AddModelError("CodigoPieza", "No coincide con la unidad del paquete.");
                            }
                        }
                    }
                }

                if (productoViewModel.CantidadProductoxPaquete == null ||
                    productoViewModel.CantidadProductoxPaquete <= 0)
                {
                    ModelState.AddModelError("CantidadProductoxPaquete", "El campo Unidad es requerido");
                }
            }
        }
    }
}
