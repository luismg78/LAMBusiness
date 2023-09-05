namespace LAMBusiness.Shared.Catalogo
{
    using Contacto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Puesto
    {
        [Key]
        public Guid PuestoID { get; set; }

        [Display(Name = "Puesto")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Descripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<DatoPersonal> Colaboradores { get; set; }
    }
}