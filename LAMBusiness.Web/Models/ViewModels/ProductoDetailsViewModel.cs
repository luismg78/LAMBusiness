namespace LAMBusiness.Web.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Shared.Catalogo;

    public class ProductoDetailsViewModel: Producto
    {
        public ICollection<ProductoAsignadoViewModel> ProductosAsignadosViewModel { get; set; }
    }

    public class ProductoAsignadoViewModel
    {
        public Guid ProductoID { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? Cantidad { get; set; }
    }
}
