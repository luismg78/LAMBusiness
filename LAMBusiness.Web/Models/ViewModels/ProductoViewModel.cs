namespace LAMBusiness.Web.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Shared.Catalogo;

    public class ProductoViewModel: Producto
    {
        [Display(Name = "Código (Pieza)")]
        //[MaxLength(14, ErrorMessage = "La longitud máxima del campo {0} es de {1} caracteres.")]
        public Guid? ProductoIDPieza { get; set; }
        public string CodigoPiezaNombre { get; set; }
        //public string CodigoPieza { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Este campo debe ser un número válido.")]
        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? CantidadProductoxPaquete { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? CantidadProducto { get; set; }

        public IEnumerable<SelectListItem> MarcasDDL { get; set; }

        public IEnumerable<SelectListItem> TasasImpuestosDDL { get; set; }

        public IEnumerable<SelectListItem> UnidadesDDL { get; set; }        
    }
}
