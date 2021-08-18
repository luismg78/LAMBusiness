namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Aplicacion;

    public class ModuloViewModel: Modulo
    {
        public IEnumerable<SelectListItem> ModulosPadresDDL { get; set; }
    }
}
