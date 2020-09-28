namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class Paquete
    {
        [Key]
        public Guid PaqueteID { get; set; }

        [Display(Name = "Código (Paquete)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid PaqueteProductoID { get; set; }

        [ForeignKey("PaqueteProductoID")]
        [JsonIgnore]
        public virtual Producto PaqueteProductos { get; set; }

        [Display(Name = "Código (Pieza)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid PiezaProductoID { get; set; }

        [ForeignKey("PiezaProductoID")]
        [JsonIgnore]
        public virtual Producto PiezaProducto { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "${0:N2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal CantidadProductoxPaquete { get; set; }
    }
}