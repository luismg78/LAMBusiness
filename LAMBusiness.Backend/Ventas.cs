﻿using DocumentFormat.OpenXml.InkML;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Dashboard;
using LAMBusiness.Shared.DTO.Movimiento;
using LAMBusiness.Shared.Movimiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace LAMBusiness.Backend
{
    public class Ventas
    {
        private readonly DataContext _contexto;
        private readonly Productos _productos;
        private readonly Estadisticas _estadisticas;

        public Ventas(DataContext contexto)
        {
            _contexto = contexto;
            _productos = new Productos(contexto);
            _estadisticas = new Estadisticas();
        }

        public async Task<Resultado> Agregar(Guid? id, Guid usuarioId)
        {
            Resultado resultado = new();

            if (id == null || id == Guid.Empty)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }
            var ventaNoAplicada = await _contexto.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }

            var ventasNoAplicadasDetalle = await _contexto.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "No hay registros para almacenar.";
                return resultado;
            }

            var ventasConDetalle = await (from v in _contexto.VentasNoAplicadas
                                          join d in _contexto.VentasNoAplicadasDetalle
                                          on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                          where v.UsuarioID == usuarioId
                                          select v.VentaNoAplicadaID
                                          ).Distinct().ToListAsync();

            if (ventasConDetalle != null)
            {
                ventaNoAplicada = await _contexto.VentasNoAplicadas
                    .FirstOrDefaultAsync(v => !ventasConDetalle.Contains(v.VentaNoAplicadaID) &&
                                         v.UsuarioID == usuarioId);
            }

            if (ventasConDetalle == null || ventaNoAplicada == null)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = usuarioId,
                    VentaNoAplicadaID = Guid.NewGuid()
                };

                _contexto.VentasNoAplicadas.Add(ventaNoAplicada);

                try
                {
                    await _contexto.SaveChangesAsync();
                    resultado.Mensaje = "Proceso realizado correctamente.";
                }
                catch (Exception ex)
                {
                    resultado.Error = true;
                    resultado.Mensaje = "Error. Proceso no realizado.";
                    resultado.Excepcion = ex.InnerException == null ? ex.ToString() : ex.InnerException.Message;
                }
            }

            return resultado;
        }

        public async Task<Resultado<VentasDTO>> Aplicar(Guid? id, Guid usuarioId, decimal importe)
        {
            Resultado<VentasDTO> resultado = new();

            if (id == null || id == Guid.Empty)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }
            var ventaNoAplicada = await _contexto.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }

            var ventasNoAplicadasDetalle = await _contexto.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "Ingrese al menos un producto a la lista.";
                return resultado;
            }

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
            {
                resultado.Error = true;
                resultado.Mensaje = "Importe inferior al total.";
                return resultado;
            }

            decimal? folio = await _contexto.Ventas.MaxAsync(v => (decimal?)v.Folio) ?? 0;
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
                UsuarioID = usuarioId,
                VentaCierreID = null,
                VentaID = (Guid)id
            };

            _contexto.Ventas.Add(venta);

            List<VentaImporte> ventasImporte = new List<VentaImporte>();
            VentaImporte ventaImporte = new VentaImporte()
            {
                FormaPagoID = 1,
                Importe = importeTotal,
                VentaID = (Guid)id,
                VentaImporteID = Guid.NewGuid()
            };

            ventasImporte.Add(ventaImporte);
            _contexto.VentasImportes.Add(ventaImporte);

            List<VentaDetalle> ventasDetalle = new List<VentaDetalle>();

            foreach (var item in ventasNoAplicadasDetalleAgrupada)
            {
                var producto = await _productos.ObtenerRegistroPorIdAsync(item.ProductoID);
                if (producto == null)
                {
                    resultado.Error = true;
                    resultado.Reiniciar = true;
                    resultado.Mensaje = "Producto incorrecto, venta no realizada";
                    return resultado;
                }

                Guid productoId = producto.ProductoID;
                decimal cantidad = item.Cantidad;

                if (producto.Unidades.Pieza)
                {
                    cantidad = Math.Round(cantidad);
                }

                if (producto.Unidades.Paquete)
                {
                    var productoPieza = await _contexto.Paquetes
                        .FirstOrDefaultAsync(p => p.ProductoID == item.ProductoID);

                    if (productoPieza != null)
                    {
                        productoId = productoPieza.PiezaProductoID;
                        cantidad = cantidad * productoPieza.CantidadProductoxPaquete;
                    }
                }

                var existencia = await _contexto.Existencias
                    .FirstOrDefaultAsync(e => e.ProductoID == productoId &&
                                              e.AlmacenID == almacenId);

                if (existencia == null)
                {
                    _contexto.Existencias.Add(new Existencia()
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
                    _contexto.Update(existencia);
                }

                var ventaDetalle = new VentaDetalle()
                {
                    Cantidad = item.Cantidad,
                    PrecioCosto = producto.PrecioCosto ?? 0,
                    PrecioVenta = item.PrecioVenta,
                    ProductoID = item.ProductoID,
                    VentaDetalleID = Guid.NewGuid(),
                    VentaID = (Guid)id
                };

                ventasDetalle.Add(ventaDetalle);

                _contexto.VentasDetalle.Add(ventaDetalle);
            }

            VentasDTO ventaDTO = new()
            {
                AlmacenID = almacenId,
                ClienteID = Guid.Empty,
                Fecha = fecha,
                Folio = (decimal)folio,
                ImporteCobro = importe,
                ImporteTotal = importeTotal,
                UsuarioID = usuarioId,
                VentaCierreID = null,
                VentaID = (Guid)id,
                VentasDetalle = ventasDetalle,
                VentasImportes = ventasImporte
            };

            EstadisticaMovimientoDTO estadisticaMovimiento = new()
            {
                AlmacenID = almacenId,
                Importe = importeTotal,
                Movimiento = TipoMovimiento.Venta
            };

            await _estadisticas.GuardarEstadisticaDeMovimientoAsync(estadisticaMovimiento, _contexto);

            try
            {
                foreach (var item in ventasNoAplicadasDetalle)
                {
                    _contexto.VentasNoAplicadasDetalle.Remove(item);
                }
                _contexto.VentasNoAplicadas.Remove(ventaNoAplicada);

                await _contexto.SaveChangesAsync();
                resultado.Datos = ventaDTO;
                resultado.Mensaje = "Proceso realizado correctamente.";
            }
            catch (Exception ex)
            {
                resultado.Error = true;
                resultado.Excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                resultado.Mensaje = "Venta no realizada.";
            }

            return resultado;
        }

        public async Task<Resultado<VentaCanceladaDTO>> CancelarVenta(Guid? id, Guid usuarioId)
        {
            Resultado<VentaCanceladaDTO> resultado = new();

            if (id == null || id == Guid.Empty)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }
            var ventaNoAplicada = await _contexto.VentasNoAplicadas
                .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                resultado.Error = true;
                resultado.Reiniciar = true;
                resultado.Mensaje = "Identificador de la venta incorrecto.";
                return resultado;
            }

            var ventasNoAplicadasDetalle = await _contexto.VentasNoAplicadasDetalle
                .Where(v => v.VentaNoAplicadaID == id).ToListAsync();

            if (ventasNoAplicadasDetalle == null || ventasNoAplicadasDetalle.Count == 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "No hay registros para cancelar.";
                return resultado;
            }

            Guid ventaCanceladaId = Guid.NewGuid();

            VentaCancelada ventaCancelada = new()
            {
                Fecha = DateTime.Now,
                UsuarioID = usuarioId,
                VentaCanceladaID = ventaCanceladaId,
                VentaCompleta = true
            };

            _contexto.VentasCanceladas.Add(ventaCancelada);

            List<VentaCanceladaDetalle> ventasCanceladasDetalle = new();
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
                _contexto.VentasCanceladasDetalle.Add(ventaCanceladaDetalle);

                _contexto.Remove(item);
            }

            _contexto.Remove(ventaNoAplicada);

            VentaCanceladaDTO ventaCanceladaDTO = new()
            {
                VentasCanceladas = ventaCancelada,
                VentasCanceladasDetalle = ventasCanceladasDetalle
            };

            try
            {
                resultado.Datos = ventaCanceladaDTO;
                await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultado.Error = true;
                resultado.Mensaje = "Cancelación no realizada.";
                resultado.Excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
            }

            return resultado;
        }

        public async Task<Resultado<CorteDeCajaDTO>> CerrarVentas(Guid usuarioId)
        {
            Resultado<CorteDeCajaDTO> resultado = new();

            Guid ventaDeCierreId = Guid.NewGuid();

            var importesDelSistema = await (from v in _contexto.Ventas
                                            join vi in _contexto.VentasImportes on v.VentaID equals vi.VentaID
                                            join fp in _contexto.FormasPago on vi.FormaPagoID equals fp.FormaPagoID
                                            where v.UsuarioID == usuarioId && (v.VentaCierreID == null || v.VentaCierreID == Guid.Empty)
                                            group new { fp, vi } by new { fp.FormaPagoID, fp.Nombre } into g
                                            select new ImporteDelSistemaDetalle()
                                            {
                                                FormaDePagoId = g.Key.FormaPagoID,
                                                FormaDePago = g.Key.Nombre,
                                                Importe = g.Sum(a => a.vi.Importe)
                                            }).ToListAsync();

            var ventas = await (from v in _contexto.Ventas
                                join vi in _contexto.VentasImportes on v.VentaID equals vi.VentaID
                                join fp in _contexto.FormasPago on vi.FormaPagoID equals fp.FormaPagoID
                                where v.UsuarioID == usuarioId && (v.VentaCierreID == null || v.VentaCierreID == Guid.Empty)
                                select v).ToListAsync();

            if (ventas == null || !ventas.Any())
            {
                resultado.Error = true;
                resultado.Mensaje = "Proceso no realizado, no hay registro de ventas con ese identificador de usuario.";
                return resultado;
            }

            foreach (var venta in ventas)
            {
                venta.VentaCierreID = ventaDeCierreId;
                _contexto.Update(venta);
            }

            var importeDelUsuario = await _contexto.RetirosCaja
                .Where(r => r.UsuarioID == usuarioId && (r.VentaCierreID == null || r.VentaCierreID == Guid.Empty)).ToListAsync();

            if (importeDelUsuario == null || !importeDelUsuario.Any())
            {
                resultado.Error = true;
                resultado.Mensaje = "Proceso no realizado, ingrese al menos un retiro de caja.";
                return resultado;
            }

            foreach (var retiro in importeDelUsuario)
            {
                retiro.VentaCierreID = ventaDeCierreId;
                _contexto.Update(retiro);
            }

            decimal totalSistema = importesDelSistema == null || !importesDelSistema.Any() ? 0 : importesDelSistema.Sum(i => i.Importe);
            decimal totalUsuario = importeDelUsuario == null || !importeDelUsuario.Any() ? 0 : importeDelUsuario.Sum(r => r.Importe);

            _contexto.VentasCierre.Add(new VentaCierre()
            {
                Fecha = DateTime.Now,
                ImporteSistema = totalSistema,
                ImporteUsuario = totalUsuario,
                UsuarioCajaID = usuarioId,
                UsuarioID = usuarioId,
                VentaCierreID = ventaDeCierreId
            });

            if (importesDelSistema != null && importesDelSistema.Any())
            {
                foreach (var item in importesDelSistema)
                {
                    _contexto.VentasCierreDetalle.Add(new VentaCierreDetalle()
                    {
                        FormaPagoID = item.FormaDePagoId,
                        Importe = item.Importe,
                        VentaCierreDetalleID = Guid.NewGuid(),
                        VentaCierreID = ventaDeCierreId
                    });
                }
            }

            var corteDeCaja = new CorteDeCajaDTO()
            {
                ImporteDelSistema = totalSistema.ToString("$###,###,##0.00"),
                ImporteDelUsuario = totalUsuario.ToString("$###,###,##0.00"),
                ImporteDelSistemaDetalle = importesDelSistema
            };

            try
            {
                await _contexto.SaveChangesAsync();
                resultado.Datos = corteDeCaja;
            }
            catch (Exception)
            {
                resultado.Error = true;
                resultado.Mensaje = "Error en el servidor. Proceso no realizado.";
            }

            return resultado;
        }

        public async Task<Resultado<VentasNoAplicadasDTO>> Inicializar(Guid usuarioId, bool nuevaVenta)
        {
            Resultado<VentasNoAplicadasDTO> resultado = new();
#warning hay un detalle al recuperar una venta y presionar la tecla esc
            var hayVentasPorCerrar = await _contexto.Ventas.AnyAsync(v => v.VentaCierreID == null || v.VentaCierreID == Guid.Empty);

            int totalDeVentasNoAplicadas = 0;
            var resultadoInicioDeVenta = await InicializarVentaAsync(usuarioId, nuevaVenta);
            if (resultadoInicioDeVenta.Error)
            {
                resultado.Error = true;
                resultado.Mensaje = resultadoInicioDeVenta.Mensaje;
                return resultado;
            }

            VentaNoAplicada? ventaNoAplicada = resultadoInicioDeVenta.Datos;

            if (ventaNoAplicada == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "La venta no puede ser inicializada.";
                return resultado;
            }
            else
            {
                totalDeVentasNoAplicadas = await (from v in _contexto.VentasNoAplicadas
                                                  join d in _contexto.VentasNoAplicadasDetalle on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                                  where v.UsuarioID == usuarioId
                                                  select v).Distinct().CountAsync();
            }

            resultado.Datos = new()
            {
                Fecha = ventaNoAplicada.Fecha,
                HayVentasPorCerrar = hayVentasPorCerrar,
                ImporteTotal = 0,
                UsuarioID = usuarioId,
                VentaNoAplicadaID = ventaNoAplicada.VentaNoAplicadaID,
                VentasNoAplicadasDetalle = null,
                TotalDeRegistrosPendientes = totalDeVentasNoAplicadas,
            };

            return resultado;
        }

        private async Task<Resultado<VentaNoAplicada>> InicializarVentaAsync(Guid usuarioId, bool nuevaVenta)
        {
            Resultado<VentaNoAplicada>? resultado = new();
            VentaNoAplicada? ventaNoAplicada = null!;
            List<VentaNoAplicada>? ventasNoAplicadas = await _contexto.VentasNoAplicadas
                .Where(v => v.UsuarioID == usuarioId)
                .ToListAsync();

            if (ventasNoAplicadas != null && ventasNoAplicadas.Any())
                ventaNoAplicada = ventasNoAplicadas.OrderByDescending(v => v.Fecha).FirstOrDefault();
            
            bool iniciarVenta = true;
            if (ventaNoAplicada != null)
            {
                iniciarVenta = await _contexto.VentasNoAplicadasDetalle.AnyAsync(v => v.VentaNoAplicadaID == ventaNoAplicada.VentaNoAplicadaID);
            }

            if (iniciarVenta && nuevaVenta)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = usuarioId,
                    VentaNoAplicadaID = Guid.NewGuid()
                };

                _contexto.VentasNoAplicadas.Add(ventaNoAplicada);
                try
                {
                    await _contexto.SaveChangesAsync();
                }
                catch (Exception)
                {
                    resultado.Error = true;
                    resultado.Mensaje = "La venta no puede ser inicializada.";
                }
            }

            resultado.Datos = ventaNoAplicada!;
            return resultado;
        }

        public async Task<Resultado<VentaNoAplicadaDetalle>> ObtenerProducto(Guid? id, Guid usuarioId, string codigo, decimal cantidad)
        {
            Resultado<VentaNoAplicadaDetalle> resultado = new()
            {
                Datos = null!,
                Error = true,
            };
            codigo = codigo.Trim().ToUpper();

            if (id == null || id == Guid.Empty)
            {
                resultado.Mensaje = "reiniciar";
                return resultado;
            }

            if (cantidad == 0)
            {
                resultado.Mensaje = "Cantidad incorrecta";
                return resultado;
            }

            var _productos = new Productos(_contexto);
            var producto = await _productos.ObtenerRegistroPorCodigoAsync(codigo);
            if (producto == null)
            {
                resultado.Mensaje = "buscarProducto";
                return resultado;
            }

            if (!producto.Activo)
            {
                resultado.Mensaje = "Producto inexistente";
                return resultado;
            }

            if (producto.Unidades.Pieza)
            {
                cantidad = Math.Round(cantidad);
            }

            if (cantidad < 0)
            {
                var productoVendido = await _contexto.VentasNoAplicadasDetalle
                    .Where(v => v.ProductoID == producto.ProductoID).ToListAsync();

                if (productoVendido == null)
                {
                    resultado.Mensaje = "Producto no registrado";
                    return resultado;
                }
                decimal cantidadRestar = cantidad;
                decimal cantidadProducto = productoVendido.Sum(p => p.Cantidad);

                if (Math.Abs(cantidadRestar) > cantidadProducto)
                {
                    resultado.Mensaje = "La cantidad excede a la vendida.";
                    return resultado;
                }

                foreach (var item in productoVendido)
                {
                    if (item.Cantidad > Math.Abs(cantidadRestar))
                    {
                        item.Cantidad += cantidadRestar;
                        _contexto.Update(item);
                        break;
                    }
                    else
                    {
                        cantidadRestar += item.Cantidad;
                        _contexto.Remove(item);
                        if (cantidadRestar == 0)
                            break;
                    }
                }
            }

            var ventaNoAplicada = await _contexto.VentasNoAplicadas.FindAsync(id);
            if (ventaNoAplicada == null)
            {
                ventaNoAplicada = new VentaNoAplicada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = usuarioId,
                    VentaNoAplicadaID = (Guid)id
                };

                _contexto.VentasNoAplicadas.Add(ventaNoAplicada);
            }

            VentaNoAplicadaDetalle ventaDetalle = new VentaNoAplicadaDetalle()
            {
                Cantidad = cantidad,
                PrecioVenta = Convert.ToDecimal(producto.PrecioVenta),
                ProductoID = producto.ProductoID,
                Productos = producto,
                VentaNoAplicadaDetalleID = Guid.NewGuid(),
                VentaNoAplicadaID = (Guid)id,
            };

            if (cantidad > 0)
            {
                _contexto.VentasNoAplicadasDetalle.Add(ventaDetalle);
            }
            else
            {
                Guid ventaCanceladaId = Guid.NewGuid();

                VentaCancelada ventaCancelada = new VentaCancelada()
                {
                    Fecha = DateTime.Now,
                    UsuarioID = usuarioId,
                    VentaCanceladaID = ventaCanceladaId,
                    VentaCompleta = false
                };

                _contexto.VentasCanceladas.Add(ventaCancelada);

                VentaCanceladaDetalle ventaCanceladaDetalle = new VentaCanceladaDetalle()
                {
                    Cantidad = Math.Abs(cantidad),
                    PrecioVenta = Convert.ToDecimal(producto.PrecioVenta),
                    ProductoID = producto.ProductoID,
                    VentaCanceladaID = ventaCanceladaId,
                    VentaCanceladaDetalleID = Guid.NewGuid()
                };

                _contexto.VentasCanceladasDetalle.Add(ventaCanceladaDetalle);
            }

            try
            {
                await _contexto.SaveChangesAsync();
                resultado.Datos = ventaDetalle;
                resultado.Error = false;
                return resultado;
            }
            catch (Exception)
            {
                resultado.Mensaje = "Error al actualizar la venta.";
                return resultado;
            }
        }

        public async Task<Resultado<Filtro<List<VentaNoAplicada>>>> RecuperarVenta(Guid usuarioId)
        {
            Resultado<Filtro<List<VentaNoAplicada>>> resultado = new();

            var ventasNoAplicadas = (from v in _contexto.VentasNoAplicadas
                                     join d in _contexto.VentasNoAplicadasDetalle
                                     on v.VentaNoAplicadaID equals d.VentaNoAplicadaID
                                     orderby v.Fecha descending
                                     where v.UsuarioID == usuarioId
                                     select v).Distinct();

            int registros = await ventasNoAplicadas.CountAsync();
            if (registros == 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "No existe registro de ventas pendientes por aplicar.";
                return resultado;
            }

            resultado.Datos = new()
            {
                Datos = await ventasNoAplicadas.ToListAsync(),
                Registros = registros
            };

            return resultado;
        }

        public async Task<Resultado<VentasNoAplicadasDTO>> RecuperarVentaPorId(Guid? id, Guid usuarioId)
        {
            Resultado<VentasNoAplicadasDTO> resultado = new();

            VentaNoAplicada? ventaNoAplicada = await _contexto.VentasNoAplicadas
                    .OrderByDescending(v => v.Fecha)
                    .FirstOrDefaultAsync(v => v.VentaNoAplicadaID == id);

            if (ventaNoAplicada == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "El identificador de la venta es incorrecta.";
                return resultado;
            }

            decimal total = 0;
            var ventaDetalle = await _contexto.VentasNoAplicadasDetalle
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

            VentasNoAplicadasDTO venta = new()
            {
                Fecha = ventaNoAplicada.Fecha,
                ImporteTotal = total,
                UsuarioID = usuarioId,
                VentaNoAplicadaID = ventaNoAplicada.VentaNoAplicadaID,
                VentasNoAplicadasDetalle = ventaDetalle
            };

            try
            {
                await _contexto.SaveChangesAsync();
                resultado.Datos = venta;
            }
            catch (Exception)
            {
                resultado.Error = true;
                resultado.Mensaje = "La venta no puede ser inicializada.";
            }

            return resultado;
        }

        public async Task<Resultado> RetirarEfectivoDeCaja(decimal total, Guid usuarioId)
        {
            Resultado resultado = new();

            if (total <= 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "Total incorrecto, tiene que ser superior a cero.";
                return resultado;
            }

            RetiroCaja retiro = new()
            {
                Fecha = DateTime.Now,
                Importe = total,
                RetiroCajaID = Guid.NewGuid(),
                UsuarioID = usuarioId,
                VentaCierreID = null
            };
            _contexto.Add(retiro);

            try
            {
                await _contexto.SaveChangesAsync();
                resultado.Mensaje = "Proceso finalizado con éxito.";
            }
            catch (Exception ex)
            {
                resultado.Error = true;
                resultado.Mensaje = "Error de servidor, proceso no realizado.";
                resultado.Excepcion = ex.InnerException == null ? ex.ToString() : ex.InnerException.Message;
            }

            return resultado;
        }
    }
}
