using System.Collections.Generic;

namespace LAMBusiness.Web.Models.ViewModels
{
    public class CorteDeCajaViewModel
    {
        public string ImporteDelSistema { get; set; }
        public string ImporteDelUsuario { get; set; }
        public List<ImporteDelSistemaDetalle> ImporteDelSistemaDetalle { get; set; } = null!;
    }

    public class ImporteDelSistemaDetalle
    {
        public byte FormaDePagoId { get; set; }
        public string FormaDePago { get; set; }
        public decimal Importe { get; set; }
    }
}
