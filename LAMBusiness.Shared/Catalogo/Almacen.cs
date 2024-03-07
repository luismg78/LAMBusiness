namespace LAMBusiness.Shared.Catalogo
{
    using Movimiento;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Almacen
    {
        [Key]
        public Guid AlmacenID { get; set; }

        [Display(Name = "Almacén")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Descripcion { get; set; }

        public string DescripcionResumida
        {
            get
            {
                if (Descripcion != null && Descripcion.Length > 50)
                {
                    return $"{Descripcion[..100]}...";
                }
                return Descripcion;
            }
        }

        [JsonIgnore]
        public virtual ICollection<Existencia> Existencias { get; set; }
    }
}