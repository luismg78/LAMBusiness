namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    public class Colaborador
    {
        [Key]
        public Guid ColaboradorID { get; set; }

        [RegularExpression(@"^([a-zA-Z][AEIOUXaeioux][a-zA-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HMhm](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS|as|b[cs]|c[clmsh]|d[fg]|g[tr]|hg|jc|m[cns]|n[etl]|oc|pl|q[tr]|s[plr]|t[csl]|vz|yn|zs)(?:[B-DF-HJ-NP-TV-Z]|[b-df-hj-np-tv-z]){3}[a-zA-Z\d])(\d)$", ErrorMessage = "Formato Incorrecto.")]
        [MaxLength(18, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string CURP { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PrimerApellido { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [ForeignKey("Puesto")]
        [Display(Name = "Puesto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid PuestoID { get; set; }

        [JsonIgnore]
        public virtual Puesto Puestos { get; set; }

        [ForeignKey("Generos")]
        [ScaffoldColumn(false)]
        public string GeneroID { get; set; }

        [JsonIgnore]
        public virtual Genero Generos { get; set; }

        [ForeignKey("Estados")]
        [ScaffoldColumn(false)]
        public short EstadoNacimientoID { get; set; }

        [JsonIgnore]
        public virtual Estado Estados { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("EstadoCivil")]
        [Display(Name = "Estado Civil")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short EstadoCivilID { get; set; }
        [JsonIgnore]
        public virtual EstadoCivil EstadosCiviles { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Domicilio { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Colonia { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int CodigoPostal { get; set; }

        [ForeignKey("Municipios")]
        [Display(Name = "Municipio")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int MunicipioID { get; set; }

        [JsonIgnore]
        public virtual Municipio Municipios { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono (Fijo)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto.")]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono (Móvil)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto.")]
        public string TelefonoMovil { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0,dd/MM/yyyy}")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; }
    }
}