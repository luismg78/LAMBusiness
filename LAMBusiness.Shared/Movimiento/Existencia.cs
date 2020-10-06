namespace LAMBusiness.Shared.Movimiento
{
    using LAMBusiness.Shared.Catalogo;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Existencia
    {
        public Guid ExistenciaID { get; set; }

        [Display(Name = "Producto")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ProductoID { get; set; }

        [ForeignKey("ProductoID")]
        [JsonIgnore]
        public virtual Producto Productos { get; set; }

        [Display(Name = "Almacén")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? AlmacenID { get; set; }

        [ForeignKey("AlmacenID")]
        [JsonIgnore]
        public virtual Almacen Almacenes { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Existencia Máxima")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? ExistenciaEnAlmacenMaxima { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Existencia Mínima")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? ExistenciaEnAlmacenMinima { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? ExistenciaEnAlmacen { get; set; }
    }
}
