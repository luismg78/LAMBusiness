namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Catalogo;

    public class ProductoViewModel: Producto
    {
        [Display(Name = "Código (Pieza)")]
        [MaxLength(14, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public string CodigoPieza { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? CantidadProductoxPaquete { get; set; }

        public IEnumerable<SelectListItem> TasasImpuestosDDL { get; set; }

        public IEnumerable<SelectListItem> UnidadesDDL { get; set; }        
    }
}
