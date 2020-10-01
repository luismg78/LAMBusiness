﻿using LAMBusiness.Shared.Catalogo;
using System;
using System.Threading.Tasks;

namespace LAMBusiness.Web.Helpers
{
    public interface IGetHelper
    {
        Task<Paquete> GetPaqueteByIdAsync(Guid id);
        Task<Paquete> GetPaqueteByPieceID(Guid id);
        Task<Producto> GetProductByCodeAsync(string codigo);
        Task<Producto> GetProductByIdAsync(Guid id);
        Task<Unidad> GetUnidadByIdAsync(Guid id);
    }
}