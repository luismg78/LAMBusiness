namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Contacto;

    public class UsuarioViewModel: Usuario
    {
        public IEnumerable<SelectListItem> AdministradoresDDL { get; set; }
    }
}
