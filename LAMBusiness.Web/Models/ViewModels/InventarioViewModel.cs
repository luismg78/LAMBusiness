namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Movimiento;

    public class InventarioViewModel: Inventario
    {
        public bool PermisoEscritura { get; set; }
        public ICollection<InventarioDetalle> InventarioDetalle { get; set; }
    }
}
