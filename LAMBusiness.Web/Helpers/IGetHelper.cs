using LAMBusiness.Shared.Catalogo;
using LAMBusiness.Shared.Contacto;
using LAMBusiness.Shared.Movimiento;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<Entrada> GetEntradaByIdAsync(Guid id);
        Task<List<EntradaDetalle>> GetEntradaDetalleByEntradaIdAsync(Guid id);
        Task<EntradaDetalle> GetEntradaDetalleByIdAsync(Guid id);
        Task<Estado> GetEstadosByIdAsync(short id);
        Task<Existencia> GetExistenciaByIdAsync(Guid id);
        Task<Existencia> GetExistenciaByProductoIdAsync(Guid id);
        Task<Marca> GetMarcaByIdAsync(Guid id);
        Task<Marca> GetMarcaByNombreAsync(string marca);
        Task<List<Marca>> GetMarcasByPatternAsync(string pattern, int skip);
        Task<Municipio> GetMunicipioByIdAsync(int id);
        Task<List<Municipio>> GetMunicipiosByEstadoIdAsync(short id);
        Task<Paquete> GetPaqueteByIdAsync(Guid id);
        Task<Paquete> GetPaqueteByPieceID(Guid id);
        Task<Producto> GetProductByCodeAsync(string codigo);
        Task<Producto> GetProductByIdAsync(Guid id);
        Task<List<Producto>> GetProductosByPatternAsync(string pattern, int skip);
        Task<Proveedor> GetProveedorByIdAsync(Guid id);
        Task<Proveedor> GetProveedorByRFCAsync(string rfc);
        Task<List<Proveedor>> GetProveedoresByPatternAsync(string pattern, int skip);
        Task<Unidad> GetUnidadByIdAsync(Guid id);
    }
}