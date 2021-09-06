namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Shared.Contacto;

    [Table("VentasCierre", Schema = "Movimiento")]
    public class VentaCierre
    {
        [Key]
        [Display(Name = "Venta")]
        public Guid VentaCierreID { get; set; }

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

        [ForeignKey("UsuarioCaja")]
        [Display(Name = "Usuario (Caja)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioCajaID { get; set; }

        [JsonIgnore]
        public virtual Usuario UsuarioCaja { get; set; }

        public decimal ImporteSistema { get; set; }
        
        public decimal ImporteUsuario { get; set; }

    }
}
