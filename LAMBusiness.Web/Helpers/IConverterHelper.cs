namespace LAMBusiness.Web.Helpers
{
    using System.Threading.Tasks;
    using Models.ViewModels;
    using Shared.Catalogo;

    public interface IConverterHelper
    {
        Task<Producto> ToProductoAsync(ProductoViewModel productoViewModel, bool isNew);
        Task<ProductoViewModel> ToProductosViewModelAsync(Producto producto);
    }
}