namespace LAMBusiness.Shared.Catalogo
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class FormaPago
    {
        [Key]
        public byte FormaPagoID { get; set; }

        [Display(Name = "Forma de pago")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Predeterminada")]
        public bool ValorPorDefault { get; set; }

        [Display(Name = "Porcentaje De Cobro Extra")]
        [Range(0, 100, ErrorMessage = "Porcentaje incorrecto.")]
        public int? PorcentajeDeCobroExtra { get; set; } = 0;

        [Display(Name = "Texto Por Cobro Extra")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string TextoPorCobroExtra { get; set; } = "";
    }
}