using System;

namespace LAMBusiness.Shared.Dashboard
{
    public class EstadisticasPorUsuario
    {
        public Guid UsuarioId { get; set; }
        public string Usuario { get; set; }
        public int RetirosDeCaja { get; set; }
        public int CortesDeCaja { get; set; }
        public int TotalDeVentas { get; set; }
        public int TotalDeVentasCerradas { get; set; }
        public int TotalDeVentasPendientesDeCierre { get; set; }
        public int TotalDeCancelacionesParciales { get; set; }
        public int TotalDeCancelacionesCompletas { get; set; }
    }
}
