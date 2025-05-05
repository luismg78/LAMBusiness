namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Shared.Movimiento;

    public class VentasViewModel: Venta
    {
		public bool PermisoEscritura { get; set; }
		public string Nombre { get; set; }
		public string PrimerApellido { get; set; }
		public string SegundoApellido { get; set; }
		public decimal ImporteTotal { get; set; }
        [JsonIgnore]
        public decimal ImporteCobro { get; set; }
        public List<VentaDetalle> VentasDetalle { get; set; }
    }
}
