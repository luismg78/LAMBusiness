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
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;
    using Web.Models.ViewModels;

    public class EntradasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("B019EBF0-5A25-4CC3-BD72-34FDA134E5C1");

        public EntradasController(DataContext context, 
            IGetHelper getHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration)
        {
            _context = context;
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

            var dataContext = _context.Entradas
                .Include(e => e.Proveedores)
                .OrderBy(e => e.Folio)
                .ThenBy(e => e.Proveedores.Nombre);

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

            IQueryable<Entrada> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Entradas
                                .Include(e => e.Proveedores)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.Proveedores.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(e => e.Proveedores)
                                .Where(p => p.Folio.Contains(w) ||
                                            p.Proveedores.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Entradas.Include(e => e.Proveedores);
            }

            var entradas = await query.OrderByDescending(m => m.FechaActualizacion)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Entrada>>(ViewData, entradas)
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

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);
            
            if (entrada == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            var entradaViewModel = await _converterHelper.ToEntradaViewModelAsync(entrada);

            return View(entradaViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (EntradaAplicada(entrada.EntradaID))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
            }

            TempData["toast"] = "Información incompleta, verifique los campos.";

            if (ModelState.IsValid)
            {
                entrada.EntradaID = Guid.NewGuid();
                entrada.Aplicado = false;
                entrada.FechaActualizacion = DateTime.Now;
                entrada.FechaCreacion = DateTime.Now;
                entrada.Folio = entrada.Folio.Trim().ToUpper();
                entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();
                entrada.UsuarioID = token.UsuarioID;

                try
                {
                    _context.Add(entrada);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la entrada se almacenaron correctamente.";
                    return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
                }
                catch (Exception)
                {
                    TempData["toast"] = "Error al guardar entrada, verifique bitácora de errores.";
                }
            }

            return View(entrada);
        }

        public async Task<IActionResult> Edit(Guid? id)
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

            if (EntradaAplicada((Guid)id))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(e => e.EntradaID == id);

            if (entrada == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EntradaID,ProveedorID,Fecha,Folio,Observaciones")] Entrada entrada)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != entrada.EntradaID)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (EntradaAplicada((Guid)id))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["toast"] = "Información incompleta, verifique los campos.";

            if (ModelState.IsValid)
            {
                var _entrada = await _context.Entradas.FindAsync(entrada.EntradaID);

                _entrada.FechaActualizacion = DateTime.Now;
                _entrada.ProveedorID = entrada.ProveedorID;
                _entrada.Folio = entrada.Folio.Trim().ToUpper();
                _entrada.Fecha = entrada.Fecha;
                _entrada.Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper();
                _entrada.UsuarioID = token.UsuarioID;

                try
                {
                    _context.Update(_entrada);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la entrada se actualizaron correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.EntradaID))
                    {
                        TempData["toast"] = "Identificador de la entrada inexistente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["toast"] = "Error al guardar entrada, verifique bitácora de errores.";
                    }
                }
                return RedirectToAction(nameof(Details), new { id = entrada.EntradaID });
            }

            return View(entrada);
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

            if (EntradaAplicada((Guid)id))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _context.Entradas
                .Include(e => e.Proveedores)
                .FirstOrDefaultAsync(m => m.EntradaID == id);
            
            if (entrada == null)
            {
                return NotFound();
            }

            var entradaDetalle = await _context.EntradasDetalle
                .Where(s => s.EntradaID == id).ToListAsync();

            if (entradaDetalle.Any())
            {
                foreach (var e in entradaDetalle)
                {
                    _context.EntradasDetalle.Remove(e);
                }
            }

            _context.Entradas.Remove(entrada);

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

            if (EntradaAplicada((Guid)id))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var entrada = await _getHelper.GetEntradaByIdAsync((Guid)id);

            if (entrada == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            entrada.Aplicado = true;
            _context.Update(entrada);

            var detalle = await _getHelper.GetEntradaDetalleByEntradaIdAsync(entrada.EntradaID);
            if(detalle == null)
            {
                TempData["toast"] = "Por favor, ingrese al menos un movimiento.";
                return RedirectToAction(nameof(Details), new { id });
            }
            
            var existencias = new List<ExistenciaViewModel>();

            foreach(var item in detalle)
            {
                Guid _almacenId = (Guid)item.AlmacenID;
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

                var existencia = existencias
                    .FirstOrDefault(e => e.ProductoID == _productoId && e.AlmacenID == _almacenId);

                if(existencia == null)
                {
                    existencias.Add(new ExistenciaViewModel() {
                        AlmacenID = _almacenId,
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

            foreach (var item in existencias)
            {
                Guid _almacenId = (Guid)item.AlmacenID;

                var existencia = await _getHelper
                    .GetExistenciaByProductoIdAndAlmacenIdAsync(item.ProductoID, _almacenId);
                if (existencia == null)
                {
                    _context.Existencias.Add(new Existencia()
                    {
                        AlmacenID = _almacenId,
                        ExistenciaEnAlmacen = item.ExistenciaEnAlmacen,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = item.ProductoID
                    });
                    
                }
                else
                {
                    existencia.ExistenciaEnAlmacen += item.ExistenciaEnAlmacen;
                    _context.Update(existencia);
                }
            }

            var productos = existencias.GroupBy(e => e.ProductoID)
                .Select(g => new {
                    produdtoID = g.Key,
                    existencia = g.Sum(p => p.ExistenciaEnAlmacen),
                    precioCosto = (g.Sum(p => p.ExistenciaEnAlmacen * p.PrecioCosto) / g.Sum(p => p.ExistenciaEnAlmacen)) })
                .ToList();

            foreach(var p in productos)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.ProductoID == p.ProductoID);

                var existenciaActual = await _context.Existencias
                    .Where(p => p.ProductoID == p.ProductoID)
                    .SumAsync(e => e.ExistenciaEnAlmacen);

                producto.PrecioCosto = ((p.existencia * p.precioCosto) +
                                        (producto.PrecioCosto * existenciaActual)) /
                                       (p.existencia + existenciaActual);

                _context.Update(producto);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "La entrada ha sido aplicada, no podrá realizar cambios en la información.";
            }
            catch (Exception)
            {
                TempData["toast"] = "Error al aplicar el movimieno, verifique bitácora de errores.";
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

        public async Task<IActionResult> GetAlmacen(string almacenNombre)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (code == null || code == "")
            {
                return null;
            }

            var producto = await _getHelper.GetProductByCodeAsync(code.Trim().ToUpper());
            if (producto != null)
            {
                producto.PrecioCosto = Convert.ToDecimal(producto.PrecioCosto?.ToString("0.00"));
                producto.PrecioVenta = Convert.ToDecimal(producto.PrecioVenta?.ToString("0.00"));

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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

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

        public async Task<IActionResult> AddDetails(Guid? id)
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

            if (EntradaAplicada((Guid)id))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
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
        public async Task<IActionResult> AddDetails(EntradaDetalle entradaDetalle)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (entradaDetalle == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (EntradaAplicada(entradaDetalle.EntradaID))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
            }

            if (entradaDetalle.AlmacenID == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(entradaDetalle);
            }

            if (entradaDetalle.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(entradaDetalle);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)entradaDetalle.AlmacenID);
            var producto = await _getHelper.GetProductByIdAsync((Guid)entradaDetalle.ProductoID);

            TempData["toast"] = "Información incompleta, verifique los campos.";

            ValidarDatosDelProducto(entradaDetalle);

            if (ModelState.IsValid)
            {
                if (almacen == null)
                {
                    ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                    return View(entradaDetalle);
                }

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

                    TempData["toast"] = "Registro almacenado.";
                    ModelState.AddModelError(string.Empty, "Registro almacenado.");

                    return View(new EntradaDetalle() { 
                        AlmacenID = entradaDetalle.AlmacenID,
                        Almacenes = almacen,
                        EntradaID = entradaDetalle.EntradaID,
                        Cantidad = 0,
                        PrecioCosto = 0,
                        PrecioVenta = 0
                    });

                }
                catch (Exception)
                {
                    TempData["toast"] = "Error al guardar registro, verifique bitácora de errores.";
                    ModelState.AddModelError(string.Empty, "Error al guardar registro");
                }
            }

            entradaDetalle.Almacenes = almacen;
            entradaDetalle.Productos = producto;
            return View(entradaDetalle);
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

            var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);
            
            if (EntradaAplicada(detalle.EntradaID))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
            }

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(EntradaDetalle entradaDetalle)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (entradaDetalle == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (EntradaAplicada(entradaDetalle.EntradaID))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
            }

            if (entradaDetalle.AlmacenID == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(entradaDetalle);
            }

            if (entradaDetalle.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(entradaDetalle);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)entradaDetalle.AlmacenID);
            var producto = await _getHelper.GetProductByIdAsync((Guid)entradaDetalle.ProductoID);

            TempData["toast"] = "Información incompleta, verifique los campos.";

            ValidarDatosDelProducto(entradaDetalle);

            if (ModelState.IsValid)
            {
                if (almacen == null)
                {
                    ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                    return View(entradaDetalle);
                }

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
                    TempData["toast"] = "Registro almacenado.";
                    ModelState.AddModelError(string.Empty, "Registro almacenado.");

                    return RedirectToAction(nameof(Details), new { id = entradaDetalle.EntradaID });
                }
                catch (Exception)
                {
                    TempData["toast"] = "Error al guardar registro, verifique bitácora de errores.";
                    ModelState.AddModelError(string.Empty, "Error al guardar registro");
                }
            }

            entradaDetalle.Productos = producto;

            return View(entradaDetalle);
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

            var detalle = await _getHelper.GetEntradaDetalleByIdAsync((Guid)id);

            if (detalle == null)
            {
                TempData["toast"] = "Identificador de la entrada inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (EntradaAplicada(detalle.EntradaID))
            {
                TempData["toast"] = "Entrada aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
            }

            _context.EntradasDetalle.Remove(detalle);
            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "El producto ha sido eliminado de la lista.";
            }
            catch (Exception)
            {
                TempData["toast"] = "Error al intentar eliminar el producto de la lista.";
            }

            return RedirectToAction(nameof(Details), new { id = detalle.EntradaID });
        }
    
        private void ValidarDatosDelProducto(EntradaDetalle entradaDetalle)
        {
            if(entradaDetalle.PrecioVenta == null || entradaDetalle.PrecioVenta <= 0)
            {
                TempData["toast"] = "Precio de venta incorrecto.";
                ModelState.AddModelError("PrecioVenta", "Precio de venta incorrecto.");
            }

            if (entradaDetalle.Cantidad == null || entradaDetalle.Cantidad <= 0)
            {
                TempData["toast"] = "Cantidad de productos incorrecto.";
                ModelState.AddModelError("Cantidad", "Precio de venta incorrecto.");
            }

            if (entradaDetalle.PrecioCosto == null || entradaDetalle.PrecioCosto < 0)
            {
                TempData["toast"] = "Precio de costo incorrecto.";
                ModelState.AddModelError("PrecioCosto", "Precio de venta incorrecto.");
            }
        }
    }
}
