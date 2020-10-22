namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class EntradaViewModel: Entrada
    {
        public ICollection<EntradaDetalle> EntradaDetalle { get; set; }
    }
}
