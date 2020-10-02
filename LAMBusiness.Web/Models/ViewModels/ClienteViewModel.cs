namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Contacto;

    public class ClienteViewModel:Cliente
    {
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que {1}.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short? EstadoID { get; set; }

        public IEnumerable<SelectListItem> EstadosDDL { get; set; }

        public IEnumerable<SelectListItem> MunicipiosDDL { get; set; }
    }
}
