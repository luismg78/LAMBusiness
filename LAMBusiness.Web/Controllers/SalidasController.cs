namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Catalogo;
    using Shared.Movimiento;

    public class SalidasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("D6DC97D9-C3DE-4920-A0B1-B63D7685BB6A");

        public SalidasController(DataContext context,
            ICombosHelper combosHelper,
            IGetHelper getHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration)
        {
            _context = context;
            _combosHelper = combosHelper;
            _getHelper = getHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            var dataContext = _context.Salidas
                .Include(e => e.SalidaTipo)
                .OrderBy(e => e.Folio);

            return View(dataContext);
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            IQueryable<Salida> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Salidas
                                .Include(e => e.SalidaTipo)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.SalidaTipo.SalidaTipoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(e => e.SalidaTipo)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.SalidaTipo.SalidaTipoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Salidas.Include(e => e.SalidaTipo);
            }

            var salidas = await query.OrderByDescending(m => m.FechaActualizacion)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Salida>>(ViewData, salidas)
            };
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            if (id == null)
            {
                return NotFound();
            }
            var salida = await _getHelper.GetSalidaByIdAsync((Guid)id);

            var salidaViewModel = await _converterHelper.ToSalidaViewModelAsync(salida);

            return View(salidaViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            var salidaViewModel = new SalidaViewModel()
            {
                SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync()
            };

            return View(salidaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalidaTipoID,Fecha,Folio,Observaciones")] SalidaViewModel salidaViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            ValidateFieldsAsync(salidaViewModel.SalidaTipoID);

            if (ModelState.IsValid)
            {
                var salida = await _converterHelper.ToSalidaAsync(salidaViewModel, true);
                _context.Add(salida);

                try
                {
                    _context.Add(salida);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = salida.SalidaID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            salidaViewModel.SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync();

            return View(salidaViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return NotFound();
            }

            if (SalidaAplicada((Guid)id))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var salida = await _context.Salidas
                .Include(e => e.SalidaTipo)
                .FirstOrDefaultAsync(s => s.SalidaID == id);

            if (salida == null)
            {
                return NotFound();
            }

            SalidaViewModel salidaViewModel = await _converterHelper.ToSalidaViewModelAsync(salida);

            return View(salidaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SalidaID,SalidaTipoID,Fecha,Folio,Observaciones")] SalidaViewModel salidaViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id != salidaViewModel.SalidaID)
            {
                return NotFound();
            }

            ValidateFieldsAsync(salidaViewModel.SalidaTipoID);

            if (ModelState.IsValid)
            {
                var salida = await _converterHelper.ToSalidaAsync(salidaViewModel, false);
                if(salida == null)
                {
                    ModelState.AddModelError("ModelOnly", "Identificador incorrecto.");
                    salidaViewModel.SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync();
                    return View(salidaViewModel);
                }

                try
                {
                    _context.Update(salida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalidaExists(salidaViewModel.SalidaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = salidaViewModel.SalidaID });
            }

            salidaViewModel.SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync();

            return View(salidaViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            if (SalidaAplicada((Guid)id))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var salida = await _context.Salidas
                .Include(s => s.SalidaTipo)
                .FirstOrDefaultAsync(s => s.SalidaID == id);

            if (salida == null)
            {
                return NotFound();
            }

            var salidaDetalle = await _context.SalidasDetalle
                .Where(s => s.SalidaID == id).ToListAsync();
            
            if (salidaDetalle.Any())
            {
                foreach(var s in salidaDetalle)
                {
                    _context.SalidasDetalle.Remove(s);
                }
            }

            _context.Salidas.Remove(salida);

            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "El registro ha sido eliminado correctamente.";
            }
            catch (Exception)
            {
                TempData["toast"] = "El registro no ha sido eliminado, verifique bitácora de errores.";
            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Apply(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return NotFound();
            }

            if (SalidaAplicada((Guid)id))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            var salida = await _getHelper.GetSalidaByIdAsync((Guid)id);

            if (salida == null)
            {
                return NotFound();
            }

            salida.Aplicado = true;
            _context.Update(salida);

            var detalle = await _getHelper.GetSalidaDetalleBySalidaIdAsync(salida.SalidaID);
            if (detalle == null)
            {
                //Ingresar al menos un movimiento
                return RedirectToAction(nameof(Details), new { id });
            }

            var _salidasDetalle = new List<SalidaDetalle>();

            foreach (var item in detalle)
            {
                Guid _almacenId = (Guid)item.AlmacenID;
                Guid _productoId = (Guid)item.ProductoID;
                decimal _cantidad = (decimal)item.Cantidad;

                if (item.Productos.Unidades.Pieza)
                {
                    _cantidad = (int)_cantidad;
                }

                if (item.Productos.Unidades.Paquete)
                {
                    _productoId = item.Productos.Paquete.PiezaProductoID;
                    _cantidad = item.Productos.Paquete.CantidadProductoxPaquete * _cantidad;
                }

                var _salidaDetalle = _salidasDetalle.FirstOrDefault(s => s.ProductoID == item.ProductoID &&
                                                                         s.AlmacenID == item.AlmacenID);
                if(_salidaDetalle == null)
                {
                    _salidasDetalle.Add(new SalidaDetalle()
                    {
                        Almacenes = item.Almacenes,
                        AlmacenID = item.AlmacenID,
                        Cantidad = item.Cantidad,
                        PrecioCosto = item.PrecioCosto,
                        ProductoID = item.ProductoID,
                        Productos = item.Productos,
                        SalidaDetalleID = item.SalidaDetalleID,
                        SalidaID = item.SalidaID,
                        Salidas = item.Salidas
                    });
                }
                else
                {
                    _salidaDetalle.Cantidad += item.Cantidad;
                }                
            }

            foreach(var s in _salidasDetalle)
            {
                var existencia = _context.Existencias
                    .FirstOrDefault(e => e.ProductoID == s.ProductoID && e.AlmacenID == s.AlmacenID);

                if (existencia == null)
                {
                    ModelState.AddModelError("", "");
                }
                else
                {
                    if((existencia.ExistenciaEnAlmacen - s.Cantidad) < 0)
                    {
                        TempData["toast"] = "La cantidad del producto excede la cantidad en inventario";
                        return RedirectToAction(nameof(Details), new { id });
                    }
                    else
                    {
                        existencia.ExistenciaEnAlmacen -= (decimal)s.Cantidad;
                        _context.Update(existencia);
                    }
                }
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

        private bool SalidaExists(Guid id)
        {
            return _context.Salidas.Any(s => s.SalidaID == id);
        }

        private bool SalidaAplicada(Guid id)
        {
            return _context.Salidas.Any(s => s.SalidaID == id && s.Aplicado == true);
        }

        public async Task<IActionResult> GetAlmacen(string almacenNombre)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (almacenNombre == null || almacenNombre == "")
            {
                return null;
            }

            var almacen = await _getHelper.GetAlmacenByNombreAsync(almacenNombre.Trim().ToUpper());
            if (almacen != null)
            {
                return Json(
                    new
                    {
                        almacen.AlmacenID,
                        almacen.AlmacenNombre,
                        almacen.AlmacenDescripcion,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Almacén inexistente" });

        }

        public async Task<IActionResult> GetAlmacenes(string pattern, int? skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (pattern == null || pattern == "" || skip == null)
            {
                return null;
            }

            var almacenes = await _getHelper.GetAlmacenesByPatternAsync(pattern, (int)skip);

            return new PartialViewResult
            {
                ViewName = "_GetAlmacenes",
                ViewData = new ViewDataDictionary
                            <List<Almacen>>(ViewData, almacenes)
            };
        }

        public async Task<IActionResult> GetProducto(string code)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (code == null || code == "")
            {
                return null;
            }

            var producto = await _getHelper.GetProductByCodeAsync(code.Trim().ToUpper());
            if (producto != null)
            {
                return Json(
                    new
                    {
                        producto.ProductoID,
                        producto.Codigo,
                        producto.ProductoNombre,
                        producto.PrecioCosto,
                        producto.PrecioVenta,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Producto inexistente" });

        }

        public async Task<IActionResult> GetProductos(string pattern, int? skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

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

        //Detalle de movimientos

        public async Task<IActionResult> AddDetails(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return NotFound();
            }

            if (SalidaAplicada((Guid)id))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(new SalidaDetalle()
            {
                SalidaID = (Guid)id,
                Cantidad = 0,
                PrecioCosto = 0,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetails(SalidaDetalle salidaDetalle)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (SalidaAplicada(salidaDetalle.SalidaID))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = salidaDetalle.SalidaID });
            }

            if (salidaDetalle.AlmacenID == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(salidaDetalle);
            }

            if (salidaDetalle.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(salidaDetalle);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)salidaDetalle.AlmacenID);
            var producto = await _getHelper.GetProductByIdAsync((Guid)salidaDetalle.ProductoID);

            if (ModelState.IsValid)
            {
                if (almacen == null)
                {
                    ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                    return View(salidaDetalle);
                }

                if (producto == null)
                {
                    ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                    return View(salidaDetalle);
                }

                var existencia = await _context.Existencias
                    .FirstOrDefaultAsync(e => e.ProductoID == producto.ProductoID && 
                                              e.AlmacenID == almacen.AlmacenID);

                salidaDetalle.Almacenes = almacen;
                salidaDetalle.Productos = producto;

                var cantidadTotalPorProducto = await _context.SalidasDetalle
                    .Where(s => s.SalidaID == salidaDetalle.SalidaID && 
                                s.ProductoID == salidaDetalle.ProductoID &&
                                s.AlmacenID == salidaDetalle.AlmacenID)
                    .SumAsync(s => s.Cantidad);

                if(existencia == null || 
                  (existencia.ExistenciaEnAlmacen - (cantidadTotalPorProducto + salidaDetalle.Cantidad)) < 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad excede a la cantidad en inventario.");
                    return View(salidaDetalle);
                }

                try
                {
                    salidaDetalle.SalidaDetalleID = Guid.NewGuid();

                    if (producto.Unidades.Pieza)
                    {
                        salidaDetalle.Cantidad = (int)salidaDetalle.Cantidad;
                    }

                    salidaDetalle.PrecioCosto = producto.PrecioCosto;

                    _context.Add(salidaDetalle);

                    await _context.SaveChangesAsync();

                    ModelState.AddModelError(string.Empty, "Registro almacenado.");
                    return View(new SalidaDetalle()
                    {
                        AlmacenID = salidaDetalle.AlmacenID,
                        Almacenes = almacen,
                        SalidaID = salidaDetalle.SalidaID,
                        Cantidad = 0,
                        PrecioCosto = 0,
                    });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            salidaDetalle.Almacenes = almacen;
            salidaDetalle.Productos = producto;
            return View(salidaDetalle);
        }

        public async Task<IActionResult> EditDetails(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _getHelper.GetSalidaDetalleByIdAsync((Guid)id);

            if (SalidaAplicada(detalle.SalidaID))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = detalle.SalidaID });
            }

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(SalidaDetalle salidaDetalle)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (SalidaAplicada(salidaDetalle.SalidaID))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = salidaDetalle.SalidaID });
            }

            if (salidaDetalle.AlmacenID == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(salidaDetalle);
            }

            if (salidaDetalle.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(salidaDetalle);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)salidaDetalle.AlmacenID);
            var producto = await _getHelper.GetProductByIdAsync((Guid)salidaDetalle.ProductoID);

            if (ModelState.IsValid)
            {
                if (almacen == null)
                {
                    ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                    return View(salidaDetalle);
                }

                if (producto == null)
                {
                    ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                    return View(salidaDetalle);
                }

                if (producto.Unidades.Pieza)
                {
                    salidaDetalle.Cantidad = (int)salidaDetalle.Cantidad;
                }

                var existencia = await _context.Existencias
                    .FirstOrDefaultAsync(e => e.ProductoID == producto.ProductoID &&
                                              e.AlmacenID == almacen.AlmacenID);

                if (existencia == null || (existencia.ExistenciaEnAlmacen - salidaDetalle.Cantidad) < 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad excede a la cantidad en inventario.");
                    return View(salidaDetalle);
                }

                if (producto.Unidades.Pieza)
                {
                    salidaDetalle.Cantidad = (int)salidaDetalle.Cantidad;
                }

                salidaDetalle.PrecioCosto = producto.PrecioCosto;

                try
                {
                    _context.Update(salidaDetalle);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = salidaDetalle.SalidaID });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            salidaDetalle.Productos = producto;

            return View(salidaDetalle);
        }

        public async Task<IActionResult> DeleteDetails(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                return NotFound();
            }

            var detalle = await _getHelper.GetSalidaDetalleByIdAsync((Guid)id);

            if (detalle == null)
            {
                return NotFound();
            }

            if (SalidaAplicada(detalle.SalidaID))
            {
                //Salida aplicada no se permiten cambios.
                return RedirectToAction(nameof(Details), new { id = detalle.SalidaID });
            }

            _context.SalidasDetalle.Remove(detalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = detalle.SalidaID });
        }

        private void ValidateFieldsAsync(Guid? salidaTipoId)
        {
            if (salidaTipoId == null || salidaTipoId == Guid.Empty)
            {
                ModelState.Clear();
                ModelState.AddModelError("SalidaTipoID", "El campo Tipo de Salida es requerido");
            }
        }
    }
}
