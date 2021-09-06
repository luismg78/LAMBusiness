namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Contacto;

    public class ProveedorViewModel: Proveedor
    {
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccionar un registro del campo {0}.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public short? EstadoID { get; set; }

        public bool PermisoEscritura { get; set; }

        public IEnumerable<SelectListItem> EstadosDDL { get; set; }

        public IEnumerable<SelectListItem> MunicipiosDDL { get; set; }
    }
}
