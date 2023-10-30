namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;

    public interface IGetHelper
    {
        Task<Administrador> GetAdministradorByIdAsync(string id);
        Task<Almacen> GetAlmacenByIdAsync(Guid id);
        Task<Almacen> GetAlmacenByNombreAsync(string almacen);
        Task<Filtro<List<Almacen>>> GetAlmacenesByPatternAsync(Filtro<List<Almacen>> filtro);
        Task<Cliente> GetClienteByIdAsync(Guid id);
        Task<DatoPersonal> GetDatosPersonalesByCURPAsync(string curp);
        Task<DatoPersonal> GetDatosPersonalesByIdAsync(Guid id);
        Task<List<Usuario>> GetUsuariosByPatternAsync(string pattern, int skip);
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
        Task<EstadisticaMovimientoChartViewModel> GetMovementsDashboardAsync(List<int> años);
        Task<Municipio> GetMunicipioByIdAsync(int id);
        Task<List<Municipio>> GetMunicipiosByEstadoIdAsync(short id);
        Task<Paquete> GetPaqueteByIdAsync(Guid id);
        Task<Paquete> GetPaqueteByPieceID(Guid id);
        Task<Proveedor> GetProveedorByIdAsync(Guid id);
        Task<Proveedor> GetProveedorByRFCAsync(string rfc);
        Task<Filtro<List<Proveedor>>> GetProveedoresByPatternAsync(Filtro<List<Proveedor>> filtro);
        Task<Salida> GetSalidaByIdAsync(Guid id);
        Task<SalidaDetalle> GetSalidaDetalleByIdAsync(Guid id);
        Task<List<SalidaDetalle>> GetSalidaDetalleBySalidaIdAsync(Guid id);
        Task<SalidaTipo> GetSalidaTipoByIdAsync(Guid id);
        Task<Resultado<Token>> GetTokenBySessionIdAndUsuarioIDAsync(string sessionId, string directorio);
        Task<Unidad> GetUnidadByIdAsync(Guid id);
        Task<Usuario> GetUsuarioByIdAsync(Guid id);
        Task<UsuarioModulo> GetUsuarioModuloByUsuarioAndModuloIDAsync(Guid usuarioId, Guid moduloId);
        Task SetBitacoraAsync(Token token, string accion, Guid moduloId, object clase, string parametroId, string directorio, string excepcion = "");
        Task<Resultado<Token>> SetTokenByUsuarioIDAsync(string sessionId, Guid usuarioId, string directorio);
    }
}