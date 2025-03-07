﻿using LAMBusiness.Shared.Catalogo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LAMBusiness.Shared.Kardex
{
    public class KardexDeProducto
    {
        public Guid ProductoID { get; set; }
        public Guid AlmacenID { get; set; }
        public string MovimientoID { get; set; }
        public virtual ICollection<KardexDeProductoDetalle> KardexDeProductoDetalle { get; set; } = null!;
        public virtual Producto Producto { get; set; }
        public virtual IEnumerable<SelectListItem> AlmacenesDDL { get; set; } = null!;
    }
}
