namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
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

        #region Image
        public byte[] UploadImageBase64(string file, int size)
        {
            byte[] aImage;
            aImage = Convert.FromBase64String(file);
            MemoryStream ms = new MemoryStream(aImage, 0, aImage.Length);
            ms.Write(aImage, 0, aImage.Length);
            try
            {
                Image img = Image.FromStream(ms, true);

                int max = size;
                int h, w, newH, newW;

                h = img.Height;
                w = img.Width;
                //if (h > w && h > max)
                if (h > max)
                {
                    newH = max;
                    newW = w * max / h;
                }
                //else if (w > h && w > max)
                else if (w > max)
                {
                    newH = h * max / w;
                    newW = max;
                }
                else
                {
                    newH = h;
                    newW = w;
                }

                if (h != newH && w != newW)
                {
                    Bitmap newImg = new Bitmap(img, newW, newH);
                    Graphics g = Graphics.FromImage(newImg);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.DrawImage(img, 0, 0, newImg.Width, newImg.Height);
                    MemoryStream ms2 = new MemoryStream();
                    newImg.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aImage = ms2.ToArray();
                }
                return aImage;
            }
            catch
            {
                return null;
            }
        }
        public FileContentResult ToImageBase64(string path)
        {
            byte[] img = null;
            img = System.IO.File.ReadAllBytes(path);
            if (img == null) return null;
            return new FileContentResult(img, "image/jpeg");
        }
        #endregion

        #region Entities
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
                Domicilio = clienteViewModel.Domicilio.Trim().ToUpper(),
                Email = clienteViewModel.Email.Trim().ToLower(),
                FechaRegistro = DateTime.Now,
                MunicipioID = clienteViewModel.MunicipioID,
                Municipios = await _getHelper.GetMunicipioByIdAsync((int)clienteViewModel.MunicipioID),
                Nombre = clienteViewModel.Nombre.Trim().ToUpper(),
                RFC = clienteViewModel.RFC.Trim().ToUpper(),
                Telefono = clienteViewModel.Telefono
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
            var _cliente = await _context.Clientes.FindAsync(cliente.ClienteID);

            var clienteViewModel = new ClienteViewModel()
            {
                Activo = cliente.Activo,
                ClienteID = cliente.ClienteID,
                CodigoPostal = cliente.CodigoPostal,
                Colonia = cliente.Colonia.Trim().ToUpper(),
                Domicilio = cliente.Domicilio.Trim().ToUpper(),
                Email = cliente.Email.Trim().ToLower(),
                EstadoID = cliente.Municipios.EstadoID,
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                FechaRegistro = _cliente == null ? DateTime.Now : _cliente.FechaRegistro,
                MunicipioID = cliente.MunicipioID,
                Municipios = cliente.Municipios,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(cliente.Municipios.EstadoID),
                Nombre = cliente.Nombre.Trim().ToUpper(),
                RFC = cliente.RFC.Trim().ToUpper(),
                Telefono = cliente.Telefono
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

        //Proveedores

        /// <summary>
        /// Convertir clase proveedorViewModel a proveedor
        /// </summary>
        /// <param name="proveedorViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Proveedor(class)</returns>
        public async Task<Proveedor> ToProveedorAsync(ProveedorViewModel proveedorViewModel, bool isNew)
        {
            var proveedor = new Proveedor()
            {
                Activo = proveedorViewModel.Activo,
                ProveedorID = isNew ? Guid.NewGuid() : proveedorViewModel.ProveedorID,
                CodigoPostal = proveedorViewModel.CodigoPostal,
                Colonia = proveedorViewModel.Colonia.Trim().ToUpper(),
                Domicilio = proveedorViewModel.Domicilio.Trim().ToUpper(),
                Email = proveedorViewModel.Email.Trim().ToLower(),
                FechaRegistro = DateTime.Now,
                MunicipioID = proveedorViewModel.MunicipioID,
                Municipios = await _getHelper.GetMunicipioByIdAsync((int)proveedorViewModel.MunicipioID),
                Nombre = proveedorViewModel.Nombre.Trim().ToUpper(),
                RFC = proveedorViewModel.RFC.Trim().ToUpper(),
                Telefono = proveedorViewModel.Telefono
            };

            return proveedor;
        }

        /// <summary>
        /// Convertir clase proveedor a proveedorViewModel.
        /// </summary>
        /// <param name="proveedor"></param>
        /// <returns>ProveedorViewModel(class)</returns>
        public async Task<ProveedorViewModel> ToProveedorViewModelAsync(Proveedor proveedor)
        {
            var _proveedor = await _context.Proveedores.FindAsync(proveedor.ProveedorID);

            var proveedorViewModel = new ProveedorViewModel()
            {
                Activo = proveedor.Activo,
                ProveedorID = proveedor.ProveedorID,
                CodigoPostal = proveedor.CodigoPostal,
                Colonia = proveedor.Colonia.Trim().ToUpper(),
                Domicilio = proveedor.Domicilio.Trim().ToUpper(),
                Email = proveedor.Email.Trim().ToLower(),
                EstadoID = proveedor.Municipios.EstadoID,
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                FechaRegistro = _proveedor == null ? DateTime.Now : _proveedor.FechaRegistro,
                MunicipioID = proveedor.MunicipioID,
                Municipios = proveedor.Municipios,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(proveedor.Municipios.EstadoID),
                Nombre = proveedor.Nombre.Trim().ToUpper(),
                RFC = proveedor.RFC.Trim().ToUpper(),
                Telefono = proveedor.Telefono
            };

            return proveedorViewModel;
        } 
        #endregion

    }
}
