namespace LAMBusiness.Web.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class InicioSesionViewModel
    {
        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Password { get; set; }
    }
}
