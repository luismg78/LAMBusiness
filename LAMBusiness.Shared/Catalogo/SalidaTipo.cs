namespace LAMBusiness.Shared.Catalogo
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SalidaTipo
    {
        [Key]
        public Guid SalidaTipoID { get; set; }

        [Display(Name = "Tipo de salida")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }
    }
}
