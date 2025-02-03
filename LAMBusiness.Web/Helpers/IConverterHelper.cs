namespace LAMBusiness.Web.Helpers
{
    using LAMBusiness.Shared.Aplicacion;
    using LAMBusiness.Shared.Contacto;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels;
    using Shared.Catalogo;
    using System.Threading.Tasks;

    public interface IConverterHelper
    {
        //FileContentResult GenerateBarcode(string _data, Type t);
        Task<Cliente> ToClienteAsync(ClienteViewModel clienteViewModel, bool isNew);
        Task<ClienteViewModel> ToClienteViewModelAsync(Cliente cliente);
        Task<Resultado<DatoPersonal>> ToDatoPersonalAsync(DatoPersonalViewModel datoPersonalViewModel, bool isNew);
        Task<DatoPersonalViewModel> ToDatoPersonalViewModelAsync(DatoPersonal datoPersonal);
        Task<Entrada> ToEntradaAsync(EntradaViewModel entradaViewModel, bool isNew);
        Task<EntradaViewModel> ToEntradaViewModelAsync(Entrada entrada);
        Inventario ToInventarioAsync(InventarioViewModel entradaViewModel, bool isNew);
        Task<InventarioViewModel> ToInventarioViewModelAsync(Inventario entrada);
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
		Venta ToVenta(VentasViewModel entradaViewModel, bool isNew);
		VentasViewModel ToVentaViewModel(Venta entrada);
		byte[] UploadImageBase64(string file, int size);
    }
}