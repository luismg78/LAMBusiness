namespace LAMBusiness.Shared.Catalogo
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Estados", Schema = "Catalogo")]
    public class Estado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EstadoID { get; set; }

        [Display(Name = "Clave")]
        [MaxLength(5, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string EstadoClave { get; set; }

        [Display(Name = "Estado")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string EstadoDescripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Municipio> Municipios { get; set; }
    }
}
