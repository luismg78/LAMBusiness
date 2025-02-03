using System;

namespace LAMBusiness.Shared.Kardex
{
    public class KardexDeProductoDetalle
    {
        public string Folio { get; set; }
        public string Movimiento { get; set; }
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public decimal? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Importe => Cantidad * Precio;
        public bool Aplicado { get; set; }
        public string Icono { get; set; } = string.Empty;
        public string IconoColor { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
