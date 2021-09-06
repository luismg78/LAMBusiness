namespace LAMBusiness.Shared.Catalogo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("FormasPago", Schema = "Catalogo")]
    public class FormaPago
    {
        [Key]
        public byte FormaPagoID { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string FormaPagoDescripcion { get; set; }

        [Display(Name = "Predeterminada")]
        public bool FormaPagoDefault { get; set; }
    }
}