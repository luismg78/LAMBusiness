﻿using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Dashboard;
using LAMBusiness.Shared.Dashboard.Entidades;
using LAMBusiness.Shared.DTO.Movimiento;
using Microsoft.EntityFrameworkCore;

namespace LAMBusiness.Backend
{
    public class Estadisticas
    {
        public Estadisticas()
        {
        }

        public async Task GuardarEstadisticaDeMovimientoAsync(EstadisticaMovimientoDTO estadisticaMovimiento, DataContext contexto)
        {
            var _contexto = contexto;
            DateTime fecha = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            short año = (short)fecha.Year;
            byte mes = (byte)fecha.Month;

            EstadisticasMovimientosMensual? estadisticaMensual = await _contexto.EstadisticasMovimientosMensual
                .FirstOrDefaultAsync(e => e.AlmacenID == estadisticaMovimiento.AlmacenID &&
                                          e.Año == año && e.Mes == mes);

            EstadisticasMovimientosDiario? estadisticaDiaria;
            bool estadisticaMensualNueva = true;

            if (estadisticaMensual == null)
            {
                estadisticaMensual = new EstadisticasMovimientosMensual()
                {
                    EstadisticaMovimientoMensualID = Guid.NewGuid(),
                    AlmacenID = estadisticaMovimiento.AlmacenID,
                    Año = año,
                    Mes = mes,
                    Devoluciones = 0,
                    DevolucionesImporte = 0,
                    Entradas = 0,
                    EntradasImporte = 0,
                    Salidas = 0,
                    SalidasImporte = 0,
                    Ventas = 0,
                    VentasImporte = 0
                };
                estadisticaDiaria = null;
            }
            else
            {
                estadisticaMensualNueva = false;
                estadisticaDiaria = await _contexto.EstadisticasMovimientosDiario
                    .FirstOrDefaultAsync(e => e.EstadisticaMovimientoMensualID == estadisticaMensual.EstadisticaMovimientoMensualID &&
                                              e.Fecha == fecha);
            }

            bool estadisticaDiariaNueva = false;
            if (estadisticaDiaria == null)
            {
                estadisticaDiariaNueva = true;
                estadisticaDiaria = new()
                {
                    EstadisticaMovimientoDiarioID = Guid.NewGuid(),
                    EstadisticaMovimientoMensualID = estadisticaMensual.EstadisticaMovimientoMensualID,
                    Fecha = fecha,
                    Dia = fecha.ToString("dddd").ToUpper(),
                    Devoluciones = 0,
                    DevolucionesImporte = 0,
                    Entradas = 0,
                    EntradasImporte = 0,
                    Salidas = 0,
                    SalidasImporte = 0,
                    Ventas = 0,
                    VentasImporte = 0
                };
            }
            switch (estadisticaMovimiento.Movimiento)
            {
                case TipoMovimiento.Entrada:
                    estadisticaMensual.Entradas += 1;
                    estadisticaMensual.EntradasImporte += estadisticaMovimiento.Importe;
                    estadisticaDiaria.Entradas += 1;
                    estadisticaDiaria.EntradasImporte += estadisticaMovimiento.Importe;
                    break;
                case TipoMovimiento.Devolucion:
                    estadisticaMensual.Devoluciones += 1;
                    estadisticaMensual.DevolucionesImporte += estadisticaMovimiento.Importe;
                    estadisticaDiaria.Devoluciones += 1;
                    estadisticaDiaria.DevolucionesImporte += estadisticaMovimiento.Importe;
                    break;
                case TipoMovimiento.Salida:
                    estadisticaMensual.Salidas += 1;
                    estadisticaMensual.SalidasImporte += estadisticaMovimiento.Importe;
                    estadisticaDiaria.Salidas += 1;
                    estadisticaDiaria.SalidasImporte += estadisticaMovimiento.Importe;
                    break;
                case TipoMovimiento.Venta:
                    estadisticaMensual.Ventas += 1;
                    estadisticaMensual.VentasImporte += estadisticaMovimiento.Importe;
                    estadisticaDiaria.Ventas += 1;
                    estadisticaDiaria.VentasImporte += estadisticaMovimiento.Importe;
                    break;

            }

            if (estadisticaMensualNueva)
                _contexto.EstadisticasMovimientosMensual.Add(estadisticaMensual);
            else
                _contexto.EstadisticasMovimientosMensual.Update(estadisticaMensual);

            if (estadisticaDiariaNueva)
                _contexto.EstadisticasMovimientosDiario.Add(estadisticaDiaria);
            else
                _contexto.EstadisticasMovimientosDiario.Update(estadisticaDiaria);
        }

        public static async Task<EstadisticasDeMovimientos> ObtenerEstadisticasDeEntradas(string cadenaDeConexion)
        {
            DataContext contexto = new(cadenaDeConexion);
            var entradas = await (from e in contexto.Entradas
                                  group e by e.Aplicado into g
                                  select new EstadisticasDeMovimientos
                                  {
                                      Movimiento = "Entradas",
                                      TotalDeMovimientos = g.Count(),
                                      TotalDeMovimientosAplicados = g.Count(x => x.Aplicado == true),
                                      TotalDeMovimientosNoAplicados = g.Count(x => x.Aplicado == false)
                                  }).FirstOrDefaultAsync();
            if (entradas == null)
            {
                return new EstadisticasDeMovimientos()
                {
                    Movimiento = "Entradas",
                    TotalDeMovimientos = 0,
                    TotalDeMovimientosAplicados = 0,
                    TotalDeMovimientosNoAplicados = 0
                };
            }

            return entradas;
        }

        public static async Task<EstadisticasDeMovimientos> ObtenerEstadisticasDeSalidas(string cadenaDeConexion)
        {
            DataContext contexto = new(cadenaDeConexion);
            var salidas = await (from e in contexto.Salidas
                                 group e by e.Aplicado into g
                                 select new EstadisticasDeMovimientos
                                 {
                                     Movimiento = "Salidas",
                                     TotalDeMovimientos = g.Count(),
                                     TotalDeMovimientosAplicados = g.Count(x => x.Aplicado == true),
                                     TotalDeMovimientosNoAplicados = g.Count(x => x.Aplicado == false)
                                 }).FirstOrDefaultAsync();
            if (salidas == null)
            {
                return new EstadisticasDeMovimientos()
                {
                    Movimiento = "Salidas",
                    TotalDeMovimientos = 0,
                    TotalDeMovimientosAplicados = 0,
                    TotalDeMovimientosNoAplicados = 0
                };
            }

            return salidas;
        }

        public static async Task<EstadisticasDeProductos> ObtenerEstadisticasDeProductos(string cadenaDeConexion)
        {
            DataContext contexto = new(cadenaDeConexion);
            var negativos = await contexto.Existencias.CountAsync(p => p.ExistenciaEnAlmacen < 0);
            return new EstadisticasDeProductos()
            {
                ProductosNegativos = negativos
            };
        }
    }
}
