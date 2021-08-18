namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Contacto;

    public class Entrada
    {
        [Key]
        [Display(Name = "Entrada")]
        public Guid EntradaID { get; set; }

        [ForeignKey("Proveedor")]
        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ProveedorID { get; set; }

        [JsonIgnore]
        public virtual Proveedor Proveedores { get; set; }

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
