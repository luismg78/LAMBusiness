using LAMBusiness.Shared.Aplicacion;
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
        Task<Administrador> GetAdministradorByIdAsync(string id);
        Task<Almacen> GetAlmacenByIdAsync(Guid id);
        Task<Almacen> GetAlmacenByNombreAsync(string almacen);
        Task<List<Almacen>> GetAlmacenesByPatternAsync(string pattern, int skip);
        Task<Cliente> GetClienteByIdAsync(Guid id);
        Task<Colaborador> GetColaboradorByCURPAsync(string curp);
        Task<Colaborador> GetColaboradorByIdAsync(Guid id);
        Task<List<Colaborador>> GetColaboradoresByPatternAsync(string pattern, int skip);
        Task<List<Colaborador>> GetColaboradoresSinCuentaUsuarioByPatternAsync(string pattern, int skip);
        Task<ClienteContacto> GetContactoClienteByClienteIdAsync(Guid id);
        Task<ClienteContacto> GetContactoClienteByIdAsync(Guid id);
        Task<ProveedorContacto> GetContactoProveedorByIdAsync(Guid id);
        Task<ProveedorContacto> GetContactoProveedorByProveedorIdAsync(Guid id);
        Task<Entrada> GetEntradaByIdAsync(Guid id);
        Task<List<EntradaDetalle>> GetEntradaDetalleByEntradaIdAsync(Guid id);
        Task<EntradaDetalle> GetEntradaDetalleByIdAsync(Guid id);
        Task<Estado> GetEstadosByIdAsync(short id);
        Task<Existencia> GetExistenciaByIdAsync(Guid id);
        Task<Existencia> GetExistenciaByProductoIdAndAlmacenIdAsync(Guid productoId, Guid almacenId);
        Task<decimal> GetExistenciaByProductoIdAsync(Guid productoId);
        Task<Marca> GetMarcaByIdAsync(Guid id);
        Task<Marca> GetMarcaByNombreAsync(string marca);
        Task<List<Marca>> GetMarcasByPatternAsync(string pattern, int skip);
        Task<List<Guid>> GetModulesByUsuarioIDAndModuloPadreID(Guid usuarioId, Guid moduloPadreId);
        Task<Modulo> GetModuloByIdAsync(Guid id);
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
        Task<Salida> GetSalidaByIdAsync(Guid id);
        Task<SalidaDetalle> GetSalidaDetalleByIdAsync(Guid id);
        Task<List<SalidaDetalle>> GetSalidaDetalleBySalidaIdAsync(Guid id);
        Task<SalidaTipo> GetSalidaTipoByIdAsync(Guid id);
        Task<Resultado<Token>> GetTokenBySessionIdAndUsuarioIDAsync(string sessionId, string directorio);
        Task<Unidad> GetUnidadByIdAsync(Guid id);
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task<UsuarioModulo> GetUsuarioModuloByUsuarioAndModuloIDAsync(Guid usuarioId, Guid moduloId);
        Task<Resultado<Token>> SetTokenByUsuarioIDAsync(string sessionId, Guid usuarioId, string directorio);
    }
}