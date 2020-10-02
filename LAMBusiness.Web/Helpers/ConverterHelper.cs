namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Models.ViewModels;
    using Shared.Catalogo;
    using Shared.Contacto;

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

        //Clientes

        /// <summary>
        /// Convertir clase clienteViewModel a cliente
        /// </summary>
        /// <param name="clienteViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Cliente(class)</returns>
        public async Task<Cliente> ToClienteAsync(ClienteViewModel clienteViewModel, bool isNew)
        {
            var cliente = new Cliente()
            {
                Activo = clienteViewModel.Activo,
                ClienteID = isNew ? Guid.NewGuid() : clienteViewModel.ClienteID,
                CodigoPostal = clienteViewModel.CodigoPostal,
                Colonia = clienteViewModel.Colonia.Trim().ToUpper(),
                Contacto = clienteViewModel.Contacto.Trim().ToUpper(),
                Domicilio = clienteViewModel.Domicilio.Trim().ToUpper(),
                Email = clienteViewModel.Email.Trim().ToLower(),
                FechaRegistro = DateTime.Now,
                MunicipioID = clienteViewModel.MunicipioID,
                Municipios = await _getHelper.GetMunicipioByIdAsync((int)clienteViewModel.MunicipioID),
                Nombre = clienteViewModel.Nombre.Trim().ToUpper(),
                RFC = clienteViewModel.RFC.Trim().ToUpper(),
                Telefono = clienteViewModel.Telefono,
                TelefonoMovil = clienteViewModel.TelefonoMovil
            };

            return cliente;
        }

        /// <summary>
        /// Convertir clase cliente a clienteViewModel.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>ClienteViewModel(class)</returns>
        public async Task<ClienteViewModel> ToClienteViewModelAsync(Cliente cliente)
        {

            var clienteViewModel = new ClienteViewModel()
            {
                Activo = cliente.Activo,
                ClienteID = cliente.ClienteID,
                CodigoPostal = cliente.CodigoPostal,
                Colonia = cliente.Colonia.Trim().ToUpper(),
                Contacto = cliente.Contacto.Trim().ToUpper(),
                Domicilio = cliente.Domicilio.Trim().ToUpper(),
                Email = cliente.Email.Trim().ToLower(),
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                FechaRegistro = DateTime.Now,
                MunicipioID = cliente.MunicipioID,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(cliente.Municipios.EstadoID),
                Municipios = await _getHelper.GetMunicipioByIdAsync((int)cliente.MunicipioID),
                Nombre = cliente.Nombre.Trim().ToUpper(),
                RFC = cliente.RFC.Trim().ToUpper(),
                Telefono = cliente.Telefono,
                TelefonoMovil = cliente.TelefonoMovil
            };

            return clienteViewModel;
        }

        //Productos

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

            if (productoViewModel.Unidades.Paquete)
            {
                var pieza = await _getHelper.GetProductByCodeAsync(productoViewModel.CodigoPieza);

                var paquete = new Paquete()
                {
                    CantidadProductoxPaquete = (decimal)productoViewModel.CantidadProductoxPaquete,
                    ProductoID = producto.ProductoID,
                    PiezaProductoID = pieza.ProductoID
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

                var pieza = await _getHelper.GetProductByIdAsync(paquete.PiezaProductoID);
                productoViewModel.CodigoPieza = pieza.Codigo;
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
