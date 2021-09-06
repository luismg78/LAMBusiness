namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    [Table("Existencias", Schema = "Movimiento")]
    public class Existencia
    {
        public Guid ExistenciaID { get; set; }

        [Display(Name = "Producto")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ProductoID { get; set; }

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
        [Display(Name = "Existencia")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal ExistenciaEnAlmacen { get; set; }
    }
}
