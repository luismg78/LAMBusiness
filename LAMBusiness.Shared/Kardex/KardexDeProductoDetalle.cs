using System;

namespace LAMBusiness.Shared.Kardex
{
    public class KardexDeProductoDetalle
    {
        public string Movimiento { get; set; }
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public decimal? Cantidad { get; set; }
        public decimal? Existencia { get; set; }
        public bool Aplicado { get; set; }
        public string Icono { get; set; } = string.Empty;
        public string IconoColor { get; set; } = string.Empty;
    }
}
