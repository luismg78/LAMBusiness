namespace LAMBusiness.Shared.Catalogo
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TasaImpuesto
    {
        [Key]
        [JsonIgnore]
        public Guid TasaID { get; set; }

        [Display(Name = "Tasa (Impuesto)")]
        [MaxLength(45, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(0, 100, ErrorMessage = "Valor incorrecto, rango permitido de {1} a {2}.")]
        public short? Porcentaje { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}