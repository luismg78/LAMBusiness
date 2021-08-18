namespace LAMBusiness.Shared.Catalogo
{
    using System.ComponentModel.DataAnnotations;

    public class Administrador
    {
        [Key]
        [Display(Name = "Administrador")]
        [MaxLength(2, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")] 
        public string AdministradorID { get; set; }

        [Display(Name = "Administrador (Tipo)")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AdministradorNombre { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AdministradorDescripcion { get; set; }
    }
}
