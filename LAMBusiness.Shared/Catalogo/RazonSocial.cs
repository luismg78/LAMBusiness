using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LAMBusiness.Shared.Catalogo
{
    public class RazonSocial
    {
        [Key]
        public Guid RazonSocialId { get; set; }

        [Display(Name = "Nombre Corto")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string NombreCorto { get; set; }

        [MaxLength(13, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]        
        public string RFC { get; set; }

        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [MaxLength(250, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Domicilio { get; set; }

        [MaxLength(250, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Colonia { get; set; }

        public int? CodigoPostal { get; set; }

        [MaxLength(10, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Telefono { get; set; }

        [MaxLength(250, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Lugar { get; set; }

        [MaxLength(250, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Slogan { get; set; }
    }
}
