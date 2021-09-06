namespace LAMBusiness.Shared.Aplicacion
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using LAMBusiness.Shared.Contacto;
    using Newtonsoft.Json;

    [Table("Bitacora", Schema = "Aplicacion")]
    public class Bitacora
    {
        [Key]
        public Guid BitacoraID { get; set; }

        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid UsuarioID { get; set; }

        [JsonIgnore]
        public virtual Usuario Usuarios { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0,dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [ForeignKey("Modulo")]
        [Display(Name = "Módulo")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid ModuloID { get; set; }

        [JsonIgnore]
        public virtual Modulo Modulos { get; set; }

        [Display(Name = "Acción")]
        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Accion { get; set; }

        [MaxLength(45, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string Hostname { get; set; }

        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string IPPublica { get; set; }

        [MaxLength(25, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string IPPrivada { get; set; }

        [Display(Name = "Parámetros")]
        public string ParametrosJson { get; set; }
        
        public string ParametroID { get; set; }

        public bool AccionRealizada { get; set; }        
    }
}
