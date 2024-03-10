using System.Collections.Generic;

namespace LAMBusiness.Shared.DTO.Movimiento
{
    public class TicketDeVentaDTO
    {
        public string RFC { get; set; }
        public string NombreDeLaSucursal { get; set; }
        public string DomicilioDeLaSucursal { get; set; }
        public string ColoniaDeLaSucursal { get; set; }
        public string LugarDeLaSucursal { get; set; }
        public string AtendidoPor { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string ImporteTotalDeVenta { get; set; }
        public List<TicketDeVentaDetalleDTO> DetalleDeVenta { get; set; }
    }
    public class TicketDeVentaDetalleDTO
    {
        public string NombreDelProducto { get; set; }
        public string CantidadPorPrecioDeVenta { get; set; }
        public string Importe { get; set; }
    }
}
