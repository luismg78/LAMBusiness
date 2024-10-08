﻿namespace LAMBusiness.Shared.Contacto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Catalogo;
    using Newtonsoft.Json;

    public class Cliente
    {
        [Key]
        [Display(Name = "Cliente")]
        public Guid ClienteID { get; set; }

        [MaxLength(13, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$", ErrorMessage = "Formato Incorrecto.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string RFC { get; set; }

        [MaxLength(75, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

       [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Domicilio { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Colonia { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int? CodigoPostal { get; set; }

        [Display(Name = "Municipio")]
        [ForeignKey("Municipios")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccionar un registro del campo {0}.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int? MunicipioID { get; set; }

        [JsonIgnore]
        public virtual Municipio Municipios { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono (Fijo)")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Formato Incorrecto.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del Correo Electrónico es incorrecto.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; }

        public ICollection<ClienteContacto> ClienteContactos { get; set; }
    }
}