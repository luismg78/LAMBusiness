namespace LAMBusiness.Web.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Interfaces;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Dashboard.Entidades;

    public class Dashboard : IDashboard
    {
        public async Task GuardarEstadisticaDeMovimientoAsync(EstadisticaMovimientoViewModel estadisticaMovimiento)
        {
            DateTime fecha = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            short año = (short)fecha.Year;
            byte mes = (byte)fecha.Month;

            EstadisticasMovimientosMensual estadisticaMensual = await estadisticaMovimiento.DB.EstadisticasMovimientosMensual
                .FirstOrDefaultAsync(e => e.AlmacenID == estadisticaMovimiento.AlmacenID &&
                                          e.Año == año && e.Mes == mes);

            EstadisticasMovimientosDiario estadisticaDiaria;
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
                estadisticaDiaria = await estadisticaMovimiento.DB.EstadisticasMovimientosDiario
                    .FirstOrDefaultAsync(e => e.EstadisticaMovimientoMensualID == estadisticaMensual.EstadisticaMovimientoMensualID &&
                                              e.Fecha == fecha);
            }

            bool estadisticaDiariaNueva = false;
            if (estadisticaDiaria == null)
            {
                estadisticaDiariaNueva = true;
                estadisticaDiaria = new EstadisticasMovimientosDiario()
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
                estadisticaMovimiento.DB.EstadisticasMovimientosMensual.Add(estadisticaMensual);
            else
                estadisticaMovimiento.DB.EstadisticasMovimientosMensual.Update(estadisticaMensual);

            if (estadisticaDiariaNueva)
                estadisticaMovimiento.DB.EstadisticasMovimientosDiario.Add(estadisticaDiaria);
            else
                estadisticaMovimiento.DB.EstadisticasMovimientosDiario.Update(estadisticaDiaria);
        }
    }
}
