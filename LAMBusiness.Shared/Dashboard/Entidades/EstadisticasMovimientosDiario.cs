namespace LAMBusiness.Shared.Dashboard.Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class EstadisticasMovimientosDiario
    {
        [Key]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid EstadisticaMovimientoDiarioID { get; set; }

        [ForeignKey("EstadisticasMovimientosMensual")]
        [Display(Name = "EstadisticasMovimientosMensual")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid EstadisticaMovimientoMensualID { get; set; }

        [JsonIgnore]
        public virtual EstadisticasMovimientosMensual EstadisticasMovimientosMensual { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime Fecha { get; set; }
        
        public string Dia { get; set; }

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
