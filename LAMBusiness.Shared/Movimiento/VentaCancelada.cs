namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contacto;
    using Newtonsoft.Json;

    public class VentaCancelada
    {
        [Key]
        [Display(Name = "Venta (Detalle)")]
        public Guid VentaCanceladaID { get; set; }

        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuarios { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Venta Completa")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public bool VentaCompleta { get; set; }
        
    }
}
