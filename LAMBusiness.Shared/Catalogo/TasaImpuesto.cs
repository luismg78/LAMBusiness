namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class TasaImpuesto
    {
        [Key]
        [JsonIgnore]
        public Guid TasaID { get; set; }

        [Display(Name = "Tasa (Impuesto)")]
        [MaxLength(45, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Tasa { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short Porcentaje { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string TasaDescripcion { get; set; }
    }
}