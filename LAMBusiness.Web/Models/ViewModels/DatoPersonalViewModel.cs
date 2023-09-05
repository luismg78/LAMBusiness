namespace LAMBusiness.Web.Models.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Contacto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DatoPersonalViewModel : DatoPersonal
    {
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccionar un registro del campo {0}.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short? EstadoID { get; set; }

        public IEnumerable<SelectListItem> EstadosDDL { get; set; }
        public IEnumerable<SelectListItem> EstadosNacimientoDDL { get; set; }
        public IEnumerable<SelectListItem> EstadosCivilesDDL { get; set; }
        public IEnumerable<SelectListItem> GenerosDDL { get; set; }
        public IEnumerable<SelectListItem> MunicipiosDDL { get; set; }
        public IEnumerable<SelectListItem> PuestosDDL { get; set; }
    }
}
