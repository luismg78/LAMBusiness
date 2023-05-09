namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("VentasCierreDetalle", Schema = "Movimiento")]
    public class VentaCierreDetalle
    {
        [Key]
        [Display(Name = "Venta (Detalle del cierre)")]
        public Guid VentaCierreDetalleID { get; set; }

        [ForeignKey("VentaCierre")]
        [Display(Name = "Venta (Cierre)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaCierreID { get; set; }

        [JsonIgnore]
        public virtual VentaCierre VentasCierre { get; set; }

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
