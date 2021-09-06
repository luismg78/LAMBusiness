namespace LAMBusiness.Shared.Aplicacion
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("BitacoraExcepciones", Schema = "Aplicacion")]
    public class BitacoraExcepciones
    {
        [Key]
        [ForeignKey("Bitacora")]
        [Display(Name = "Bitacora")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid BitacoraID { get; set; }

        [JsonIgnore]
        public virtual Bitacora Bitacora { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Excepcion { get; set; }

    }
}
