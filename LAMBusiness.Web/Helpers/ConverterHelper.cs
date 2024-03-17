namespace LAMBusiness.Web.Helpers
{
    using LAMBusiness.Backend;
    using LAMBusiness.Contextos;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
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

        #region Image
        //public FileContentResult GenerateBarcode(String _data, BarcodeStandard.Type t)
        //{
        //    Barcode barcode = new Barcode()
        //    {
        //        IncludeLabel = false,
        //        Alignment = BarcodeStandard.AlignmentPositions.Center,
        //        Width = 400,
        //        Height = 200,
        //        //RotateFlipType = RotateFlipType.RotateNoneFlipNone,
        //        //BackColor = Color.White,
        //        //ForeColor = Color.Black,
        //    };
        //    SKImage img = barcode.Encode(t, _data);
        //    return new FileContentResult(ImageToByte(img), "image/png");
        //}
        public byte[] ImageToByte(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
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
            img = File.ReadAllBytes(path);
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

        //Datos Personales

        /// <summary>
        /// Convertir clase DatoPersonalViewModel a datoPersonal
        /// </summary>
        /// <param name="datoPersonalViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>DatoPersonal(class)</returns>
        public async Task<Resultado<DatoPersonal>> ToDatoPersonalAsync(DatoPersonalViewModel datoPersonalViewModel, bool isNew)
        {
            Resultado<DatoPersonal> resultado = new()
            {
                Datos = null,
                Error = true,
                Mensaje = ""
            };

            if (!string.IsNullOrEmpty(datoPersonalViewModel.CURP))
            {
                datoPersonalViewModel.CURP = datoPersonalViewModel.CURP.Trim().ToUpper();

                //Validar fecha de nacimiento
                var fechaNacimiento = $"{datoPersonalViewModel.CURP.Substring(8, 2)}-{datoPersonalViewModel.CURP.Substring(6, 2)}-{datoPersonalViewModel.CURP.Substring(4, 2)}";
                datoPersonalViewModel.FechaNacimiento = Convert.ToDateTime(fechaNacimiento);

                //validar género
                datoPersonalViewModel.GeneroID = datoPersonalViewModel.CURP.Substring(10, 1);
                switch (datoPersonalViewModel.GeneroID)
                {
                    case "H":
                        datoPersonalViewModel.GeneroID = "M";
                        break;
                    case "M":
                        datoPersonalViewModel.GeneroID = "F";
                        break;
                    default:
                        resultado.Mensaje = "Error en el valor del género [CURP]";
                        return resultado;
                }

                //validar Estado de nacimiento
                datoPersonalViewModel.EstadoNacimientoID = 0;
                var estadoNacimiento = datoPersonalViewModel.CURP.Substring(11, 2);
                var estado = await _context.Estados.FirstOrDefaultAsync(e => e.Clave == estadoNacimiento);
                if (estado == null)
                {
                    resultado.Mensaje = "Error en el valor del estado de nacimiento [CURP].";
                    return resultado;
                }

                datoPersonalViewModel.EstadoNacimientoID = estado.EstadoID;
            }

            var fechaActualizacion = DateTime.Now;
            if (isNew)
            {
                resultado.Error = false;
                resultado.Datos = new DatoPersonal()
                {
                    UsuarioID = Guid.NewGuid(),
                    CodigoPostal = datoPersonalViewModel.CodigoPostal,
                    Colonia = datoPersonalViewModel.Colonia.Trim().ToUpper(),
                    CURP = datoPersonalViewModel.CURP,
                    Domicilio = datoPersonalViewModel.Domicilio.Trim().ToUpper(),
                    EstadoCivilID = datoPersonalViewModel.EstadoCivilID,
                    EstadoNacimientoID = datoPersonalViewModel.EstadoNacimientoID,
                    FechaActualizacion = fechaActualizacion,
                    FechaNacimiento = datoPersonalViewModel.FechaNacimiento,
                    FechaRegistro = fechaActualizacion,
                    GeneroID = datoPersonalViewModel.GeneroID,
                    MunicipioID = datoPersonalViewModel.MunicipioID,
                    Municipios = await _getHelper.GetMunicipioByIdAsync((int)datoPersonalViewModel.MunicipioID),
                    PuestoID = datoPersonalViewModel.PuestoID,
                    Telefono = datoPersonalViewModel.Telefono,
                };
            }
            else
            {
                var employee = await _context.DatosPersonales.FindAsync(datoPersonalViewModel.UsuarioID);
                if (employee != null)
                {
                    employee.CodigoPostal = datoPersonalViewModel.CodigoPostal;
                    employee.Colonia = datoPersonalViewModel.Colonia.Trim().ToUpper();
                    employee.CURP = datoPersonalViewModel.CURP;
                    employee.Domicilio = datoPersonalViewModel.Domicilio.Trim().ToUpper();
                    employee.EstadoCivilID = datoPersonalViewModel.EstadoCivilID;
                    employee.EstadoNacimientoID = datoPersonalViewModel.EstadoNacimientoID;
                    employee.FechaActualizacion = fechaActualizacion;
                    employee.FechaNacimiento = datoPersonalViewModel.FechaNacimiento;
                    employee.GeneroID = datoPersonalViewModel.GeneroID;
                    employee.MunicipioID = datoPersonalViewModel.MunicipioID;
                    employee.Municipios = await _getHelper.GetMunicipioByIdAsync((int)datoPersonalViewModel.MunicipioID);
                    employee.PuestoID = datoPersonalViewModel.PuestoID;
                    employee.Telefono = datoPersonalViewModel.Telefono;
                    resultado.Datos = employee;
                    resultado.Error = false;
                }
                else
                {
                    resultado.Mensaje = "Identificador del colaborador incorrecto";
                }
            }

            return resultado;
        }

        /// <summary>
        /// Convertir clase colaborador a colaboradorViewModel.
        /// </summary>
        /// <param name="datoPersonal"></param>
        /// <returns>ColaboradorViewModel(class)</returns>
        public async Task<DatoPersonalViewModel> ToDatoPersonalViewModelAsync(DatoPersonal datoPersonal)
        {
            var _colaborador = await _context.DatosPersonales.FindAsync(datoPersonal.UsuarioID);

            var colaboradorViewModel = new DatoPersonalViewModel()
            {
                UsuarioID = datoPersonal.UsuarioID,
                CodigoPostal = datoPersonal.CodigoPostal,
                Colonia = datoPersonal.Colonia.Trim().ToUpper(),
                CURP = datoPersonal.CURP.Trim().ToUpper(),
                Domicilio = datoPersonal.Domicilio.Trim().ToUpper(),
                EstadoID = datoPersonal.Municipios.EstadoID,
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                EstadoCivilID = datoPersonal.EstadoCivilID,
                EstadosCivilesDDL = await _combosHelper.GetComboEstadosCivilesAsync(),
                EstadoNacimientoID = datoPersonal.EstadoNacimientoID,
                EstadosNacimientoDDL = await _combosHelper.GetComboEstadosAsync(),
                FechaNacimiento = datoPersonal.FechaNacimiento,
                GeneroID = datoPersonal.GeneroID,
                GenerosDDL = await _combosHelper.GetComboGenerosAsync(),
                FechaRegistro = _colaborador == null ? DateTime.Now : _colaborador.FechaRegistro,
                MunicipioID = datoPersonal.MunicipioID,
                Municipios = datoPersonal.Municipios,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(datoPersonal.Municipios.EstadoID),
                PuestoID = datoPersonal.PuestoID,
                PuestosDDL = await _combosHelper.GetComboPuestosAsync(),
                Telefono = datoPersonal.Telefono,
            };

            return colaboradorViewModel;
        }

        //Entradas

        /// <summary>
        /// Convertir clase EntradaviewModel a Entrada.
        /// </summary>
        /// <param name="entradaViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public async Task<Entrada> ToEntradaAsync(EntradaViewModel entradaViewModel, bool isNew)
        {
            return new Entrada()
            {
                Aplicado = entradaViewModel.Aplicado,
                EntradaID = isNew ? Guid.NewGuid() : entradaViewModel.EntradaID,
                Fecha = entradaViewModel.Fecha,
                FechaActualizacion = DateTime.Now,
                FechaCreacion = isNew ? DateTime.Now : entradaViewModel.FechaCreacion,
                Folio = entradaViewModel.Folio.Trim().ToUpper(),
                Observaciones = entradaViewModel.Observaciones == null ? "" : entradaViewModel.Observaciones.Trim().ToUpper(),
                ProveedorID = entradaViewModel.ProveedorID,
                UsuarioID = entradaViewModel.UsuarioID,
                Proveedores = await _getHelper.GetProveedorByIdAsync((Guid)entradaViewModel.ProveedorID)
            };

        }

        /// <summary>
        /// Convertir clase Entrada a EntradaViewModel.
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        public async Task<EntradaViewModel> ToEntradaViewModelAsync(Entrada entrada)
        {
            var _entrada = await _getHelper.GetEntradaByIdAsync(entrada.EntradaID);

            return new EntradaViewModel()
            {
                Aplicado = entrada == null ? false : entrada.Aplicado,
                EntradaID = entrada == null ? Guid.NewGuid() : entrada.EntradaID,
                Fecha = entrada == null ? DateTime.Now : entrada.Fecha,
                FechaActualizacion = entrada == null ? DateTime.Now : entrada.FechaActualizacion,
                FechaCreacion = _entrada == null ? DateTime.Now : _entrada.FechaCreacion,
                Folio = entrada.Folio == null ? "" : entrada.Folio.Trim().ToUpper(),
                Observaciones = entrada.Observaciones == null ? "" : entrada.Observaciones.Trim().ToUpper(),
                ProveedorID = entrada.ProveedorID,
                Proveedores = await _getHelper.GetProveedorByIdAsync((Guid)entrada.ProveedorID),
                UsuarioID = entrada.UsuarioID,
                Usuarios = entrada.Usuarios,
                EntradaDetalle = await _getHelper.GetEntradaDetalleByEntradaIdAsync(entrada.EntradaID)
            };

        }

        //Inventarios

        /// <summary>
        /// Convertir clase InventarioviewModel a Inventario.
        /// </summary>
        /// <param name="inventarioViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public Inventario ToInventarioAsync(InventarioViewModel inventarioViewModel, bool isNew)
        {
            return new Inventario()
            {
                Aplicado = inventarioViewModel.Aplicado,
                InventarioID = isNew ? Guid.NewGuid() : inventarioViewModel.InventarioID,
                Fecha = inventarioViewModel.Fecha,
                FechaActualizacion = DateTime.Now,
                FechaCreacion = isNew ? DateTime.Now : inventarioViewModel.FechaCreacion,
                Observaciones = inventarioViewModel.Observaciones == null ? "" : inventarioViewModel.Observaciones.Trim().ToUpper(),
                UsuarioID = inventarioViewModel.UsuarioID
            };
        }

        /// <summary>
        /// Convertir clase Inventario a InventarioViewModel.
        /// </summary>
        /// <param name="inventario"></param>
        /// <returns></returns>
        public async Task<InventarioViewModel> ToInventarioViewModelAsync(Inventario inventario)
        {
            var _inventario = await _getHelper.GetInventarioByIdAsync(inventario.InventarioID);

            return new InventarioViewModel()
            {
                Aplicado = inventario == null ? false : inventario.Aplicado,
                InventarioID = inventario == null ? Guid.NewGuid() : inventario.InventarioID,
                Fecha = inventario == null ? DateTime.Now : inventario.Fecha,
                FechaActualizacion = inventario == null ? DateTime.Now : inventario.FechaActualizacion,
                FechaCreacion = _inventario == null ? DateTime.Now : _inventario.FechaCreacion,
                Observaciones = inventario.Observaciones == null ? "" : inventario.Observaciones.Trim().ToUpper(),
                UsuarioID = inventario.UsuarioID,
                Usuarios = inventario.Usuarios,
                InventarioDetalle = await _getHelper.GetInventarioDetalleByInventarioIdAsync(inventario.InventarioID)
            };

        }

        //Módulos

        /// <summary>
        /// Convertir clase moduloViewModel a modulo
        /// </summary>
        /// <param name="moduloViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Cliente(class)</returns>
        public Modulo ToModulo(ModuloViewModel moduloViewModel, bool isNew)
        {
            var modulo = new Modulo()
            {
                Activo = moduloViewModel.Activo,
                Descripcion = moduloViewModel.Descripcion.Trim().ToUpper(),
                ModuloID = isNew ? Guid.NewGuid() : moduloViewModel.ModuloID,
                ModuloPadreID = moduloViewModel.ModuloPadreID,
            };

            return modulo;
        }

        /// <summary>
        /// Convertir clase cliente a clienteViewModel.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>ClienteViewModel(class)</returns>
        public async Task<ModuloViewModel> ToModuloViewModelAsync(Modulo modulo)
        {
            var _modulo = await _context.Modulos.FindAsync(modulo.ModuloID);

            var moduloViewModel = new ModuloViewModel()
            {
                Activo = modulo.Activo,
                Descripcion = modulo.Descripcion.Trim().ToUpper(),
                ModuloID = modulo.ModuloID,
                ModuloPadreID = modulo.ModuloPadreID,
                ModulosPadresDDL = await _combosHelper.GetComboModulosAsync()
            };

            return moduloViewModel;
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
                MarcaID = productoViewModel.MarcaID,
                PrecioCosto = isNew ? 0 : productoViewModel.PrecioCosto,
                PrecioVenta = productoViewModel.PrecioVenta,
                Descripcion = productoViewModel.Descripcion.Trim().ToUpper(),
                ProductoID = isNew ? Guid.NewGuid() : productoViewModel.ProductoID,
                Nombre = productoViewModel.Nombre.Trim().ToUpper(),
                TasaID = productoViewModel.TasaID,
                UnidadID = productoViewModel.UnidadID,
                Paquete = null
            };

            if (productoViewModel.Unidades.Paquete)
            {
                var _productos = new Productos(_context);
                var pieza = await _productos.ObtenerRegistroPorCodigoAsync(productoViewModel.CodigoPieza);

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
                MarcaID = producto.MarcaID,
                Marcas = producto.Marcas,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                Descripcion = producto.Descripcion,
                ProductoID = producto.ProductoID,
                Nombre = producto.Nombre,
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

                var _productos = new Productos(_context);
                var pieza = await _productos.ObtenerRegistroPorIdAsync(paquete.PiezaProductoID);
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
                MarcaID = producto.MarcaID,
                Marcas = producto.Marcas,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                Descripcion = producto.Descripcion,
                ProductoID = producto.ProductoID,
                Nombre = producto.Nombre,
                TasaID = producto.TasaID,
                TasasImpuestos = producto.TasasImpuestos,
                Unidades = producto.Unidades,
                UnidadID = producto.UnidadID,
                Paquete = producto.Paquete,
                Existencias = producto.Existencias
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
                        Descripcion = productoAsignado.Nombre,
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
                                                                                  Descripcion = p.Nombre,
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
                FrecuenciaVisitas = proveedorViewModel.FrecuenciaVisitas,
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
                FrecuenciaVisitas = proveedor.FrecuenciaVisitas,
                MunicipioID = proveedor.MunicipioID,
                Municipios = proveedor.Municipios,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(proveedor.Municipios.EstadoID),
                Nombre = proveedor.Nombre.Trim().ToUpper(),
                RFC = proveedor.RFC.Trim().ToUpper(),
                Telefono = proveedor.Telefono
            };

            return proveedorViewModel;
        }

        //Salidas

        /// <summary>
        /// Convertir clase SalidaViewModel a Salida.
        /// </summary>
        /// <param name="salidaViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public async Task<Salida> ToSalidaAsync(SalidaViewModel salidaViewModel, bool isNew)
        {
            DateTime _fecha = DateTime.Now;
            DateTime _fechaCreacion = _fecha;

            if (!isNew)
            {
                var fechaCreacion = await _context.Salidas
                    .Where(s => s.SalidaID == salidaViewModel.SalidaID)
                    .Select(s => s.FechaCreacion).FirstOrDefaultAsync();

                if (fechaCreacion == null)
                {
                    fechaCreacion = _fecha;
                }
            }

            return new Salida()
            {
                Aplicado = salidaViewModel.Aplicado,
                SalidaID = isNew ? Guid.NewGuid() : salidaViewModel.SalidaID,
                Fecha = salidaViewModel.Fecha,
                FechaActualizacion = _fecha,
                FechaCreacion = _fechaCreacion,
                Folio = salidaViewModel.Folio.Trim().ToUpper(),
                Observaciones = salidaViewModel.Observaciones == null ? "" : salidaViewModel.Observaciones.Trim().ToUpper(),
                SalidaTipoID = salidaViewModel.SalidaTipoID,
                SalidaTipo = await _getHelper.GetSalidaTipoByIdAsync((Guid)salidaViewModel.SalidaTipoID),
                UsuarioID = salidaViewModel.UsuarioID
            };

        }

        /// <summary>
        /// Convertir clase Salida a SalidaViewModel.
        /// </summary>
        /// <param name="salida"></param>
        /// <returns></returns>
        public async Task<SalidaViewModel> ToSalidaViewModelAsync(Salida salida)
        {
            var _salida = await _getHelper.GetSalidaByIdAsync(salida.SalidaID);

            return new SalidaViewModel()
            {
                Aplicado = salida == null ? false : salida.Aplicado,
                Fecha = salida == null ? DateTime.Now : salida.Fecha,
                FechaActualizacion = salida == null ? DateTime.Now : salida.FechaActualizacion,
                FechaCreacion = _salida == null ? DateTime.Now : _salida.FechaCreacion,
                Folio = salida.Folio == null ? "" : salida.Folio.Trim().ToUpper(),
                Observaciones = salida.Observaciones == null ? "" : salida.Observaciones.Trim().ToUpper(),
                SalidaID = salida == null ? Guid.NewGuid() : salida.SalidaID,
                SalidaTipoID = salida.SalidaTipoID,
                SalidaTipo = await _getHelper.GetSalidaTipoByIdAsync((Guid)salida.SalidaTipoID),
                SalidaTipoDDL = await _combosHelper.GetComboSalidasTipoAsync(),
                UsuarioID = salida.UsuarioID,
                Usuarios = salida.Usuarios,
                SalidaDetalle = await _getHelper.GetSalidaDetalleBySalidaIdAsync(salida.SalidaID)
            };

        }

        //Usuarios
        /// <summary>
        /// Convertir clase usuarioViewModel a usuario
        /// </summary>
        /// <param name="usuarioViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Producto(class)</returns>
        public async Task<Usuario> ToUsuarioAsync(UsuarioViewModel usuarioViewModel, bool isNew)
        {
            if (isNew)
            {
                return new Usuario()
                {
                    Activo = usuarioViewModel.Activo,
                    AdministradorID = usuarioViewModel.AdministradorID,
                    FechaInicio = usuarioViewModel.FechaInicio.AddHours(0).AddMinutes(0).AddSeconds(0),
                    FechaTermino = usuarioViewModel.FechaTermino.AddHours(23).AddMinutes(59).AddSeconds(59),
                    FechaUltimoAcceso = usuarioViewModel.FechaUltimoAcceso,
                    Email = string.IsNullOrEmpty(usuarioViewModel.Email) ? "" : usuarioViewModel.Email.ToLower(),
                    Nombre = string.IsNullOrEmpty(usuarioViewModel.Nombre) ? "" : usuarioViewModel.Nombre.ToUpper(),
                    PrimerApellido = string.IsNullOrEmpty(usuarioViewModel.PrimerApellido) ? "" : usuarioViewModel.PrimerApellido.ToUpper(),
                    SegundoApellido = string.IsNullOrEmpty(usuarioViewModel.SegundoApellido) ? "" : usuarioViewModel.SegundoApellido.ToUpper(),
                    TelefonoMovil = string.IsNullOrEmpty(usuarioViewModel.TelefonoMovil) ? "" : usuarioViewModel.TelefonoMovil,
                    Password = "",
                    UsuarioID = Guid.NewGuid()
                };
            }
            else
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioViewModel.UsuarioID);
                if (usuario == null)
                    return null;

                usuario.Activo = usuarioViewModel.Activo;
                usuario.AdministradorID = usuarioViewModel.AdministradorID;
                usuario.Email = usuarioViewModel.Email;
                usuario.Nombre = usuarioViewModel.Nombre;
                usuario.PrimerApellido = usuarioViewModel.PrimerApellido;
                usuario.SegundoApellido = usuarioViewModel.SegundoApellido;
                usuario.TelefonoMovil = usuarioViewModel.TelefonoMovil;
                usuario.FechaInicio = usuarioViewModel.FechaInicio.AddHours(0).AddMinutes(0).AddSeconds(0);
                usuario.FechaTermino = usuarioViewModel.FechaTermino.AddHours(23).AddMinutes(59).AddSeconds(59);

                return usuario;
            }

        }

        //public FileContentResult GenerateBarcode(string _data, Type t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
