namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Newtonsoft.Json;

    public class SalidaDetalle
    {
        [Key]
        public Guid SalidaDetalleID { get; set; }

        [ForeignKey("Salida")]
        [Display(Name = "Salida")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid SalidaID { get; set; }

        [JsonIgnore]
        public virtual Salida Salidas { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio (Costo)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? PrecioCosto { get; set; }

        [NotMapped]
        public virtual IEnumerable<SelectListItem> AlmacenesDDL { get; set; }

    }
}
