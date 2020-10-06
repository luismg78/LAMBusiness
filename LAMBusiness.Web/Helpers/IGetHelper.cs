using LAMBusiness.Shared.Catalogo;
using LAMBusiness.Shared.Contacto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAMBusiness.Web.Helpers
{
    public interface IGetHelper
    {
        Task<Cliente> GetClienteByIdAsync(Guid id);
        Task<ClienteContacto> GetContactoClienteByClienteIdAsync(Guid id);
        Task<ClienteContacto> GetContactoClienteByIdAsync(Guid id);
        Task<ProveedorContacto> GetContactoProveedorByIdAsync(Guid id);
        Task<ProveedorContacto> GetContactoProveedorByProveedorIdAsync(Guid id);
        Task<Estado> GetEstadosByIdAsync(short id);
        Task<Municipio> GetMunicipioByIdAsync(int id);
        Task<List<Municipio>> GetMunicipiosByEstadoIdAsync(short id);
        Task<Paquete> GetPaqueteByIdAsync(Guid id);
        Task<Paquete> GetPaqueteByPieceID(Guid id);
        Task<Producto> GetProductByCodeAsync(string codigo);
        Task<Producto> GetProductByIdAsync(Guid id);
        Task<Proveedor> GetProveedorByIdAsync(Guid id);
        Task<Unidad> GetUnidadByIdAsync(Guid id);
    }
}