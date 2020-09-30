namespace LAMBusiness.Web.Helpers
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels;
    using Shared.Catalogo;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context,
            IGetHelper getHelper,
            ICombosHelper combosHelper)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
        }

        /// <summary>
        /// Convertir clase productoViewModel a producto (incluye clase paquete)
        /// </summary>
        /// <param name="productoViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Producto(class)</returns>
        public async Task<Producto> ToProductoAsync(ProductoViewModel productoViewModel, bool isNew)
        {
            var producto = new Producto()
            {
                Activo = productoViewModel.Activo,
                Codigo = productoViewModel.Codigo.Trim().ToUpper(),
                Existencia = isNew ? 0 : productoViewModel.Existencia,
                ExistenciaMaxima = productoViewModel.ExistenciaMaxima,
                PrecioCosto = productoViewModel.PrecioCosto,
                PrecioVenta = productoViewModel.PrecioVenta,
                ProductoDescripcion = productoViewModel.ProductoDescripcion.Trim().ToUpper(),
                ProductoID = isNew ? Guid.NewGuid() : productoViewModel.ProductoID,
                ProductoNombre = productoViewModel.ProductoNombre.Trim().ToUpper(),
                TasaID = productoViewModel.TasaID,
                UnidadID = productoViewModel.UnidadID,
                Paquete = null
            };

            if (productoViewModel.UnidadID == Guid.Parse("6C9C7801-D654-11E9-8B00-8CDCD47D68A1") ||
                productoViewModel.UnidadID == Guid.Parse("95B850EC-D654-11E9-8B00-8CDCD47D68A1"))
            {
                var productopiezaid = await _getHelper.GetProductoIDAsync(productoViewModel.CodigoPieza);
                var paquete = new Paquete()
                {
                    CantidadProductoxPaquete = (decimal)productoViewModel.CantidadProductoxPaquete,
                    ProductoID = producto.ProductoID,
                    PiezaProductoID = productopiezaid
                };
                producto.Paquete = paquete;
            }

            return producto;

        }

        /// <summary>
        /// Convertir clase producto a productoViewModel (incluye clase paquete)
        /// </summary>
        /// <param name="producto"></param>
        /// <returns>ProductViewModel(class)</returns>
        public async Task<ProductoViewModel> ToProductosViewModelAsync(Producto producto)
        {

            var productoViewModel = new ProductoViewModel()
            {
                Activo = producto.Activo,
                Codigo = producto.Codigo,
                Existencia = producto.Existencia,
                ExistenciaMaxima = producto.ExistenciaMaxima,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                ProductoDescripcion = producto.ProductoDescripcion,
                ProductoID = producto.ProductoID,
                ProductoNombre = producto.ProductoNombre,
                TasaID = producto.TasaID,
                TasasImpuestos = producto.TasasImpuestos,
                TasasImpuestosDDL = await _combosHelper.GetComboTasaImpuestosAsync(),
                Unidades = producto.Unidades,
                UnidadesDDL = await _combosHelper.GetComboUnidadesAsync(),
                UnidadID = producto.UnidadID,
                //Paquete
                CantidadProductoxPaquete = 0,
                CodigoPieza = ""
            };

            var paquete = await _context.Paquetes.FindAsync(producto.ProductoID);
            if (paquete != null)
            {
                productoViewModel.CantidadProductoxPaquete = paquete.CantidadProductoxPaquete;
                productoViewModel.CodigoPieza = await _getHelper.GetCodigoProductoAsync(paquete.PiezaProductoID);
            }

            return productoViewModel;
        }

        /// <summary>
        /// Convertir clase producto a productoDetailsViewModel (incluye productos asignados, piezas o paquetes)
        /// </summary>
        /// <param name="producto"></param>
        /// <returns>ProductViewModel(class)</returns>
        public async Task<ProductoDetailsViewModel> ToProductosDetailsViewModelAsync(Producto producto)
        {

            var productoDetailsViewModel = new ProductoDetailsViewModel()
            {
                Activo = producto.Activo,
                Codigo = producto.Codigo,
                Existencia = producto.Existencia,
                ExistenciaMaxima = producto.ExistenciaMaxima,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                ProductoDescripcion = producto.ProductoDescripcion,
                ProductoID = producto.ProductoID,
                ProductoNombre = producto.ProductoNombre,
                TasaID = producto.TasaID,
                TasasImpuestos = producto.TasasImpuestos,
                Unidades = producto.Unidades,
                UnidadID = producto.UnidadID,
                Paquete = producto.Paquete
            };

            if (productoDetailsViewModel.Paquete != null)
            {
                var productoAsignado = await _context.Productos.FindAsync(productoDetailsViewModel.Paquete.PiezaProductoID);
                if (productoAsignado != null)
                {
                    List<ProductoAsignadoViewModel> productosAsignados = new List<ProductoAsignadoViewModel>();
                    var productoAsignadoViewModel = new ProductoAsignadoViewModel()
                    {
                        ProductoID = productoAsignado.ProductoID,
                        Codigo = productoAsignado.Codigo,
                        Descripcion = productoAsignado.ProductoNombre,
                        Cantidad = productoDetailsViewModel.Paquete.CantidadProductoxPaquete
                    };
                    productosAsignados.Add(productoAsignadoViewModel);

                    productoDetailsViewModel.ProductosAsignadosViewModel = productosAsignados;
                }
            }
            else
            {
                productoDetailsViewModel.ProductosAsignadosViewModel = await (from p in _context.Productos
                                                                              join pa in _context.Paquetes on p.ProductoID equals pa.ProductoID
                                                                              where pa.PiezaProductoID == productoDetailsViewModel.ProductoID
                                                                              orderby p.Codigo
                                                                              select new ProductoAsignadoViewModel()
                                                                              {
                                                                                  ProductoID = p.ProductoID,
                                                                                  Codigo = p.Codigo,
                                                                                  Descripcion = p.ProductoNombre,
                                                                                  Cantidad = pa.CantidadProductoxPaquete
                                                                              }).ToListAsync();

            }

            return productoDetailsViewModel;
        }

    }
}
