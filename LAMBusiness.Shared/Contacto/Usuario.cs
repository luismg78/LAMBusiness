namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    public class Usuario
    {
        [Key]
        [Display(Name = "Usuario")]
        public Guid UsuarioID { get; set; }

        [Display(Name = "Colaborador")]
        [ForeignKey("Colaboradores")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ColaboradorID { get; set; }

        [JsonIgnore]
        public virtual Colaborador Colaborador { get; set; }

        public string Password { get; set; }

        public bool Activo { get; set; }

        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Término")]
        [DataType(DataType.Date)]
        public DateTime FechaTermino { get; set; }

        [Display(Name = "Fecha Último Acceso")]
        [DataType(DataType.Date)]
        public DateTime FechaUltimoAcceso { get; set; }

        [Display(Name = "Administrador")]
        [ForeignKey("Administradores")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string AdministradorID { get; set; }

        [JsonIgnore]
        public virtual Administrador Administrador { get; set; }
    }
}
