namespace LAMBusiness.Shared.DTO.Movimiento
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Shared.Movimiento;

    public class VentasDTO: Venta
    {
        public decimal ImporteTotal { get; set; }
        
        [JsonIgnore]
        public decimal ImporteCobro { get; set; }

        public List<VentaImporte> VentasImportes { get; set; }

        public List<VentaDetalle> VentasDetalle { get; set; }
    }
}
