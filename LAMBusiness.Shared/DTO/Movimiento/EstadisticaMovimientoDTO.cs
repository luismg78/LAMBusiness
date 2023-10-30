namespace LAMBusiness.Shared.DTO.Movimiento
{
    using System;

    public class EstadisticaMovimientoDTO
    {
        public Guid AlmacenID { get; set; }
        public TipoMovimiento Movimiento { get; set; }
        public decimal Importe { get; set; }
    }

    public enum TipoMovimiento
    {
        Entrada = 0,
        Salida = 1,
        Venta = 2,
        Devolucion = 3
    }
}
