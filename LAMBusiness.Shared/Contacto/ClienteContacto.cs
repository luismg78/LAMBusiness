namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class ClienteContacto
    {
        [Key]
        public Guid ClienteContactoID { get; set; }

        [ForeignKey("Cliente")]
        public Guid ClienteID { get; set; }

        [JsonIgnore]
        public virtual Cliente Clientes { get; set; }

        [Column(name: "Nombre")]
        [Display(Name = "Nombre")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string NombreContacto { get; set; }

        [Column(name: "PrimerApellido")]
        [Display(Name = "Primer Apellido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PrimerApellidoContacto { get; set; }

        [Column(name: "SegundoApellido")]
        [Display(Name = "Segundo Apellido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string SegundoApellidoContacto { get; set; }

        [Column(name: "TelefonoMovil")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono (Móvil)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto.")]
        public string TelefonoMovilContacto { get; set; }

        [Column(name: "Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string EmailContacto { get; set; }
    }
}
