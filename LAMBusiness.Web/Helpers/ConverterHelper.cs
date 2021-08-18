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
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;

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

        //Colaboradores

        /// <summary>
        /// Convertir clase colaboradorViewModel a colaborador
        /// </summary>
        /// <param name="colaboradorViewModel"></param>
        /// <param name="isNew"></param>
        /// <returns>Colaborador(class)</returns>
        public async Task<Resultado<Colaborador>> ToColaboradorAsync(ColaboradorViewModel colaboradorViewModel, bool isNew)
        {
            Resultado<Colaborador> resultado = new Resultado<Colaborador>() {
                Contenido = null,
                Error = true,
                Mensaje = ""
            };

            if (!string.IsNullOrEmpty(colaboradorViewModel.CURP))
            {
                colaboradorViewModel.CURP = colaboradorViewModel.CURP.Trim().ToUpper();
                
                //Validar fecha de nacimiento
                var fechaNacimiento = $"{colaboradorViewModel.CURP.Substring(8, 2)}-{colaboradorViewModel.CURP.Substring(6, 2)}-{colaboradorViewModel.CURP.Substring(4, 2)}";
                colaboradorViewModel.FechaNacimiento = Convert.ToDateTime(fechaNacimiento);

                //validar género
                colaboradorViewModel.GeneroID = colaboradorViewModel.CURP.Substring(10, 1);
                switch (colaboradorViewModel.GeneroID)
                {
                    case "H":
                        colaboradorViewModel.GeneroID = "M";
                        break;
                    case "M":
                        colaboradorViewModel.GeneroID = "F";
                        break;
                    default:
                        resultado.Mensaje = "Error en el valor del género [CURP]";
                        return resultado;
                }
                
                //validar Estado de nacimiento
                colaboradorViewModel.EstadoNacimientoID = 0;
                var estadoNacimiento = colaboradorViewModel.CURP.Substring(11, 2);
                var estado = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoClave == estadoNacimiento);
                if (estado == null)
                {
                    resultado.Mensaje = "Error en el valor del estado de nacimiento [CURP].";
                    return resultado;
                }
                    
                colaboradorViewModel.EstadoNacimientoID = estado.EstadoID;
            }
            
            var fechaActualizacion = DateTime.Now;
            if (isNew)
            {
                resultado.Error = false;
                resultado.Contenido = new Colaborador()
                {
                    Activo = colaboradorViewModel.Activo,
                    ColaboradorID = Guid.NewGuid(),
                    CodigoPostal = colaboradorViewModel.CodigoPostal,
                    Colonia = colaboradorViewModel.Colonia.Trim().ToUpper(),
                    CURP = colaboradorViewModel.CURP,
                    Domicilio = colaboradorViewModel.Domicilio.Trim().ToUpper(),
                    Email = colaboradorViewModel.Email.Trim().ToLower(),
                    EstadoCivilID = colaboradorViewModel.EstadoCivilID,
                    EstadoNacimientoID = colaboradorViewModel.EstadoNacimientoID,
                    FechaActualizacion = fechaActualizacion,
                    FechaNacimiento = colaboradorViewModel.FechaNacimiento,
                    FechaRegistro = fechaActualizacion,
                    GeneroID = colaboradorViewModel.GeneroID,
                    MunicipioID = colaboradorViewModel.MunicipioID,
                    Municipios = await _getHelper.GetMunicipioByIdAsync((int)colaboradorViewModel.MunicipioID),
                    Nombre = colaboradorViewModel.Nombre.Trim().ToUpper(),
                    PrimerApellido = colaboradorViewModel.PrimerApellido.Trim().ToUpper(),
                    PuestoID = colaboradorViewModel.PuestoID,
                    SegundoApellido = colaboradorViewModel.SegundoApellido == null ? "" : colaboradorViewModel.SegundoApellido.Trim().ToUpper(),
                    Telefono = colaboradorViewModel.Telefono,
                    TelefonoMovil = colaboradorViewModel.TelefonoMovil
                };
            }
            else
            {
                var employee = await _context.Colaboradores.FindAsync(colaboradorViewModel.ColaboradorID);
                if (employee != null)
                {
                    employee.Activo = colaboradorViewModel.Activo;
                    employee.CodigoPostal = colaboradorViewModel.CodigoPostal;
                    employee.Colonia = colaboradorViewModel.Colonia.Trim().ToUpper();
                    employee.CURP = colaboradorViewModel.CURP;
                    employee.Domicilio = colaboradorViewModel.Domicilio.Trim().ToUpper();
                    employee.Email = colaboradorViewModel.Email.Trim().ToLower();
                    employee.EstadoCivilID = colaboradorViewModel.EstadoCivilID;
                    employee.EstadoNacimientoID = colaboradorViewModel.EstadoNacimientoID;
                    employee.FechaActualizacion = fechaActualizacion;
                    employee.FechaNacimiento = colaboradorViewModel.FechaNacimiento;
                    employee.GeneroID = colaboradorViewModel.GeneroID;
                    employee.MunicipioID = colaboradorViewModel.MunicipioID;
                    employee.Municipios = await _getHelper.GetMunicipioByIdAsync((int)colaboradorViewModel.MunicipioID);
                    employee.Nombre = colaboradorViewModel.Nombre.Trim().ToUpper();
                    employee.PrimerApellido = colaboradorViewModel.PrimerApellido.Trim().ToUpper();
                    employee.PuestoID = colaboradorViewModel.PuestoID;
                    employee.SegundoApellido = colaboradorViewModel.SegundoApellido == null ? "" : colaboradorViewModel.SegundoApellido.Trim().ToUpper();
                    employee.Telefono = colaboradorViewModel.Telefono;
                    employee.TelefonoMovil = colaboradorViewModel.TelefonoMovil;

                    resultado.Contenido = employee;
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
        /// <param name="colaborador"></param>
        /// <returns>ColaboradorViewModel(class)</returns>
        public async Task<ColaboradorViewModel> ToColaboradorViewModelAsync(Colaborador colaborador)
        {
            var _colaborador = await _context.Colaboradores.FindAsync(colaborador.ColaboradorID);

            var colaboradorViewModel = new ColaboradorViewModel()
            {
                Activo = colaborador.Activo,
                ColaboradorID = colaborador.ColaboradorID,
                CodigoPostal = colaborador.CodigoPostal,
                Colonia = colaborador.Colonia.Trim().ToUpper(),
                CURP = colaborador.CURP.Trim().ToUpper(),
                Domicilio = colaborador.Domicilio.Trim().ToUpper(),
                Email = colaborador.Email.Trim().ToLower(),
                EstadoID = colaborador.Municipios.EstadoID,
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                EstadoCivilID = colaborador.EstadoCivilID,
                EstadosCivilesDDL = await _combosHelper.GetComboEstadosCivilesAsync(),
                EstadoNacimientoID = colaborador.EstadoNacimientoID,
                EstadosNacimientoDDL = await _combosHelper.GetComboEstadosAsync(),
                FechaNacimiento = colaborador.FechaNacimiento,
                GeneroID = colaborador.GeneroID,
                GenerosDDL = await _combosHelper.GetComboGenerosAsync(),
                FechaRegistro = _colaborador == null ? DateTime.Now : _colaborador.FechaRegistro,
                MunicipioID = colaborador.MunicipioID,
                Municipios = colaborador.Municipios,
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(colaborador.Municipios.EstadoID),
                Nombre = colaborador.Nombre.Trim().ToUpper(),
                PrimerApellido = colaborador.PrimerApellido.Trim().ToUpper(),
                PuestoID = colaborador.PuestoID,
                PuestosDDL = await _combosHelper.GetComboPuestosAsync(),
                SegundoApellido = colaborador.SegundoApellido == null ? "" : colaborador.SegundoApellido.Trim().ToUpper(),
                Telefono = colaborador.Telefono,
                TelefonoMovil = colaborador.TelefonoMovil
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
                Observaciones = entradaViewModel.Observaciones.Trim().ToUpper(),
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
                EntradaDetalle = await _getHelper.GetEntradaDetalleByEntradaIdAsync(entrada.EntradaID)
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
                MarcaID = producto.MarcaID,
                Marcas = producto.Marcas,
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
                MarcaID = producto.MarcaID,
                Marcas = producto.Marcas,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                ProductoDescripcion = producto.ProductoDescripcion,
                ProductoID = producto.ProductoID,
                ProductoNombre = producto.ProductoNombre,
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
            var fechaCreacion = await _context.Salidas
                .Where(s => s.SalidaID == salidaViewModel.SalidaID)
                .Select(s => s.FechaCreacion).FirstOrDefaultAsync();

            if (fechaCreacion == null)
            {
                return null;
            }

            return new Salida()
            {
                Aplicado = salidaViewModel.Aplicado,
                SalidaID = isNew ? Guid.NewGuid() : salidaViewModel.SalidaID,
                Fecha = salidaViewModel.Fecha,
                FechaActualizacion = DateTime.Now,
                FechaCreacion = isNew ? DateTime.Now: fechaCreacion,
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
                    ColaboradorID = usuarioViewModel.ColaboradorID,
                    FechaInicio = usuarioViewModel.FechaInicio.AddHours(0).AddMinutes(0).AddSeconds(0),
                    FechaTermino = usuarioViewModel.FechaTermino.AddHours(23).AddMinutes(59).AddSeconds(59),
                    FechaUltimoAcceso = usuarioViewModel.FechaUltimoAcceso,
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
                usuario.FechaInicio = usuarioViewModel.FechaInicio.AddHours(0).AddMinutes(0).AddSeconds(0);
                usuario.FechaTermino = usuarioViewModel.FechaTermino.AddHours(23).AddMinutes(59).AddSeconds(59);

                return usuario;
            }

        }

        #endregion
    }
}
