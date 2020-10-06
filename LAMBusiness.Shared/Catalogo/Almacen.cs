namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Contacto;
    using LAMBusiness.Shared.Movimiento;
    using Newtonsoft.Json;

    public class Almacen
    {
        [Key]
        public Guid AlmacenID { get; set; }

        [Display(Name = "Almacén")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AlmacenNombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AlmacenDescripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Existencia> Existencias { get; set; }
    }
}