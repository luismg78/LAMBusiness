namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Contacto;

    public class UsuarioDetailsViewModelUsuario: Usuario
    {
        public List<UsuarioModulo> UsuarioModulos { get; set; }
    }
}
