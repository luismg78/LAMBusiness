namespace LAMBusiness.Web.Models.ViewModels
{
    using Data;
    using System;

    public class EstadisticaMovimientoViewModel
    {
        public Guid AlmacenID { get; set; }
        public TipoMovimiento Movimiento { get; set; }
        public decimal Importe { get; set; }
        public DataContext DB { get; set; }
    }

    public enum TipoMovimiento
    {
        Entrada = 0,
        Salida = 1,
        Venta = 2,
        Devolucion = 3
    }
}
