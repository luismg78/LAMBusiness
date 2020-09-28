namespace LAMBusiness.Web.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Modulo
    {
        [Key]
        public Guid ModuloID { get; set; }

        [Display(Name = "Módulo")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")] 
        public string Descripcion { get; set; }
        
        public bool Activo { get; set; }
    }
}
