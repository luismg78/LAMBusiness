namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Contacto;
    using Newtonsoft.Json;

    public class VentaImporte
    {
        [Key]
        [Display(Name = "Venta (Importe)")]
        public Guid VentaImporteID { get; set; }

        [ForeignKey("Venta")]
        [Display(Name = "Venta")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaID { get; set; }

        [JsonIgnore]
        public virtual Venta Ventas { get; set; }

        [ForeignKey("FormaPago")]
        [Display(Name = "Forma de pago")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte FormaPagoID { get; set; }

        [JsonIgnore]
        public virtual FormaPago FormasPago { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Importe { get; set; }
    }
}
