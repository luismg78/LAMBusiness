namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class Usuario
    {
        [Key]
        [Display(Name = "Usuario")]
        public Guid UsuarioID { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PrimerApellido { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono (Móvil)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string TelefonoMovil { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del Correo Electrónico es incorrecto.")]
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        
        [JsonIgnore]
        public bool CambiarPassword { get; set; }

        public bool Activo { get; set; }

        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Término")]
        [DataType(DataType.Date)]
        public DateTime FechaTermino { get; set; }

        [Display(Name = "Fecha Último Acceso")]
        [DataType(DataType.Date)]
        public DateTime FechaUltimoAcceso { get; set; }

        [Display(Name = "Administrador")]
        [ForeignKey("Administradores")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AdministradorID { get; set; }

        [JsonIgnore]
        public virtual Administrador Administrador { get; set; }

        [NotMapped]
        public string NombreCompleto => $"{PrimerApellido} {SegundoApellido} {Nombre}".ToLower();


        public virtual DatoPersonal DatosPersonales { get; set; }
    }
}
