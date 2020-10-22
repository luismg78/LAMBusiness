namespace LAMBusiness.Web.Helpers
{
    using System.Threading.Tasks;
    using LAMBusiness.Shared.Contacto;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels;
    using Shared.Catalogo;

    public interface IConverterHelper
    {
        Task<Cliente> ToClienteAsync(ClienteViewModel clienteViewModel, bool isNew);
        Task<ClienteViewModel> ToClienteViewModelAsync(Cliente cliente);
        Task<Entrada> ToEntradaAsync(EntradaViewModel entradaViewModel, bool isNew);
        Task<EntradaViewModel> ToEntradaViewModelAsync(Entrada entrada);
        FileContentResult ToImageBase64(string path);
        Task<Producto> ToProductoAsync(ProductoViewModel productoViewModel, bool isNew);
        Task<ProductoDetailsViewModel> ToProductosDetailsViewModelAsync(Producto producto);
        Task<ProductoViewModel> ToProductosViewModelAsync(Producto producto);
        Task<Proveedor> ToProveedorAsync(ProveedorViewModel proveedorViewModel, bool isNew);
        Task<ProveedorViewModel> ToProveedorViewModelAsync(Proveedor proveedor);
        byte[] UploadImageBase64(string file, int size);
    }
}