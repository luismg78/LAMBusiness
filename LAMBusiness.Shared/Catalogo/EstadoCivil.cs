namespace LAMBusiness.Shared.Catalogo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("EstadosCiviles", Schema = "Catalogo")]
    public class EstadoCivil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EstadoCivilID { get; set; }

        [Display(Name = "Estado Civil")]
        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string EstadoCivilDescripcion { get; set; }
    }
}
