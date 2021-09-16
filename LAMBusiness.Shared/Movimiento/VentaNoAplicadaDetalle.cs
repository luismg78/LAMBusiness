namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("VentasNoAplicadasDetalle", Schema = "Movimiento")]
    public class VentaNoAplicadaDetalle
    {
        [Key]
        [Display(Name = "Venta (Detalle)")]
        public Guid VentaNoAplicadaDetalleID { get; set; }

        [ForeignKey("VentaNoAplicada")]
        [Display(Name = "Venta (No Aplicada)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaNoAplicadaID { get; set; }

        [JsonIgnore]
        public virtual VentaNoAplicada VentasNoAplicadas { get; set; }

        [ForeignKey("Producto")]
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ProductoID { get; set; }

        [JsonIgnore]
        public virtual Producto Productos { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioVenta { get; set; }
    }
}
