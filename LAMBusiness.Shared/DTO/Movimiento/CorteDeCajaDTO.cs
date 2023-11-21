using System.Collections.Generic;

namespace LAMBusiness.Shared.DTO.Movimiento
{
    public class CorteDeCajaDTO
    {
        public decimal ImporteDelSistema { get; set; }
        public decimal ImporteDelUsuario { get; set; }
        public List<ImporteDelSistemaDetalle> ImporteDelSistemaDetalle { get; set; } = null!;
    }

    public class ImporteDelSistemaDetalle
    {
        public byte FormaDePagoId { get; set; }
        public string FormaDePago { get; set; }
        public decimal Importe { get; set; }
    }
}
