namespace LAMBusiness.Web.Helpers
{
    using System.Threading.Tasks;
    using LAMBusiness.Shared.Aplicacion;
    using LAMBusiness.Shared.Contacto;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels;
    using Shared.Catalogo;

    public interface IConverterHelper
    {
        Task<Cliente> ToClienteAsync(ClienteViewModel clienteViewModel, bool isNew);
        Task<ClienteViewModel> ToClienteViewModelAsync(Cliente cliente);
        Task<Resultado<Colaborador>> ToColaboradorAsync(ColaboradorViewModel colaboradorViewModel, bool isNew);
        Task<ColaboradorViewModel> ToColaboradorViewModelAsync(Colaborador colaborador);
        Task<Entrada> ToEntradaAsync(EntradaViewModel entradaViewModel, bool isNew);
        Task<EntradaViewModel> ToEntradaViewModelAsync(Entrada entrada);
        FileContentResult ToImageBase64(string path);
        Modulo ToModulo(ModuloViewModel moduloViewModel, bool isNew);
        Task<ModuloViewModel> ToModuloViewModelAsync(Modulo modulo);
        Task<Producto> ToProductoAsync(ProductoViewModel productoViewModel, bool isNew);
        Task<ProductoDetailsViewModel> ToProductosDetailsViewModelAsync(Producto producto);
        Task<ProductoViewModel> ToProductosViewModelAsync(Producto producto);
        Task<Proveedor> ToProveedorAsync(ProveedorViewModel proveedorViewModel, bool isNew);
        Task<ProveedorViewModel> ToProveedorViewModelAsync(Proveedor proveedor);
        Task<Salida> ToSalidaAsync(SalidaViewModel salidaViewModel, bool isNew);
        Task<SalidaViewModel> ToSalidaViewModelAsync(Salida salida);
        Task<Usuario> ToUsuarioAsync(UsuarioViewModel usuarioViewModel, bool isNew);
        byte[] UploadImageBase64(string file, int size);
    }
}