using LAMBusiness.Shared.Catalogo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LAMBusiness.Web.Models.ViewModels
{
    public class InventarioDetalleViewModel
    {
        public Guid InventarioDetalleID { get; set; }
        public Guid InventarioID { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? ProductoID { get; set; }
        public virtual Producto Productos { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Guid? AlmacenID { get; set; }
        public virtual Almacen Almacenes { get; set; }

        [Display(Name = "Cantidad Inventariada")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? Cantidad { get; set; } = 0;

        [Display(Name = "Ingrese las piezas individuales correspondientes al paquete.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal? CantidadEnPiezas { get; set; } = 0;
        
        public bool EsPaquete { get; set; }

        public virtual IEnumerable<SelectListItem> AlmacenesDDL { get; set; }

    }
}
