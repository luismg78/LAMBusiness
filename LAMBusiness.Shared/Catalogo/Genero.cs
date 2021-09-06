namespace LAMBusiness.Shared.Catalogo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Generos", Schema = "Catalogo")]
    public class Genero
    {
        [Key]
        [MaxLength(1, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string GeneroID { get; set; }

        [Display(Name = "Género")]
        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string GeneroDescripcion { get; set; }
    }
}
