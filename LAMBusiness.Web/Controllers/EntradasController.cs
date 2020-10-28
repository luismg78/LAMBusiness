namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Helpers;
    using Shared.Contacto;
    using Shared.Movimiento;
    using LAMBusiness.Shared.Catalogo;
    using LAMBusiness.Web.Models.ViewModels;
    using Microsoft.CodeAnalysis.Operations;

    public class EntradasController : Controller
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;

        public EntradasController(DataContext context, 
            IGetHelper getHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _getHelper = getHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            var dataContext = _context.Entradas
                .Include(e => e.Proveedores)
                .OrderBy(e => e.Folio)
                .ThenBy(e => e.Proveedores.Nombre);

            return View(dataContext);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

            var entradaViewModel = await _converterHelper.ToEntradaViewModelAsync(entrada);

            return View(entradaViewModel);
        }

        public IActionResult Create()
        {
            ViewData["ProveedorID"] = new SelectList(_context.Proveedores, "ProveedorID", "Colonia");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                entrada.EntradaID = Guid.NewGuid();
                entrada.Aplicado = false;
                entrada.FechaActualizacion = DateTime.Now;
                entrada.FechaCreacion = DateTime.Now;
                entrada.Folio = entrada.Folio.Trim().ToUpper();
                entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();
                entrada.UsuarioID = Guid.Empty;
                try
                {
                    _context.Add(entrada);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View(entrada);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (EntradaAplicada((Guid)id))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(e => e.EntradaID == id);

            if (entrada == null)
            {
                return NotFound();
            }
            
            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EntradaID,ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
        {
            if (id != entrada.EntradaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var _entrada = await _context.Entradas.FindAsync(entrada.EntradaID);

                _entrada.FechaActualizacion = DateTime.Now;
                _entrada.ProveedorID = entrada.ProveedorID;
                _entrada.Folio = entrada.Folio.Trim().ToUpper();
                _entrada.Fecha = entrada.Fecha;
                _entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();

                try
                {
                    _context.Update(_entrada);
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
                return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
            }

            return View(entrada);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (EntradaAplicada((Guid)id))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(m => m.EntradaID == id);
            
            if (entrada == null)
            {
                return NotFound();
            }

            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Apply(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (EntradaAplicada((Guid)id))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

            if (entrada == null)
            {
                return NotFound();
            }

            entrada.Aplicado = true;
            _context.Update(entrada);

            var detalle = await _getHelper.GetEntradaDetalleByEntradaIdAsync(entrada.EntradaID);
            if(detalle == null)
            {
                //Ingresar al menos un movimiento
                return RedirectToAction(nameof(Details), new { id });
            }
            
            var existencias = new List<ExistenciaViewModel>();

            foreach(var item in detalle)
            {
                Guid _productoId = (Guid)item.ProductoID;
                decimal _cantidad = (decimal)item.Cantidad;
                decimal _precioCosto = (decimal)item.PrecioCosto;

                if(item.Productos.Unidades.Pieza)
                {
                    _cantidad = (int)_cantidad;
                }

                if (item.Productos.Unidades.Paquete)
                {
                    _productoId = item.Productos.Paquete.PiezaProductoID;
                    _precioCosto = (decimal)item.PrecioCosto / item.Productos.Paquete.CantidadProductoxPaquete;
                    _cantidad = item.Productos.Paquete.CantidadProductoxPaquete * _cantidad;
                    item.Productos.PrecioCosto = (decimal)item.PrecioCosto;
                }
                item.Productos.PrecioVenta = (decimal)item.PrecioVenta;

                var existencia = existencias.FirstOrDefault(e => e.ProductoID == _productoId);
                if(existencia == null)
                {
                    existencias.Add(new ExistenciaViewModel() { 
                        AlmacenID = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F"),
                        ExistenciaEnAlmacen = _cantidad,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = _productoId,
                        PrecioCosto = _precioCosto
                    });
                }
                else
                {
                    existencia.PrecioCosto = (
                        (existencia.ExistenciaEnAlmacen * existencia.PrecioCosto) + 
                        (_cantidad * _precioCosto)
                        ) / (existencia.ExistenciaEnAlmacen + _cantidad);
                    existencia.ExistenciaEnAlmacen += _cantidad;
                }
                
            }

            foreach(var item in existencias)
            {
                var producto = await _getHelper.GetProductByIdAsync(item.ProductoID);
                if(producto == null)
                {
                    //Producto inexistente
                    break;
                }
                decimal _existencia = 0;
                var existencia = await _getHelper.GetExistenciaByProductoIdAsync(item.ProductoID);
                if (existencia == null)
                {
                    _context.Existencias.Add(new Existencia()
                    {
                        AlmacenID = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F"),
                        ExistenciaEnAlmacen = item.ExistenciaEnAlmacen,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = item.ProductoID
                    });
                    producto.PrecioCosto = item.PrecioCosto;
                }
                else
                {
                    _existencia = existencia.ExistenciaEnAlmacen;
                    existencia.ExistenciaEnAlmacen += item.ExistenciaEnAlmacen;
                    _context.Update(existencia);
                    producto.PrecioCosto = (
                            (_existencia * producto.PrecioCosto) +
                            (item.ExistenciaEnAlmacen * item.PrecioCosto)
                            ) / (existencia.ExistenciaEnAlmacen);
                }


                _context.Update(producto);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                throw;
            }

            return RedirectToAction(nameof(Details), new { id });

        }

        private bool EntradaExists(Guid id)
        {
            return _context.Entradas.Any(e => e.EntradaID == id);
        }

        private bool EntradaAplicada(Guid id)
        {
            return _context.Entradas.Any(e => e.EntradaID == id && e.Aplicado == true);
        }

        public async Task<IActionResult> GetProducto(string code)
        {
            if (code == null || code == "")
            {
                return null;
            }

            var producto = await _getHelper.GetProductByCodeAsync(code.Trim().ToUpper());
            if (producto != null)
            {
                return Json(
                    new { 
                        producto.ProductoID, 
                        producto.Codigo,
                        producto.ProductoNombre,
                        producto.PrecioCosto,
                        producto.PrecioVenta,
                        error = false });
            }

            return Json(new { error = true, message = "Producto inexistente" });

        }

        public async Task<IActionResult> GetProductos(string pattern, int? skip)
        {
            if (pattern == null || pattern == "" || skip == null)
            {
                return null;
            }

            var productos = await _getHelper.GetProductosByPatternAsync(pattern, (int)skip);

            return new PartialViewResult
            {
                ViewName = "_GetProductos",
                ViewData = new ViewDataDictionary
                            <List<Producto>>(ViewData, productos)
            };
        }

        public async Task<IActionResult> GetProveedor(string rfc)
        {
            if (rfc == null || rfc == "")
            {
                return null;
            }

            var proveedor = await _getHelper.GetProveedorByRFCAsync(rfc);
            if (proveedor != null)
            {
                return Json(
                    new
                    {
                        proveedor.ProveedorID,
                        proveedor.RFC,
                        proveedor.Nombre,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Proveedor inexistente" });

        }

        public async Task<IActionResult> GetProveedores(string pattern, int? skip)
        {
            if (pattern == null || pattern == "" || skip == null)
            {
                return null;
            }

            var proveedores = await _getHelper.GetProveedoresByPatternAsync(pattern, (int)skip);

            return new PartialViewResult
            {
                ViewName = "_GetProveedores",
                ViewData = new ViewDataDictionary
                            <List<Proveedor>>(ViewData, proveedores)
            };
        }

        //Detalle de movimientos

        public IActionResult AddDetalle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (EntradaAplicada((Guid)id))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(new EntradaDetalle()
            {
                EntradaID = (Guid)id,
                Cantidad = 0,
                PrecioCosto = 0,
                PrecioVenta = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetalle(EntradaDetalle entradaDetalle)
        {
            if (EntradaAplicada(entradaDetalle.EntradaID))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
            }

            var producto = await _getHelper.GetProductByIdAsync((Guid)entradaDetalle.ProductoID);

            if (ModelState.IsValid)
            {
                if (producto == null)
                {
                    ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                    return View(entradaDetalle);
                }

                try
                {
                    entradaDetalle.EntradaDetalleID = Guid.NewGuid();
                    if(producto.Unidades.Pieza)
                    {
                        entradaDetalle.Cantidad = (int)entradaDetalle.Cantidad;
                    }

                    _context.Add(entradaDetalle);

                    await _context.SaveChangesAsync();

                    ModelState.AddModelError(string.Empty, "Registro almacenado.");
                    return View(new EntradaDetalle() { 
                        EntradaID = entradaDetalle.EntradaID,
                        Cantidad = 0,
                        PrecioCosto = 0,
                        PrecioVenta = 0
                    });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            entradaDetalle.Productos = producto;
            return View(entradaDetalle);
        }

        public async Task<IActionResult> EditDetalle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);
            
            if (EntradaAplicada(detalle.EntradaID))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
            }

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetalle(EntradaDetalle entradaDetalle)
        {
            if (EntradaAplicada(entradaDetalle.EntradaID))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
            }

            if (entradaDetalle.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(entradaDetalle);
            }

            var producto = await _getHelper.GetProductByIdAsync((Guid)entradaDetalle.ProductoID);

            if (ModelState.IsValid)
            {

                if (producto == null)
                {
                    ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                    return View(entradaDetalle);
                }

                if (producto.Unidades.Pieza)
                {
                    entradaDetalle.Cantidad = (int)entradaDetalle.Cantidad;
                }

                try
                {
                    _context.Update(entradaDetalle);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            entradaDetalle.Productos = producto;

            return View(entradaDetalle);
        }

        public async Task<IActionResult> DeleteDetalle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);

            if (detalle == null)
            {
                return NotFound();
            }

            if (EntradaAplicada(detalle.EntradaID))
            {
                //Entrada aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
            }

            _context.EntradasDetalle.Remove(detalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
        }
    }
}
