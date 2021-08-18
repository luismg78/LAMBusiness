namespace LAMBusiness.Shared.Aplicacion
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Modulo
    {
        [Key]
        [Display(Name = "Módulo")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ModuloID { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public bool Activo { get; set; }

        [Display(Name = "Módulo (Padre)")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ModuloPadreID { get; set; }
    }
}
