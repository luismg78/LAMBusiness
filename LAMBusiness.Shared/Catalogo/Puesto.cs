namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contacto;
    using Newtonsoft.Json;

    [Table("Puestos", Schema = "Catalogo")]
    public class Puesto
    {
        [Key]
        public Guid PuestoID { get; set; }

        [Display(Name = "Puesto")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PuestoNombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PuestoDescripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Colaborador> Colaboradores { get; set; }
    }
}