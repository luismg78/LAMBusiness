namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using Shared.Contacto;

    public class UsuarioDetailsViewModelUsuario: Usuario
    {
        public bool PermisoEscritura { get; set; }
        public List<UsuarioModulo> UsuarioModulos { get; set; }
    }
}
