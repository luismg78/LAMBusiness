namespace LAMBusiness.Shared.Aplicacion
{
    using LAMBusiness.Shared.Contacto;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Sesion
    {
        [Key]
        [Display(Name = "Sesión")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid SesionID { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string SessionID { get; set; }

        [Display(Name = "Usuario")]
        [ForeignKey("Usuarios")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime Fecha { get; set; }

    }
}
