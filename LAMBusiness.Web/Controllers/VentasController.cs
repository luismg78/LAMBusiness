namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Interfaces;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Movimiento;
    using NuGet.Packaging.Core;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using System.Collections.Immutable;

    public class VentasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private readonly IDashboard _dashboard;
        private Guid moduloId = Guid.Parse("a0ca4d51-b518-4a65-b1e3-f0a03b1caff8");

        public VentasController(DataContext context, IGetHelper getHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration,
            IDashboard dashboard)
        {
            _context = context;
            _getHelper = getHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
            _dashboard = dashboard;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var hayVentasPorCerrar = await _context.Ventas.AnyAsync(v => v.VentaCierreID == null || v.VentaCierreID == Guid.Empty);

            int totalDeVentasNoAplicadas = 0;
            VentaNoAplicada ventaNoAplicada = await InitializeSalesAsync();
            
            if (ventaNoAplicada == null)
            {
                TempData["toast"] = "La venta no puede ser inicializada.";
                return RedirectToAction("Index", "Movimiento");
            }
            else
            {
                totalDeVentasNoAplicadas = await (from v in _context.VentasNoAplicadas
                                                  join d in _context.VentasNoAplicadasDetalle on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                                  where v.UsuarioID == token.UsuarioID
                                                  select v).CountAsync();
            }

            VentasNoAplicadasViewModel venta = new()
            {
                Fecha = ventaNoAplicada.Fecha,
                HayVentasPorCerrar = hayVentasPorCerrar,
                ImporteTotal = 0,
                UsuarioID = token.UsuarioID,
                VentaNoAplicadaID = ventaNoAplicada.VentaNoAplicadaID,
                VentasNoAplicadasDetalle = null,
                TotalDeRegistrosPendientes = totalDeVentasNoAplicadas,
            };

            ViewBag.Id = token.UsuarioID;
            return View(venta);
        }

        private async Task<VentaNoAplicada> InitializeSalesAsync()
        {
            VentaNoAplicada ventaNoAplicada = null;
            List<VentaNoAplicada> ventasNoAplicadas = await _context.VentasNoAplicadas
                .Where(v => v.UsuarioID == token.UsuarioID)
                .ToListAsync();

            if (ventasNoAplicadas != null && ventasNoAplicadas.Any())
                ventaNoAplicada = ventasNoAplicadas.OrderByDescending(v => v.Fecha).FirstOrDefault();

            if (ventaNoAplicada == null)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = token.UsuarioID,
                    VentaNoAplicadaID = Guid.NewGuid()
                };

                _context.VentasNoAplicadas.Add(ventaNoAplicada);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    TempData["toast"] = "La venta no puede ser inicializada.";
                }
            }
            
            return ventaNoAplicada;
        }

        public async Task<IActionResult> CloseSales()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return Json(new { Reiniciar = true, Error = true });

            Guid ventaDeCierreId = Guid.NewGuid();

            var importesDelSistema = await (from v in _context.Ventas
                                            join vi in _context.VentasImportes on v.VentaID equals vi.VentaID
                                            join fp in _context.FormasPago on vi.FormaPagoID equals fp.FormaPagoID
                                            where v.UsuarioID == token.UsuarioID && (v.VentaCierreID == null || v.VentaCierreID == Guid.Empty)
                                            group new { fp, vi } by new { fp.FormaPagoID, fp.Nombre } into g
                                            select new ImporteDelSistemaDetalle()
                                            {
                                                FormaDePagoId = g.Key.FormaPagoID,
                                                FormaDePago = g.Key.Nombre,
                                                Importe = g.Sum(a => a.vi.Importe)
                                            }).ToListAsync();

            var ventas = await (from v in _context.Ventas
                                join vi in _context.VentasImportes on v.VentaID equals vi.VentaID
                                join fp in _context.FormasPago on vi.FormaPagoID equals fp.FormaPagoID
                                where v.UsuarioID == token.UsuarioID && (v.VentaCierreID == null || v.VentaCierreID == Guid.Empty)
                                select v).ToListAsync();

            if (ventas == null || !ventas.Any())
                return Json(new { Error = true, Estatus = "Proceso no realizado, no hay registro de ventas con ese identificador de usuario." });
            
            foreach (var venta in ventas)
            {
                venta.VentaCierreID = ventaDeCierreId;
                _context.Update(venta);
            }

            var importeDelUsuario = await _context.RetirosCaja
                .Where(r => r.UsuarioID == token.UsuarioID && (r.VentaCierreID == null || r.VentaCierreID == Guid.Empty)).ToListAsync();

            if (importeDelUsuario == null || !importeDelUsuario.Any())
                return Json(new { Error = true, Estatus = "Proceso no realizado, ingrese al menos un retiro de caja." });

            foreach (var retiro in importeDelUsuario)
            {
                retiro.VentaCierreID = ventaDeCierreId;
                _context.Update(retiro);
            }

            _context.VentasCierre.Add(new VentaCierre()
            {
                Fecha = DateTime.Now,
                ImporteSistema = importesDelSistema.Sum(i => i.Importe),
                ImporteUsuario = importeDelUsuario.Sum(r => r.Importe),
                UsuarioCajaID = token.UsuarioID,
                UsuarioID = token.UsuarioID,
                VentaCierreID = ventaDeCierreId
            });

            if (importesDelSistema != null && importesDelSistema.Any())
            {
                foreach (var item in importesDelSistema)
                {
                    _context.VentasCierreDetalle.Add(new VentaCierreDetalle()
                    {
                        FormaPagoID = item.FormaDePagoId,
                        Importe = item.Importe,
                        VentaCierreDetalleID = Guid.NewGuid(),
                        VentaCierreID = ventaDeCierreId
                    });
                }
            }

            decimal totalSistema = importesDelSistema.Sum(i => i.Importe);
            decimal totalUsuario = importeDelUsuario.Sum(r => r.Importe);
            var corteDeCaja = new CorteDeCajaViewModel()
            {
                ImporteDelSistema = totalSistema.ToString("$###,###,##0.00"),
                ImporteDelUsuario = totalUsuario.ToString("$###,###,##0.00"),
                ImporteDelSistemaDetalle = importesDelSistema
            };

            try
            {
                await _context.SaveChangesAsync();
                return PartialView(corteDeCaja);
            }
            catch (Exception)
            {
                return Json(new { Error = true, Estatus = "Error. Proceso no realizado." });
            }
        }

        public async Task<IActionResult> GetItBackSaleById(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return RedirectToAction(nameof(Index));

            VentaNoAplicada ventaNoAplicada = await _context.VentasNoAplicadas
                    .OrderByDescending(v => v.Fecha)
                    .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                TempData["toast"] = "El identificador de la venta es incorrecta.";
                return RedirectToAction("Index", "Movimiento");
            }

            decimal total = 0;
            var ventaDetalle = await _context.VentasNoAplicadasDetalle
                                    .Include(v => v.Productos)
                                    .Where(v => v.VentaNoAplicadaID == ventaNoAplicada.VentaNoAplicadaID)
                                    .ToListAsync();
            if (ventaDetalle != null)
            {
                foreach (var item in ventaDetalle)
                {
                    total += item.Cantidad * item.PrecioVenta;
                }
            }

            VentasNoAplicadasViewModel venta = new VentasNoAplicadasViewModel()
            {
                Fecha = ventaNoAplicada.Fecha,
                ImporteTotal = total,
                UsuarioID = token.UsuarioID,
                VentaNoAplicadaID = ventaNoAplicada.VentaNoAplicadaID,
                VentasNoAplicadasDetalle = ventaDetalle
            };

            ViewBag.Id = token.UsuarioID;

            try
            {
                await _context.SaveChangesAsync();
                return View(nameof(Index), venta);
            }
            catch (Exception)
            {
                TempData["toast"] = "La venta no puede ser inicializada.";
                return RedirectToAction("Index", "Movimiento");
            }
        }

        public async Task<IActionResult> GetItBackSale()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            var ventasNoAplicadas = (from v in _context.VentasNoAplicadas
                                     join d in _context.VentasNoAplicadasDetalle
                                     on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                     orderby v.Fecha descending
                                     where v.UsuarioID == token.UsuarioID
                                     select v).Distinct();

            int registros = await ventasNoAplicadas.CountAsync();
            if (registros == 0)
                return Json(new { Estatus = "No existe registro de ventas pendientes por aplicar.", Error = true });

            Filtro<List<VentaNoAplicada>> filtro = new Filtro<List<VentaNoAplicada>>()
            {
                Datos = await ventasNoAplicadas.ToListAsync(),
                Registros = registros
            };

            return PartialView(filtro);
        }

        public async Task<IActionResult> GetProductByCode(Guid? id, string codigo, decimal cantidad)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            var resultado = await _getHelper.GetProductByCodeForSale(id, token.UsuarioID, codigo, cantidad);

            if (resultado.Error)
            {
                switch (resultado.Mensaje.Trim().ToLower())
                {
                    case "buscarproducto":
                        return Json(new { BuscarProducto = true, Error = true, Reiniciar = false });
                    case "reiniciar":
                        TempData["toast"] = "Identificador de la venta incorrecto.";
                        return Json(new { Reiniciar = true, Error = true });
                    default:
                        return Json(new { Estatus = resultado.Mensaje, Error = true, Reiniciar = true });
                }
            }

            return PartialView(resultado.Contenido);
        }

        public async Task<IActionResult> SetCancelSale(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            if (id == null || id == Guid.Empty)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }
            var ventaNoAplicada = await _context.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }

            var ventasNoAplicadasDetalle = await _context.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
                return Json(new { Estatus = "No hay registros para cancelar.", Error = true });

            //ventaNoAplicada.Fecha = DateTime.Now;
            //_context.Update(ventaNoAplicada);

            Guid ventaCanceladaId = Guid.NewGuid();

            VentaCancelada ventaCancelada = new()
            {
                Fecha = DateTime.Now,
                UsuarioID = token.UsuarioID,
                VentaCanceladaID = ventaCanceladaId,
                VentaCompleta = true
            };

            _context.VentasCanceladas.Add(ventaCancelada);

            List<VentaCanceladaDetalle> ventasCanceladasDetalle = new List<VentaCanceladaDetalle>();
            foreach (var item in ventasNoAplicadasDetalle)
            {
                VentaCanceladaDetalle ventaCanceladaDetalle = new()
                {
                    Cantidad = item.Cantidad,
                    PrecioVenta = item.PrecioVenta,
                    ProductoID = item.ProductoID,
                    VentaCanceladaID = ventaCanceladaId,
                    VentaCanceladaDetalleID = Guid.NewGuid()
                };

                ventasCanceladasDetalle.Add(ventaCanceladaDetalle);
                _context.VentasCanceladasDetalle.Add(ventaCanceladaDetalle);

                _context.Remove(item);
            }

            _context.Remove(ventaNoAplicada);

            VentaCanceladaViewModel ventaCanceladaViewModel = new VentaCanceladaViewModel()
            {
                VentasCanceladas = ventaCancelada,
                VentasCanceladasDetalle = ventasCanceladasDetalle
            };

            try
            {
                await _context.SaveChangesAsync();
                await BitacoraAsync("SetCancelSale", ventaCanceladaViewModel, token.UsuarioID);

                return Json(new { Error = false });
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                await BitacoraAsync("SetCancelSale", ventaCanceladaViewModel, token.UsuarioID, excepcion);
                return Json(new { Estatus = "Cancelación no realizada.", Error = true });
            }
        }

        public async Task<IActionResult> SetSale(Guid? id, decimal importe)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            if (id == null || id == Guid.Empty)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }
            var ventaNoAplicada = await _context.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }

            var ventasNoAplicadasDetalle = await _context.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
                return Json(new { Estatus = "Ingrese al menos un producto a la lista.", Error = true });

            var ventasNoAplicadasDetalleAgrupada = ventasNoAplicadasDetalle
                .GroupBy(v => new { v.VentaNoAplicadaID, v.ProductoID, v.Cantidad, v.PrecioVenta })
                .Where(v => v.Key.VentaNoAplicadaID == id)
                .Select(v => new
                {
                    v.Key.ProductoID,
                    Cantidad = v.Sum(c => c.Cantidad),
                    v.Key.PrecioVenta,
                    Importe = v.Sum(i => i.Cantidad * i.PrecioVenta)
                }).ToList();

            decimal importeTotal = ventasNoAplicadasDetalleAgrupada.Sum(v => v.Importe);
            if (importeTotal > importe)
                return Json(new { Estatus = "Importe inferior al total.", Error = true });

            decimal? folio = await _context.Ventas.MaxAsync(v => (decimal?)v.Folio) ?? 0;
            if (folio == null)
                folio = 0;
            folio += 1;

            //ca,biar informaciopn
            Guid almacenId = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F");
            DateTime fecha = DateTime.Now;

            Venta venta = new Venta()
            {
                AlmacenID = almacenId,
                ClienteID = Guid.Empty,
                Fecha = fecha,
                Folio = (decimal)folio,
                UsuarioID = token.UsuarioID,
                VentaCierreID = null,
                VentaID = (Guid)id
            };

            _context.Ventas.Add(venta);

            List<VentaImporte> ventasImporte = new List<VentaImporte>();
            VentaImporte ventaImporte = new VentaImporte()
            {
                FormaPagoID = 1,
                Importe = importeTotal,
                VentaID = (Guid)id,
                VentaImporteID = Guid.NewGuid()
            };

            ventasImporte.Add(ventaImporte);
            _context.VentasImportes.Add(ventaImporte);

            List<VentaDetalle> ventasDetalle = new List<VentaDetalle>();

            foreach (var item in ventasNoAplicadasDetalleAgrupada)
            {
                var producto = await _getHelper.GetProductByIdAsync(item.ProductoID);
                if (producto == null)
                {
                    TempData["toast"] = "Producto incorrecto, venta no realizada";
                    return Json(new { Reiniciar = true, Error = true });
                }

                Guid productoId = producto.ProductoID;
                decimal cantidad = item.Cantidad;

                if (producto.Unidades.Pieza)
                {
                    cantidad = Math.Round(cantidad);
                }

                if (producto.Unidades.Paquete)
                {
                    var productoPieza = await _context.Paquetes
                        .FirstOrDefaultAsync(p => p.ProductoID == item.ProductoID);

                    if (productoPieza != null)
                    {
                        productoId = productoPieza.PiezaProductoID;
                        cantidad = cantidad * productoPieza.CantidadProductoxPaquete;
                    }
                }

                var existencia = await _context.Existencias
                    .FirstOrDefaultAsync(e => e.ProductoID == productoId &&
                                              e.AlmacenID == almacenId);

                if (existencia == null)
                {
                    _context.Existencias.Add(new Existencia()
                    {
                        AlmacenID = almacenId,
                        ExistenciaEnAlmacen = cantidad * -1,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = productoId
                    });
                }
                else
                {
                    existencia.ExistenciaEnAlmacen -= cantidad;
                    _context.Update(existencia);
                }

                var ventaDetalle = new VentaDetalle()
                {
                    Cantidad = item.Cantidad,
                    PrecioCosto = (decimal)producto.PrecioCosto,
                    PrecioVenta = item.PrecioVenta,
                    ProductoID = item.ProductoID,
                    VentaDetalleID = Guid.NewGuid(),
                    VentaID = (Guid)id
                };

                ventasDetalle.Add(ventaDetalle);

                _context.VentasDetalle.Add(ventaDetalle);
            }

            VentasViewModel ventaViewModel = new VentasViewModel()
            {
                AlmacenID = almacenId,
                ClienteID = Guid.Empty,
                Fecha = fecha,
                Folio = (decimal)folio,
                ImporteCobro = importe,
                ImporteTotal = importeTotal,
                UsuarioID = token.UsuarioID,
                VentaCierreID = null,
                VentaID = (Guid)id,
                VentasDetalle = ventasDetalle,
                VentasImportes = ventasImporte
            };

            EstadisticaMovimientoViewModel estadisticaMovimiento = new EstadisticaMovimientoViewModel()
            {
                AlmacenID = almacenId,
                DB = _context,
                Importe = importeTotal,
                Movimiento = TipoMovimiento.Venta
            };

            await _dashboard.GuardarEstadisticaDeMovimientoAsync(estadisticaMovimiento);

            try
            {
                foreach (var item in ventasNoAplicadasDetalle)
                {
                    _context.VentasNoAplicadasDetalle.Remove(item);
                }
                _context.VentasNoAplicadas.Remove(ventaNoAplicada);

                await _context.SaveChangesAsync();
                await BitacoraAsync("SetSale", ventaViewModel);

                //return Json(new { Error = false });
                return PartialView(ventaViewModel);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                await BitacoraAsync("SetSale", ventaViewModel, excepcion);
                return Json(new { Estatus = "Venta no realizada.", Error = true });
            }
        }

        public async Task<IActionResult> SetSaveSale(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return Json(new { Reiniciar = true, Error = true });
            }

            if (id == null || id == Guid.Empty)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }
            var ventaNoAplicada = await _context.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }

            var ventasNoAplicadasDetalle = await _context.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
                return Json(new { Estatus = "No hay registros para almacenar.", Error = true });

            var ventasConDetalle = await (from v in _context.VentasNoAplicadas
                                          join d in _context.VentasNoAplicadasDetalle
                                          on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                          where v.UsuarioID == token.UsuarioID
                                          select v.VentaNoAplicadaID
                                          ).Distinct().ToListAsync();

            if (ventasConDetalle != null)
            {
                ventaNoAplicada = await _context.VentasNoAplicadas
                    .FirstOrDefaultAsync(v => !ventasConDetalle.Contains(v.VentaNoAplicadaID) &&
                                         v.UsuarioID == token.UsuarioID);
            }

            if (ventasConDetalle == null || ventaNoAplicada == null)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = token.UsuarioID,
                    VentaNoAplicadaID = Guid.NewGuid()
                };

                _context.VentasNoAplicadas.Add(ventaNoAplicada);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return Json(new { Estatus = "La venta no puede ser inicializada.", Error = true });
                }
            }

            return Json(new { Error = false });
        }

        public async Task<IActionResult> GetWithdrawCashApply(decimal total)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return Json(new { Reiniciar = true, Error = true }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
                return Json(new { Reiniciar = true, Error = true });

            if (total <= 0)
                return Json(new { Error = true, Estatus = "Total incorrecto, tiene que ser superior a cero." });

            RetiroCaja retiro = new()
            {
                Fecha = DateTime.Now,
                Importe = total,
                RetiroCajaID = Guid.NewGuid(),
                UsuarioID = token.UsuarioID,
                VentaCierreID = null
            };
            _context.Add(retiro);

            try
            {
                await _context.SaveChangesAsync();
                TempData["toast"] = "Proceso finalizado con éxito.";
                return Json(new { Error = false });
            }
            catch (Exception)
            {
                return Json(new { Error = true, Estatus = "Error de servidor, proceso no realizado." });
            }
        }

        private async Task BitacoraAsync(string accion, VentasViewModel venta, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                venta, venta.VentaID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, VentaCanceladaViewModel ventaCancelada, Guid usuarioId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                ventaCancelada, usuarioId.ToString(), directorioBitacora, excepcion);
        }
    }
}
