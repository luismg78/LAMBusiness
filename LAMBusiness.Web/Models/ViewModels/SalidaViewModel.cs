namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Movimiento;

    public class SalidaViewModel: Salida
    {
        public IEnumerable<SelectListItem> SalidaTipoDDL { get; set; }
        public ICollection<SalidaDetalle> SalidaDetalle { get; set; }
    }
}
