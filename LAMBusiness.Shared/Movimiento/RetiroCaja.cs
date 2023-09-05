namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Shared.Contacto;

    public class RetiroCaja
    {
        [Key]
        [Display(Name = "Retiro de caja")]
        public Guid RetiroCajaID { get; set; }

        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuarios { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime? Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Importe { get; set; }

        public Guid? VentaCierreID { get; set; }

    }
}
