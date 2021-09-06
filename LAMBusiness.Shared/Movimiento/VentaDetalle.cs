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

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioCosto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioVenta { get; set; }

    }
}
