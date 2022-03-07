namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("VentasCanceladasDetalle", Schema = "Movimiento")]
    public class VentaCanceladaDetalle
    {
        [Key]
        [Display(Name = "Venta Cancelada (Detalle)")]
        public Guid VentaCanceladaDetalleID { get; set; }

        [ForeignKey("VentaCancelada")]
        [Display(Name = "VentaCancelada")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaCanceladaID { get; set; }

        [JsonIgnore]
        public virtual VentaCancelada VentasCanceladas { get; set; }

        [ForeignKey("Producto")]
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ProductoID { get; set; }

        [JsonIgnore]
        public virtual Producto Productos { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PrecioVenta { get; set; }

    }
}
