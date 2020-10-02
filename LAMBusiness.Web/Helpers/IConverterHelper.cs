namespace LAMBusiness.Web.Helpers
{
    using System.Threading.Tasks;
    using LAMBusiness.Shared.Contacto;
    using Models.ViewModels;
    using Shared.Catalogo;

    public interface IConverterHelper
    {
        Task<Cliente> ToClienteAsync(ClienteViewModel clienteViewModel, bool isNew);
        Task<ClienteViewModel> ToClienteViewModelAsync(Cliente cliente);
        Task<Producto> ToProductoAsync(ProductoViewModel productoViewModel, bool isNew);
        Task<ProductoDetailsViewModel> ToProductosDetailsViewModelAsync(Producto producto);
        Task<ProductoViewModel> ToProductosViewModelAsync(Producto producto);
    }
}