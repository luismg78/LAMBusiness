using System;
using System.Collections.Generic;
using System.Linq;

namespace LAMBusiness.Shared.DTO.Movimiento
{
    public class CorteDeCajaDTO
    {
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal ImporteDelSistema { get; set; }
        public decimal ImporteDelUsuario { get; set; }
        public List<FormasDePagoDTO> FormasDePagoDetalle { get; set; } = null!;
        public decimal Cambio => FormasDePagoDetalle.Sum(x => x.Importe) - ImporteDelSistema;
    }

    public class FormasDePagoDTO
    {
        public byte FormaDePagoId { get; set; }
        public string Nombre { get; set; }
        public decimal Importe { get; set; }
    }
}
