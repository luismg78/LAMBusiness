namespace LAMBusiness.Shared.DTO.Movimiento
{
    using System.Collections.Generic;
    using Shared.Movimiento;
    public class VentasNoAplicadasDTO : VentaNoAplicada
    {
        public decimal ImporteTotal { get; set; }
        public int TotalDeRegistrosPendientes { get; set; }
        public bool HayVentasPorCerrar { get; set; }
        public ICollection<VentaNoAplicadaDetalle> VentasNoAplicadasDetalle { get; set; }
    }
}
