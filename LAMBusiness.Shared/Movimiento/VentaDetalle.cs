namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("VentasDetalle", Schema = "Movimiento")]
    public class VentaDetalle
    {
        [Key]
        [Display(Name = "Venta (Detalle)")]
        public Guid VentaDetalleID { get; set; }

        [ForeignKey("Venta")]
        [Display(Name = "Venta")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaID { get; set; }

        [JsonIgnore]
        public virtual Venta Ventas { get; set; }

        [ForeignKey("Producto")]
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ProductoID { get; set; }

        [JsonIgnore]
        public virtual Producto Productos { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Costo)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioCosto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Venta)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioVenta { get; set; }

    }
}
