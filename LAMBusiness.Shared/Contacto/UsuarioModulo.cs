namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Aplicacion;
    using Newtonsoft.Json;

    [Table("UsuariosModulos", Schema = "Contacto")]
    public class UsuarioModulo
    {
        [Key]
        public Guid UsuarioModuloID { get; set; }
        
        [Display(Name = "Usuario")]
        [ForeignKey("Usuarios")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Modulo")]
        [ForeignKey("Modulos")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ModuloID { get; set; }

        [JsonIgnore]
        public virtual Modulo Modulo { get; set; }
        
        public bool PermisoLectura { get; set; }

        public bool PermisoEscritura { get; set; }

        public bool PermisoImprimir { get; set; }
    }
}
