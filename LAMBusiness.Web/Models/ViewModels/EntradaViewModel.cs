namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class EntradaViewModel: Entrada
    {
        public bool PermisoEscritura { get; set; }
        public ICollection<EntradaDetalle> EntradaDetalle { get; set; }
    }
}
