namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Movimiento;
    using Newtonsoft.Json;

    public class Producto
    {
        [Key]
        public Guid ProductoID { get; set; }

        [Display(Name = "Código")]
        [MaxLength(14, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Codigo { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string ProductoNombre { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string ProductoDescripcion { get; set; }

        [Display(Name = "Unidad")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? UnidadID { get; set; }

        [ForeignKey("UnidadID")]
        [JsonIgnore]
        public virtual Unidad Unidades { get; set; }

        [Display(Name = "Tasa (Impuesto)")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? TasaID { get; set; }

        [ForeignKey("TasaID")]
        [JsonIgnore]
        public virtual TasaImpuesto TasasImpuestos { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Precio (Costo)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? PrecioCosto { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Precio (Venta)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? PrecioVenta { get; set; }

        public bool Activo { get; set; }

        public virtual Paquete Paquete { get; set; }

        public virtual ICollection<Existencia> Existencias { get; set; }

    }
}