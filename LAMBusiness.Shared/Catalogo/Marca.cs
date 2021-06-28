namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class Marca
    {
        [Key]
        public Guid MarcaID { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string MarcaNombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string MarcaDescripcion { get; set; }
    }
}
