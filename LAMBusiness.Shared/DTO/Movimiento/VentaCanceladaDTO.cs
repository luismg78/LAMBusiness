namespace LAMBusiness.Shared.DTO.Movimiento
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class VentaCanceladaDTO
    {
        public VentaCancelada VentasCanceladas { get; set; }
        public List<VentaCanceladaDetalle> VentasCanceladasDetalle { get; set; }
    }
}
