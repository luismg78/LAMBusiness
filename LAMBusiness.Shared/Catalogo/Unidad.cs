namespace LAMBusiness.Shared.Catalogo
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Unidad
    {
        [Key]
        public Guid UnidadID { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(150, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Descripcion { get; set; }

        public bool Pieza { get; set; }

        public bool Paquete { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}