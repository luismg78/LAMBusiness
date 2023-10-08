namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;
    public class VentasNoAplicadasViewModel : VentaNoAplicada
    {
        public decimal ImporteTotal { get; set; }
        public int TotalDeRegistrosPendientes { get; set; }
        public ICollection<VentaNoAplicadaDetalle> VentasNoAplicadasDetalle { get; set; }
    }
}
