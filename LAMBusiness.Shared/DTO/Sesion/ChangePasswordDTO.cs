namespace LAMBusiness.Shared.DTO.Sesion
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordDTO
    {
        public Guid UsuarioID { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo es requerido.")]
        public string PasswordEncrypt { get; set; }

        [Required(ErrorMessage = "El campo es requerido.")]
        public string ConfirmPasswordEncrypt { get; set; }
    }
}
