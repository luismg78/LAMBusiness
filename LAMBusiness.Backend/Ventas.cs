using DocumentFormat.OpenXml.InkML;
using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.DTO.Movimiento;
using LAMBusiness.Shared.Movimiento;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace LAMBusiness.Backend
{
    public class Ventas
    {
        private readonly DataContext _contexto;

        public Ventas(DataContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Resultado<VentasNoAplicadasDTO>> Inicializar(Guid usuarioId) {
            Resultado<VentasNoAplicadasDTO> resultado = new();
            
            var hayVentasPorCerrar = await _contexto.Ventas.AnyAsync(v => v.VentaCierreID == null || v.VentaCierreID == Guid.Empty);

            int totalDeVentasNoAplicadas = 0;
            var resultadoInicioDeVenta = await InicializarVentaAsync(usuarioId);
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
                                                  select v).CountAsync();
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

        private async Task<Resultado<VentaNoAplicada>> InicializarVentaAsync(Guid usuarioId)
        {
            Resultado<VentaNoAplicada>? resultado = new();
            VentaNoAplicada? ventaNoAplicada = null!;
            List<VentaNoAplicada>? ventasNoAplicadas = await _contexto.VentasNoAplicadas
                .Where(v => v.UsuarioID == usuarioId)
                .ToListAsync();

            if (ventasNoAplicadas != null && ventasNoAplicadas.Any())
                ventaNoAplicada = ventasNoAplicadas.OrderByDescending(v => v.Fecha).FirstOrDefault();

            if (ventaNoAplicada == null)
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

            resultado.Datos = ventaNoAplicada;
            return resultado;
        }
    }
}
