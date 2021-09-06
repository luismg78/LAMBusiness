﻿namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("EntradasDetalle", Schema = "Movimiento")]
    public class EntradaDetalle
    {
        [Key]
        public Guid EntradaDetalleID { get; set; }

        [ForeignKey("Entrada")]
        [Display(Name = "Entrada")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid EntradaID { get; set; }

        [JsonIgnore]
        public virtual Entrada Entradas { get; set; }

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
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Costo)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? PrecioCosto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Venta)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? PrecioVenta { get; set; }
    }
}
