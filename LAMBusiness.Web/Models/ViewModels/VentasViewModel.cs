namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class VentasViewModel: Venta
    {
        public decimal ImporteTotal { get; set; }
        public List<VentaImporte> VentasImportes { get; set; }
        public List<VentaDetalle> VentasDetalle { get; set; }
    }
}
