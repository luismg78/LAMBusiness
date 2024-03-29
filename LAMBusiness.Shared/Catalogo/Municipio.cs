﻿namespace LAMBusiness.Shared.Catalogo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class Municipio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MunicipioID { get; set; }

        [ForeignKey("Estado")]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short EstadoID { get; set; }
        
        [JsonIgnore]
        public virtual Estado Estados { get; set; }

        [Display(Name = "Clave")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short Clave { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Nombre { get; set; }        
    }
}
