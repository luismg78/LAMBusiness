namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    public class InventarioDetalle
    {
        [Key]
        public Guid InventarioDetalleID { get; set; }

        [ForeignKey("Inventario")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid InventarioID { get; set; }

        [JsonIgnore]
        public virtual Inventario Inventarios { get; set; }

        [ForeignKey("Producto")]
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ProductoID { get; set; }

        [JsonIgnore]
        public virtual Producto Productos { get; set; }

        [ForeignKey("Almacen")]
        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? AlmacenID { get; set; }

        [JsonIgnore]
        public virtual Almacen Almacenes { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad en Sistema")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? CantidadEnSistema { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad Inventariada")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? CantidadInventariada { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad Faltante")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? CantidadFaltante { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Costo)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? PrecioCosto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Importe")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? Importe { get; set; }
    }
}
