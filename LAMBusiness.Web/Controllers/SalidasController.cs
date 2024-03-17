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
    using LAMBusiness.Contextos;
    using Helpers;
    using Interfaces;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Movimiento;
    using LAMBusiness.Backend;

    public class SalidasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private readonly IDashboard _dashboard;
        private readonly Productos _productos;
        private Guid moduloId = Guid.Parse("D6DC97D9-C3DE-4920-A0B1-B63D7685BB6A");

        public SalidasController(DataContext context,
            ICombosHelper combosHelper,
            IGetHelper getHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration,
            IDashboard dashboard)
        {
            _context = context;
            _combosHelper = combosHelper;
            _getHelper = getHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
            _dashboard = dashboard;
            _productos = new Productos(context);
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Index", "Home");
            }

            var salidas = _context.Salidas
                .Include(e => e.SalidaTipo)
                .OrderByDescending(e => e.FechaActualizacion)
                .ThenBy(e => e.Folio);

            var filtro = new Filtro<List<Salida>>()
            {
                Datos = await salidas.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await salidas.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Salida>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Salida> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Salidas
                                .Include(e => e.SalidaTipo)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.SalidaTipo.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(e => e.SalidaTipo)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.SalidaTipo.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Salidas.Include(e => e.SalidaTipo);
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderByDescending(e => e.FechaActualizacion)
                .ThenBy(e => e.Folio)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoLectura;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Salida>>>(ViewData, filtro)
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

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var salida = await _getHelper.GetSalidaByIdAsync((Guid)id);

            if (salida == null)
            {
                TempData["toast"] = "Identificador de la salida inexistente.";
                return RedirectToAction(nameof(Index));
            }

            var salidaViewModel = await _converterHelper.ToSalidaViewModelAsync(salida);
            
            salidaViewModel.PermisoEscritura = permisosModulo.PermisoEscritura;

            return View(salidaViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada(salidaViewModel.SalidaID))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = salidaViewModel.SalidaID });
            }

            TempData["toast"] = "Falta información en algún campo.";
            ValidateFieldsAsync(salidaViewModel.SalidaTipoID);

            if (ModelState.IsValid)
            {
                salidaViewModel.UsuarioID = token.UsuarioID;
                var salida = await _converterHelper.ToSalidaAsync(salidaViewModel, true);

                try
                {
                    _context.Add(salida);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la salida se almacenaron correctamente.";
                    await BitacoraAsync("Alta", salida);
                    return RedirectToAction(nameof(Details), new { id = salida.SalidaID });
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "Error al guardar salida, verifique bitácora de errores.";
                    await BitacoraAsync("Alta", salida, excepcion);
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
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada((Guid)id))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var salida = await _context.Salidas
                .Include(e => e.SalidaTipo)
                .FirstOrDefaultAsync(s => s.SalidaID == id);

            if (salida == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != salidaViewModel.SalidaID)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }
            if (SalidaAplicada((Guid)id))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["toast"] = "Falta información en algún campo.";
            ValidateFieldsAsync(salidaViewModel.SalidaTipoID);

            if (ModelState.IsValid)
            {
                salidaViewModel.UsuarioID = token.UsuarioID;
                var salida = await _converterHelper.ToSalidaAsync(salidaViewModel, false);
                if(salida == null)
                {
                    TempData["toast"] = "Identificador incorrecto.";
                    salidaViewModel.SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync();
                    return View(salidaViewModel);
                }

                try
                {
                    _context.Update(salida);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la salida se actualizaron correctamente.";
                    await BitacoraAsync("Actualizar", salida);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", salida, excepcion);
                    if (!SalidaExists(salidaViewModel.SalidaID))
                    {
                        TempData["toast"] = "Identificador de la salida inexistente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos de la salida no fueron actualizados.";
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var salida = await _context.Salidas
                .Include(s => s.SalidaTipo)
                .FirstOrDefaultAsync(s => s.SalidaID == id);

            if (salida == null)
            {
                TempData["toast"] = "Identificador incorrecto, entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada((Guid)id))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
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
                TempData["toast"] = "Los datos de la salida fueron eliminados correctamente.";
                await BitacoraAsync("Baja", salida);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos de la salida no fueron eliminados.";
                await BitacoraAsync("Baja", salida);
            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Apply(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            //cambiar por permiso para aplicar que se debe agregar como acción.
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada((Guid)id))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var salida = await _getHelper.GetSalidaByIdAsync((Guid)id);

            if (salida == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            salida.Aplicado = true;
            _context.Update(salida);

            var detalle = await _getHelper.GetSalidaDetalleBySalidaIdAsync(salida.SalidaID);
            if (detalle == null)
            {
                TempData["toast"] = "Por favor, ingrese al menos un movimiento.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var _salidasDetalle = new List<SalidaDetalle>();
            Dictionary<Guid, decimal> importePorAlmacen = new Dictionary<Guid, decimal>();

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

                var _salidaDetalle = _salidasDetalle
                    .FirstOrDefault(s => s.ProductoID == item.ProductoID &&
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

                decimal importe = Convert.ToDecimal(item.Cantidad) * Convert.ToDecimal(item.PrecioCosto);

                if (!importePorAlmacen.ContainsKey(_almacenId))
                {
                    importePorAlmacen.Add(_almacenId, importe);
                }
                else
                {
                    importePorAlmacen[_almacenId] += importe;
                }
            }

            foreach (KeyValuePair<Guid, decimal> keyValuePair in importePorAlmacen)
            {
                EstadisticaMovimientoViewModel estadisticaMovimiento = new EstadisticaMovimientoViewModel()
                {
                    AlmacenID = keyValuePair.Key,
                    DB = _context,
                    Importe = keyValuePair.Value,
                    Movimiento = TipoMovimiento.Salida
                };

                await _dashboard.GuardarEstadisticaDeMovimientoAsync(estadisticaMovimiento);
            }

            foreach (var s in _salidasDetalle)
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
                TempData["toast"] = "La salida ha sido aplicada, no podrá realizar cambios en la información.";
                await BitacoraAsync("Aplicar", salida);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "Error al aplicar el movimiento, verifique bitácora de errores.";
                await BitacoraAsync("Aplicar", salida, excepcion);
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

        
        //Detalle de movimientos
        public async Task<IActionResult> AddDetails(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada((Guid)id))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (salidaDetalle == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada(salidaDetalle.SalidaID))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = salidaDetalle.SalidaID });
            }

            if (salidaDetalle.AlmacenID == null)
            {
                TempData["toast"] = "El campo almacén es requerido.";
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(salidaDetalle);
            }

            if (salidaDetalle.ProductoID == null)
            {
                TempData["toast"] = "El campo producto es requerido.";
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(salidaDetalle);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)salidaDetalle.AlmacenID);
            var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)salidaDetalle.ProductoID);

            TempData["toast"] = "Falta información en algún campo.";
            if (ModelState.IsValid)
            {
                if (almacen == null)
                {
                    TempData["toast"] = "El campo almacén es requerido.";
                    ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                    return View(salidaDetalle);
                }

                if (producto == null)
                {
                    TempData["toast"] = "El campo producto es requerido.";
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

                if (salidaDetalle.Cantidad <= 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor a cero.");
                    return View(salidaDetalle);
                }

                if (existencia == null || 
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

                    TempData["toast"] = "Los datos del producto fueron almacenados correctamente.";
                    await BitacoraAsync("Alta", salidaDetalle, salidaDetalle.SalidaID);
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
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del producto no fueron almacenados.";
                    ModelState.AddModelError(string.Empty, "Error al guardar registro");
                    await BitacoraAsync("Alta", salidaDetalle, salidaDetalle.SalidaID, excepcion);
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var detalle = await _getHelper.GetSalidaDetalleByIdAsync((Guid)id);

            if (SalidaAplicada(detalle.SalidaID))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (salidaDetalle == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada(salidaDetalle.SalidaID))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
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
            var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)salidaDetalle.ProductoID);

            TempData["toast"] = "Información incompleta, verifique los campos.";
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
                    TempData["toast"] = "Los datos del producto fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", salidaDetalle, salidaDetalle.SalidaID);

                    return RedirectToAction(nameof(Details), new { id = salidaDetalle.SalidaID });

                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del producto no fueron actualizados.";
                    await BitacoraAsync("Actualizar", salidaDetalle, salidaDetalle.SalidaID, excepcion);
                }
            }

            salidaDetalle.Productos = producto;

            return View(salidaDetalle);
        }

        public async Task<IActionResult> DeleteDetails(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var detalle = await _getHelper.GetSalidaDetalleByIdAsync((Guid)id);

            if (detalle == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (SalidaAplicada(detalle.SalidaID))
            {
                TempData["toast"] = "Salida aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = detalle.SalidaID });
            }

            _context.SalidasDetalle.Remove(detalle);
            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del producto fueron eliminados correctamente.";
                await BitacoraAsync("Baja", detalle, detalle.SalidaID);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del producto no fueron eliminados.";
                await BitacoraAsync("Baja", detalle, detalle.SalidaID, excepcion);
            }
            
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

        private async Task BitacoraAsync(string accion, Salida salida, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                salida, salida.SalidaID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, SalidaDetalle salidaDetalle, Guid salidaId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                salidaDetalle, salidaId.ToString(), directorioBitacora, excepcion);
        }
    }
}
