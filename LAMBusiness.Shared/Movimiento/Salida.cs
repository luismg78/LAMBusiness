namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Catalogo;

    public class Salida
    {
        [Key]
        [Display(Name = "Salida")]
        public Guid SalidaID { get; set; }

        [Display(Name = "Tipo de salida")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? SalidaTipoID { get; set; }
        
        [ForeignKey("SalidaTipoID")]
        [JsonIgnore]
        public virtual SalidaTipo SalidaTipo { get; set; }

        //[ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioID { get; set; }

        //[JsonIgnore]
        //public virtual Usuario Usuarios { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime? Fecha { get; set; }

        [MaxLength(10, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Folio { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        public bool Aplicado { get; set; }


        [Display(Name = "Fecha Creación")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }


        [Display(Name = "Fecha Última Actualización")]
        [DataType(DataType.Date)]
        public DateTime FechaActualizacion { get; set; }
    }
}
