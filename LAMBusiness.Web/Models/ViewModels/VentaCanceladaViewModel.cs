namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class VentaCanceladaViewModel
    {
        public VentaCancelada VentasCanceladas { get; set; }
        public List<VentaCanceladaDetalle> VentasCanceladasDetalle { get; set; }
    }
}
