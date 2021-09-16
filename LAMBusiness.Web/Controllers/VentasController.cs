namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Movimiento;
    using System.Collections.Generic;

    public class VentasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("a0ca4d51-b518-4a65-b1e3-f0a03b1caff8");

        public VentasController(DataContext context, IGetHelper getHelper,
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

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            VentaNoAplicada ventaNoAplicada = await _context.VentasNoAplicadas
                .OrderByDescending(v => v.Fecha)
                .FirstOrDefaultAsync(v => v.UsuarioID == token.UsuarioID);

            if(ventaNoAplicada == null)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = token.UsuarioID,
                    VentaNoAplicadaID = Guid.NewGuid()
                };

                _context.VentasNoAplicadas.Add(ventaNoAplicada);
            }

            decimal total = 0;
            var ventaDetalle = await _context.VentasNoAplicadasDetalle
                                    .Include(v => v.Productos)
                                    .Where(v => v.VentaNoAplicadaID == ventaNoAplicada.VentaNoAplicadaID)
                                    .ToListAsync();
            if(ventaDetalle != null)
            {
                foreach(var item in ventaDetalle)
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

            ViewBag.Id = token.ColaboradorID;

            try
            {
                await _context.SaveChangesAsync();
                return View(venta);
            }
            catch (Exception)
            {
                TempData["toast"] = "La venta no puede ser inicializada.";
                return RedirectToAction("Index", "Movimiento");
            }
        }

        public async Task<IActionResult> GetProductByCode(Guid? id, string codigo, decimal cantidad)
        {
            var resultado = await _getHelper.GetProductByCodeForSale(id, codigo, cantidad);

            if (resultado.Error)
            {
                switch (resultado.Mensaje.Trim().ToLower())
                {
                    case "buscarproducto":
                        return Json(new { BuscarProducto = true, Error = true});
                    case "reiniciar":
                        TempData["toast"] = "Identificador de la venta incorrecto.";
                        return Json(new { Reiniciar = true, Error = true});
                    default:
                        return Json(new { Estatus = resultado.Mensaje, Error = true});
                }
            }

            return PartialView(resultado.Contenido);
        }

        public async Task<IActionResult> SetSale(Guid? id, decimal importe)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null || id == Guid.Empty)
            {
                TempData["toast"] = "Identificador de la venta incorrecto.";
                return Json(new { Reiniciar = true, Error = true });
            }
            var ventaNoAplicada = await _context.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);
            
            if(ventaNoAplicada == null)
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
            if(importeTotal > importe)
                return Json(new { Estatus = "Importe inferior al total.", Error = true });

            decimal? folio = await _context.Ventas.MaxAsync(v => (decimal?)v.Folio) ?? 0;
            if (folio == null)
                folio = 0;

            folio += 1;

            //ca,biar informaciopn
            Guid almacenId = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F");

            Venta venta = new Venta()
            {
                AlmacenID = almacenId,
                ClienteID = Guid.Empty,
                Fecha = ventaNoAplicada.Fecha,
                FechaFinProceso = DateTime.Now,
                Folio = (decimal)folio + 1,
                UsuarioID = token.UsuarioID,
                VentaCierreID = Guid.Empty,
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

            foreach(var item in ventasNoAplicadasDetalleAgrupada)
            {
                var producto = await _getHelper.GetProductByIdAsync(item.ProductoID);
                if(producto == null)
                {
                    TempData["toast"] = "Producto incorrecto, venta no realizada";
                    return Json(new { Reiniciar = true, Error = true });
                }

                Guid productoId = producto.ProductoID;
                decimal cantidad = item.Cantidad;

                if (producto.Unidades.Pieza)
                {
                    cantidad = Convert.ToDecimal((int)cantidad);
                }

                if (producto.Unidades.Paquete)
                {
                    var productoPieza = await _context.Paquetes
                        .FirstOrDefaultAsync(p => p.ProductoID == item.ProductoID);
                    
                    if(productoPieza != null)
                    {
                        productoId = productoPieza.ProductoID;
                        cantidad = cantidad * productoPieza.CantidadProductoxPaquete;
                    }
                }

                var existencia = await _context.Existencias
                    .FirstOrDefaultAsync(e => e.ProductoID == productoId &&
                                              e.AlmacenID == almacenId);

                if(existencia == null)
                {
                    _context.Existencias.Add(new Existencia()
                    {
                        AlmacenID = almacenId,
                        ExistenciaEnAlmacen = cantidad * -1,
                        ExistenciaID = Guid.NewGuid(),
                        ProductoID = item.ProductoID
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
                Fecha = ventaNoAplicada.Fecha,
                FechaFinProceso = DateTime.Now,
                Folio = (decimal)folio,
                ImporteTotal = importeTotal,
                UsuarioID = token.UsuarioID,
                VentaCierreID = Guid.Empty,
                VentaID = (Guid)id,
                VentasDetalle = ventasDetalle,
                VentasImportes = ventasImporte
            };

            try
            {
                foreach (var item in ventasNoAplicadasDetalle)
                {
                    _context.VentasNoAplicadasDetalle.Remove(item);
                }
                _context.VentasNoAplicadas.Remove(ventaNoAplicada);

                await _context.SaveChangesAsync();
                await BitacoraAsync("SetSale", ventaViewModel);
                return Json(new { Error = false });
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                await BitacoraAsync("SetSale", ventaViewModel, excepcion);
                return Json(new { Estatus = "Venta no realizada.", Error = true });
            }
        }

        private async Task BitacoraAsync(string accion, VentasViewModel venta, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                venta, venta.VentaID.ToString(), directorioBitacora, excepcion);
        }
    }
}
