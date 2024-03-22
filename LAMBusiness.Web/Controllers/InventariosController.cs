using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Movimiento;
using LAMBusiness.Web.Helpers;
using LAMBusiness.Web.Interfaces;
using LAMBusiness.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using LAMBusiness.Backend;

namespace LAMBusiness.Web.Controllers
{
    public class InventariosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private readonly IDashboard _dashboard;
        private readonly Productos _productos;
        private Guid moduloId = Guid.Parse("1D1048A7-44FE-44B5-ABC8-CF0A4DFC0AFF");

        public InventariosController(DataContext context,
            IGetHelper getHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration,
            IDashboard dashboard)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
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

            var inventarios = _context.Inventarios
                .OrderByDescending(e => e.FechaActualizacion);

            var filtro = new Filtro<List<Inventario>>()
            {
                Datos = await inventarios.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await inventarios.CountAsync(),
                Skip = 0
            };

            return View(filtro);

        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Inventario>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Inventario> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Inventarios
                                .Include(e => e.Usuarios)
                                .Where(p => p.Usuarios.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(e => e.Usuarios)
                                .Where(p => p.Usuarios.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Inventarios.Include(e => e.Usuarios);
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderByDescending(m => m.FechaActualizacion)
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
                            <Filtro<List<Inventario>>>(ViewData, filtro)
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

            var inventario = await _getHelper.GetInventarioByIdAsync((Guid)id);

            if (inventario == null)
            {
                TempData["toast"] = "Identificador del inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            var inventarioViewModel = await _converterHelper.ToInventarioViewModelAsync(inventario);

            inventarioViewModel.PermisoEscritura = permisosModulo.PermisoEscritura;

            return View(inventarioViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new Inventario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fecha,Observaciones")] Inventario inventario)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (InventarioAplicada(inventario.InventarioID))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = inventario.InventarioID });
            }

            TempData["toast"] = "Falta información en algún campo.";

            if (ModelState.IsValid)
            {
                inventario.Aplicado = false;
                inventario.FechaCreacion = DateTime.Now;
                inventario.FechaActualizacion = DateTime.Now;
                inventario.Observaciones = inventario.Observaciones == null ? "" : inventario.Observaciones.Trim().ToUpper();
                inventario.UsuarioID = token.UsuarioID;

                try
                {
                    _context.Add(inventario);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del inventario se almacenaron correctamente.";
                    await BitacoraAsync("Alta", inventario);
                    return RedirectToAction(nameof(Details), new { id = inventario.InventarioID });
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "Error al guardar inventario, verifique bitácora de errores.";
                    await BitacoraAsync("Alta", inventario, excepcion);
                }
            }

            return View(inventario);
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

            if (InventarioAplicada((Guid)id))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var inventario = await _getHelper.GetInventarioByIdAsync((Guid)id);

            if (inventario == null)
            {
                TempData["toast"] = "Identificador del inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            return View(inventario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InventarioID,Fecha,Observaciones")] Inventario inventario)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != inventario.InventarioID)
            {
                TempData["toast"] = "Identificador del inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (InventarioAplicada((Guid)id))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["toast"] = "Falta información en algún campo.";

            if (ModelState.IsValid)
            {
                var _inventario = await _getHelper.GetInventarioByIdAsync(inventario.InventarioID);

                _inventario.FechaActualizacion = DateTime.Now;
                _inventario.Fecha = inventario.Fecha;
                _inventario.Observaciones = inventario.Observaciones == null ? "" : inventario.Observaciones.Trim().ToUpper();
                _inventario.UsuarioID = token.UsuarioID;

                try
                {
                    _context.Update(_inventario);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del inventario se actualizaron correctamente.";
                    await BitacoraAsync("Actualizar", inventario);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", inventario, excepcion);
                    if (!InventarioExists(inventario.InventarioID))
                    {
                        TempData["toast"] = "Identificador del inventario inexistente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del inventario no fueron actualizados.";
                    }
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del inventario no fueron actualizados.";
                    await BitacoraAsync("Actualizar", inventario, excepcion);
                }
                return RedirectToAction(nameof(Details), new { id = inventario.InventarioID });
            }

            return View(inventario);
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

            var inventario = await _context.Inventarios
                .Include(e => e.Usuarios)
                .FirstOrDefaultAsync(m => m.InventarioID == id);

            if (inventario == null)
            {
                TempData["toast"] = "Identificador incorrecto, inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (InventarioAplicada((Guid)id))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var inventarioDetalle = await _context.InventariosDetalle
                .Where(s => s.InventarioID == id).ToListAsync();

            if (inventarioDetalle.Any())
            {
                foreach (var e in inventarioDetalle)
                {
                    _context.InventariosDetalle.Remove(e);
                }
            }

            _context.Inventarios.Remove(inventario);

            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del inventario fueron eliminados correctamente.";
                await BitacoraAsync("Baja", inventario);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del inventario no fueron eliminados.";
                await BitacoraAsync("Baja", inventario);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Apply(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            //cambiar por permiso para aplicar que se debe agregar como acción.
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return RedirectToAction(nameof(Index));

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (InventarioAplicada((Guid)id))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var inventario = await _getHelper.GetInventarioByIdAsync((Guid)id);

            if (inventario == null)
            {
                TempData["toast"] = "Identificador del inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            inventario.Aplicado = true;
            _context.Update(inventario);

            var detalle = await _getHelper.GetInventarioDetalleByInventarioIdAsync(inventario.InventarioID);
            if (detalle == null)
            {
                TempData["toast"] = "Por favor, ingrese al menos un movimiento.";
                return RedirectToAction(nameof(Details), new { id });
            }

            foreach (var item in detalle)
            {
                Guid _almacenId = (Guid)item.AlmacenID;
                Guid _productoId = (Guid)item.ProductoID;
                decimal _cantidadInventariada = (decimal)item.CantidadInventariada;

                if (item.Productos.Unidades.Pieza)
                    _cantidadInventariada = (int)_cantidadInventariada;

                if (item.Productos.Unidades.Paquete)
                {
                    if (item.Productos.Paquete == null)
                    {
                        TempData["toast"] = $"El producto ({item.Productos.Codigo} - {item.Productos.Nombre}) está clasificado como paquete, pero no se encontró el código de la pieza asociada.";
                        return RedirectToAction(nameof(Details), new { id });
                    }
                    _productoId = item.Productos.Paquete.PiezaProductoID;
                    _cantidadInventariada = item.Productos.Paquete.CantidadProductoxPaquete * _cantidadInventariada;
                    var producto = await _context.Productos.FindAsync(_productoId);
                    if (producto != null)
                        item.PrecioCosto = (decimal)producto.PrecioCosto;
                }
                else
                {
                    item.PrecioCosto = (decimal)item.Productos.PrecioCosto;
                }

                var existencia = item.Productos.Existencias.FirstOrDefault(e => e.ProductoID == _productoId &&
                                        e.AlmacenID == _almacenId);

                if (existencia == null)
                {
                    item.CantidadEnSistema = 0;
                    _context.Add(new Existencia()
                    {
                        AlmacenID = _almacenId,
                        ExistenciaEnAlmacen = _cantidadInventariada,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = _productoId,
                    });
                }
                else
                {
                    item.CantidadEnSistema = existencia.ExistenciaEnAlmacen;
                    existencia.ExistenciaEnAlmacen = _cantidadInventariada;
                }

                var diferencia = item.CantidadInventariada - item.CantidadEnSistema;
                if (diferencia < 0)
                    item.CantidadFaltante = diferencia;
                else
                    diferencia = 0;

                item.Importe = diferencia * item.PrecioCosto;

                _context.Update(item);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "El inventario ha sido aplicado, no podrá realizar cambios en la información.";
                await BitacoraAsync("Aplicar", inventario);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "Error al aplicar el movimiento, verifique bitácora de errores.";
                await BitacoraAsync("Aplicar", inventario, excepcion);
            }

            return RedirectToAction(nameof(Details), new { id });

        }

        private bool InventarioExists(Guid id)
        {
            return _context.Inventarios.Any(e => e.InventarioID == id);
        }

        private bool InventarioAplicada(Guid id)
        {
            return _context.Inventarios.Any(e => e.InventarioID == id && e.Aplicado == true);
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

            if (InventarioAplicada((Guid)id))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(new InventarioDetalleViewModel()
            {
                InventarioID = (Guid)id,
                Cantidad = 0,
                CantidadEnPiezas = 0,
                EsPaquete = false,
                AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetails(InventarioDetalleViewModel inventarioDetalleVM)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return RedirectToAction(nameof(Index));

            if (inventarioDetalleVM == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }
            
            inventarioDetalleVM.AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync();

            if (InventarioAplicada(inventarioDetalleVM.InventarioID))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = inventarioDetalleVM.InventarioID });
            }

            if (inventarioDetalleVM.AlmacenID == null)
            {
                TempData["toast"] = "El campo almacén es requerido.";
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(inventarioDetalleVM);
            }

            if (inventarioDetalleVM.ProductoID == null)
            {
                TempData["toast"] = "El campo producto es requerido.";
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(inventarioDetalleVM);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)inventarioDetalleVM.AlmacenID);
            var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)inventarioDetalleVM.ProductoID);
            TempData["toast"] = "Falta información en algún campo.";

            ValidarDatosDelProducto(inventarioDetalleVM);

            if (almacen == null)
            {
                TempData["toast"] = "El campo almacén es requerido.";
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(inventarioDetalleVM);
            }
            inventarioDetalleVM.Almacenes = almacen;
            if (producto == null)
            {
                TempData["toast"] = "El campo producto es requerido.";
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(inventarioDetalleVM);
            }

            if (producto.Unidades.Pieza)
                inventarioDetalleVM.Cantidad = (int)inventarioDetalleVM.Cantidad;

            if (producto.Unidades.Paquete)
            {
                var paquete = await _context.Paquetes.FirstOrDefaultAsync(p => p.ProductoID == producto.ProductoID);
                if (paquete == null)
                {
                    TempData["toast"] = "El producto es paquete y no existe registro de la pieza asociada.";
                    ModelState.AddModelError("ProductoID", "El producto es paquete y no existe registro de la pieza asociada.");
                    inventarioDetalleVM.Cantidad = 0;
                    return View(inventarioDetalleVM);
                }

                inventarioDetalleVM.ProductoID = paquete.PiezaProductoID;
                inventarioDetalleVM.Cantidad = (inventarioDetalleVM.Cantidad * paquete.CantidadProductoxPaquete) + inventarioDetalleVM.CantidadEnPiezas;
                inventarioDetalleVM.CantidadEnPiezas = 0;
                inventarioDetalleVM.EsPaquete = false;
            }
            else
            {
                inventarioDetalleVM.CantidadEnPiezas = 0;
            }

            var productoCapturado = await _context.InventariosDetalle.AnyAsync(i => i.InventarioID == inventarioDetalleVM.InventarioID && i.ProductoID == inventarioDetalleVM.ProductoID && i.AlmacenID == inventarioDetalleVM.AlmacenID);
            if (productoCapturado)
            {
                TempData["toast"] = "El producto ya fue capturado previamente.";
                ModelState.AddModelError("ProductoID", "El producto ya fue capturado previamente.");
                inventarioDetalleVM.Cantidad = 0;
                return View(inventarioDetalleVM);
            }

            var inventarioDetalle = new InventarioDetalle()
            {
                AlmacenID = inventarioDetalleVM.AlmacenID,
                CantidadFaltante = 0,
                CantidadEnSistema = 0,
                CantidadInventariada = inventarioDetalleVM.Cantidad,
                Importe = 0,
                InventarioDetalleID = Guid.NewGuid(),
                InventarioID = inventarioDetalleVM.InventarioID,
                PrecioCosto = 0,
                ProductoID = inventarioDetalleVM.ProductoID,
            };

            try
            {
                _context.Add(inventarioDetalle);

                await _context.SaveChangesAsync();

                TempData["toast"] = "Los datos del producto fueron almacenados correctamente.";
                await BitacoraAsync("Alta", inventarioDetalle, inventarioDetalle.InventarioID);

                return View(new InventarioDetalleViewModel()
                {
                    AlmacenID = inventarioDetalleVM.AlmacenID,
                    Almacenes = almacen,
                    InventarioID = inventarioDetalleVM.InventarioID,
                    Cantidad = 0,
                    CantidadEnPiezas = 0,
                    AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync()
                });

            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del producto no fueron almacenados.";
                ModelState.AddModelError(string.Empty, "Error al guardar registro");
                await BitacoraAsync("Alta", inventarioDetalle, inventarioDetalle.InventarioID, excepcion);
            }

            return View(inventarioDetalleVM);
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

            var detalle = await _getHelper.GetInventarioDetalleByIdAsync((Guid)id);

            if (InventarioAplicada(detalle.InventarioID))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = detalle.InventarioID });
            }

            var detalleVM = new InventarioDetalleViewModel()
            {
                AlmacenID = detalle.AlmacenID,
                Almacenes = detalle.Almacenes,
                Cantidad = detalle.CantidadInventariada,
                CantidadEnPiezas = 0,
                EsPaquete = false,
                InventarioDetalleID = detalle.InventarioDetalleID,
                InventarioID = detalle.InventarioID,
                ProductoID = detalle.ProductoID,
                Productos = detalle.Productos,
                AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync()
            };

            return View(detalleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(InventarioDetalleViewModel inventarioDetalleVM)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (inventarioDetalleVM == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            inventarioDetalleVM.AlmacenesDDL = await _combosHelper.GetComboAlmacenesAsync();

            if (InventarioAplicada(inventarioDetalleVM.InventarioID))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = inventarioDetalleVM.InventarioID });
            }

            if (inventarioDetalleVM.AlmacenID == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(inventarioDetalleVM);
            }

            if (inventarioDetalleVM.ProductoID == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(inventarioDetalleVM);
            }

            var almacen = await _getHelper.GetAlmacenByIdAsync((Guid)inventarioDetalleVM.AlmacenID);
            var producto = await _productos.ObtenerRegistroPorIdAsync((Guid)inventarioDetalleVM.ProductoID);

            TempData["toast"] = "Información incompleta, verifique los campos.";

            ValidarDatosDelProducto(inventarioDetalleVM);

            if (almacen == null)
            {
                ModelState.AddModelError("AlmacenID", "El campo almacén es requerido.");
                return View(inventarioDetalleVM);
            }
            inventarioDetalleVM.Almacenes = almacen;
            if (producto == null)
            {
                ModelState.AddModelError("ProductoID", "El campo producto es requerido.");
                return View(inventarioDetalleVM);
            }

            if (producto.Unidades.Pieza)
                inventarioDetalleVM.Cantidad = (int)inventarioDetalleVM.Cantidad;

            if (producto.Unidades.Paquete)
            {
                var paquete = await _context.Paquetes.FirstOrDefaultAsync(p => p.ProductoID == producto.ProductoID);
                if (paquete == null)
                {
                    TempData["toast"] = "El producto es paquete y no existe registro de la pieza asociada.";
                    ModelState.AddModelError("ProductoID", "El producto es paquete y no existe registro de la pieza asociada.");
                    return View(inventarioDetalleVM);
                }

                inventarioDetalleVM.ProductoID = paquete.PiezaProductoID;
                inventarioDetalleVM.Cantidad = (inventarioDetalleVM.Cantidad * paquete.CantidadProductoxPaquete) + inventarioDetalleVM.CantidadEnPiezas;
                inventarioDetalleVM.CantidadEnPiezas = 0;
                inventarioDetalleVM.EsPaquete = false;
            }
            else
            {
                inventarioDetalleVM.CantidadEnPiezas = 0;
            }

            var productoCapturado = await _context.InventariosDetalle.AnyAsync(i => i.InventarioDetalleID != inventarioDetalleVM.InventarioDetalleID && i.InventarioID == inventarioDetalleVM.InventarioID && i.ProductoID == inventarioDetalleVM.ProductoID && i.AlmacenID == inventarioDetalleVM.AlmacenID);
            if (productoCapturado)
            {
                TempData["toast"] = "El producto ya fue capturado previamente.";
                ModelState.AddModelError("ProductoID", "El producto ya fue capturado previamente.");
                inventarioDetalleVM.Cantidad = 0;
                return View(inventarioDetalleVM);
            }

            var inventarioDetalle = await _context.InventariosDetalle.FindAsync(inventarioDetalleVM.InventarioDetalleID);
            if (inventarioDetalle == null)
            {
                TempData["toast"] = "El identificador del registro es incorrecto.";
                ModelState.AddModelError("ProductoID", "El identificador del registro es incorrecto");
                inventarioDetalleVM.Cantidad = 0;
                return View(inventarioDetalleVM);
            }

            inventarioDetalle.AlmacenID = inventarioDetalleVM.AlmacenID;
            inventarioDetalle.CantidadInventariada = inventarioDetalleVM.Cantidad;
            inventarioDetalle.ProductoID = inventarioDetalleVM.ProductoID;

            try
            {
                _context.Update(inventarioDetalle);

                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del producto fueron actualizados correctamente.";
                await BitacoraAsync("Actualizar", inventarioDetalle, inventarioDetalle.InventarioID);

                return RedirectToAction(nameof(Details), new { id = inventarioDetalle.InventarioID });
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del producto no fueron actualizados.";
                ModelState.AddModelError(string.Empty, "Error al actualizar el registro");
                await BitacoraAsync("Actualizar", inventarioDetalle, inventarioDetalle.InventarioID, excepcion);
            }

            return View(inventarioDetalleVM);
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

            var detalle = await _getHelper.GetInventarioDetalleByIdAsync((Guid)id);

            if (detalle == null)
            {
                TempData["toast"] = "Identificador del inventario inexistente.";
                return RedirectToAction(nameof(Index));
            }

            if (InventarioAplicada(detalle.InventarioID))
            {
                TempData["toast"] = "Inventario aplicada no se permiten cambios.";
                return RedirectToAction(nameof(Details), new { id = detalle.InventarioID });
            }

            _context.InventariosDetalle.Remove(detalle);
            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del producto fueron eliminados correctamente.";
                await BitacoraAsync("Baja", detalle, detalle.InventarioID);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del producto no fueron eliminados.";
                await BitacoraAsync("Baja", detalle, detalle.InventarioID, excepcion);
            }

            return RedirectToAction(nameof(Details), new { id = detalle.InventarioID });
        }

        //Bitácora
        private async Task BitacoraAsync(string accion, Inventario inventario, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                inventario, inventario.InventarioID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, InventarioDetalle inventarioDetalle, Guid inventarioId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                inventarioDetalle, inventarioId.ToString(), directorioBitacora, excepcion);
        }

        private void ValidarDatosDelProducto(InventarioDetalleViewModel inventarioDetalle)
        {
            if (inventarioDetalle.Cantidad == null || inventarioDetalle.Cantidad < 0)
            {
                TempData["toast"] = "Cantidad de productos incorrecto.";
                ModelState.AddModelError("Cantidad", "Cantidad de productos incorrecta.");
            }
            if (inventarioDetalle.CantidadEnPiezas == null || inventarioDetalle.Cantidad < 0)
            {
                TempData["toast"] = "Cantidad de piezas incorrecto.";
                ModelState.AddModelError("Cantidad", "Cantidad de piezas incorrecta.");
            }
        }
    }
}
