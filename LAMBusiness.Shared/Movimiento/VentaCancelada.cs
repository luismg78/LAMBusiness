namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("VentasCanceladas", Schema = "Movimiento")]
    public class VentaCancelada
    {
        [Key]
        [Display(Name = "Venta (Detalle)")]
        public Guid VentaCanceladaID { get; set; }

        [Display(Name = "Venta (No Aplicada)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid VentaID { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ProductoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal PrecioVenta { get; set; }
    }
}
