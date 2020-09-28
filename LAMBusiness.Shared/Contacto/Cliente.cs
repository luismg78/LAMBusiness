namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    public class Cliente
    {
        [Key]
        public Guid ClienteID { get; set; }

        [MaxLength(13, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^([a-zA-Z][AEIOUXaeioux][a-zA-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[a-zA-Z\d]{3})$", ErrorMessage = "Formato Incorrecto.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string RFC { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Contacto { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Domicilio { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Colonia { get; set; }

        [Display(Name = "Codigo Postal")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int CodigoPostal { get; set; }

        [Display(Name = "Municipio")]
        [ForeignKey("Municipios")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int MunicipioID { get; set; }

        [JsonIgnore]
        public virtual Municipio Municipios { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono (Fijo)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto")]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono (Móvil)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto")]
        public string TelefonoMovil { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0,dd/MM/yyyy}")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; }
    }
}