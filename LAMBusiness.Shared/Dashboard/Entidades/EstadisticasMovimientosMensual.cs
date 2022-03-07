namespace LAMBusiness.Shared.Dashboard.Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Shared.Catalogo;
    using Newtonsoft.Json;

    [Table("EstadisticasMovimientosMensual", Schema = "Dashboard")]
    public class EstadisticasMovimientosMensual
    {
        [Key]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid EstadisticaMovimientoMensualID { get; set; }

        [ForeignKey("Almacen")]
        [Display(Name = "Almacen")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid AlmacenID { get; set; }

        [JsonIgnore]
        public virtual Almacen Almacen { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short Año { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte Mes { get; set; }

        public int? Ventas { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VentasImporte { get; set; }

        public int? Entradas { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EntradasImporte { get; set; }

        public int? Salidas { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalidasImporte { get; set; }

        public int? Devoluciones { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DevolucionesImporte { get; set; }
    }
}
