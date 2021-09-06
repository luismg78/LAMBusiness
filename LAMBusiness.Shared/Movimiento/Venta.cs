namespace LAMBusiness.Shared.Movimiento
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;
    using Shared.Contacto;

    [Table("Ventas", Schema = "Movimiento")]
    public class Venta
    {
        [Key]
        [Display(Name = "Venta")]
        public Guid VentaID { get; set; }

        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuarios { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Folio { get; set; }

        [ForeignKey("Almacen")]
        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? AlmacenID { get; set; }

        [JsonIgnore]
        public virtual Almacen Almacenes { get; set; }

        public Guid? ClienteID { get; set; }

        [ForeignKey("FormaPago")]
        [Display(Name = "Forma de pago")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte FormaPagoID { get; set; }

        [JsonIgnore]
        public virtual FormaPago FormasPago { get; set; }

        [ForeignKey("VentaCierre")]
        [Display(Name = "Cierre de ventas")]
        public Guid? VentaCierreID { get; set; }

        [JsonIgnore]
        public virtual VentaCierre VentasCierre { get; set; }

        [Display(Name = "Fecha fin del proceso")]
        [DataType(DataType.Date)]
        public DateTime FechaFinProceso { get; set; }

        public bool Aplicado { get; set; }

    }
}
