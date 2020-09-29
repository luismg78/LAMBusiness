namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class Unidad
    {
        [Key]
        public Guid UnidadID { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string UnidadNombre { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string UnidadDescripcion { get; set; }

        public bool Pieza { get; set; }

        public bool Paquete { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}