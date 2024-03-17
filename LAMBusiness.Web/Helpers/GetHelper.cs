namespace LAMBusiness.Web.Helpers
{
    using LAMBusiness.Backend;
    using LAMBusiness.Contextos;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels;
    using Newtonsoft.Json;
    using Shared.Aplicacion;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class GetHelper : IGetHelper
    {
        private readonly DataContext _context;
        private readonly BitacoraContext _dbBitacora;

        public GetHelper(DataContext context, BitacoraContext dbBitacora)
        {
            _context = context;
            _dbBitacora = dbBitacora;
        }

        //Administradores

        /// <summary>
        /// Obtener administrador por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>String</returns>
        public async Task<Administrador> GetAdministradorByIdAsync(string id)
        {
            return await _context.Administradores
                .FirstOrDefaultAsync(a => a.AdministradorID == id);
        }

        //Almacenes

        /// <summary>
        /// Obtener almacén por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>String</returns>
        public async Task<Almacen> GetAlmacenByIdAsync(Guid id)
        {
            return await _context.Almacenes
                .FirstOrDefaultAsync(p => p.AlmacenID == id);
        }

        /// <summary>
        /// Obtener almacen por nombre.
        /// </summary>
        /// <param name="almacen"></param>
        /// <returns></returns>
        public async Task<Almacen> GetAlmacenByNombreAsync(string almacen)
        {
            almacen = string.IsNullOrEmpty(almacen) ? "" : almacen.Trim().ToUpper();

            return await _context.Almacenes
                .FirstOrDefaultAsync(p => p.Nombre == almacen);
        }

        /// <summary>
        /// Obtener lista de almacenes de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<Filtro<List<Almacen>>> GetAlmacenesByPatternAsync(Filtro<List<Almacen>> filtro)
        {
            IQueryable<Almacen> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Almacenes
                                .Where(a => a.Nombre.Contains(w) ||
                                            a.Descripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(a => a.Nombre.Contains(w) ||
                                                a.Descripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Almacenes;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query
                .OrderBy(p => p.Nombre)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            return filtro;
        }

        //Bitácora



        //Clientes

        /// <summary>
        /// Obtener cliente por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Cliente> GetClienteByIdAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.ClienteContactos)
                .FirstOrDefaultAsync(c => c.ClienteID == id);
        }

        //Aplicación

        private string GetHostName()
        {
            string localIP = string.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.HostName.Trim().ToUpper();
        }

        private string GetIPPrivada()
        {
            string localIP = string.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP += ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        private string GetIPPublica()
        {
            string htmlIpPublica = string.Empty;
            string ip = string.Empty;
            try
            {
                // Solicitud Web:
                WebRequest solicitudWeb = WebRequest.Create("http://checkip.dyndns.org/");
                // Recuperación de código HTML que contiene la dirección IP pública:
                using (WebResponse respuestaWeb = solicitudWeb.GetResponse())
                using (StreamReader stream = new StreamReader(respuestaWeb.GetResponseStream()))
                {
                    htmlIpPublica = stream.ReadToEnd();
                }
                // Expresión regular para una dirección IP:
                System.Text.RegularExpressions.Regex regexIp = new System.Text.RegularExpressions.Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                System.Text.RegularExpressions.MatchCollection resultado = regexIp.Matches(htmlIpPublica);
                foreach (System.Text.RegularExpressions.Match match in resultado)
                {
                    ip = match.Value;
                    break;
                }
            }
            catch
            {
                ip = "Sin Conexión";
            }
            return ip;
        }

        //Bitácora

        public async Task SetBitacoraAsync(Token token, string accion, Guid moduloId, object clase,
            string parametroId, string directorio, string excepcion = "")
        {
            Guid bitacoraId = Guid.NewGuid();
            Bitacora bitacora = new Bitacora()
            {
                Accion = accion.Trim(),
                AccionRealizada = excepcion == "" ? true : false,
                BitacoraID = bitacoraId,
                Fecha = DateTime.Now,
                Hostname = token.HostName,
                IPPrivada = token.IPPrivada,
                IPPublica = token.IPPublica,
                ModuloID = moduloId,
                ParametrosJson = JsonConvert.SerializeObject(clase, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                ParametroID = parametroId,
                UsuarioID = token.UsuarioID
            };

            _dbBitacora.Bitacora.Add(bitacora);

            if (!string.IsNullOrEmpty(excepcion))
            {
                _dbBitacora.BitacoraExcepciones.Add(new BitacoraExcepciones()
                {
                    BitacoraID = bitacoraId,
                    Excepcion = excepcion
                });
            }

            try
            {
                await _dbBitacora.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BitacoraExcepcionViewModel bitacoraExcepcion = new BitacoraExcepcionViewModel()
                {
                    Accion = accion.Trim(),
                    AccionRealizada = excepcion == "" ? true : false,
                    BitacoraID = bitacoraId,
                    ErrorAlGuardarBitacoraDB = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString(),
                    Excepcion = excepcion,
                    Fecha = DateTime.Now,
                    Hostname = token.HostName,
                    IPPrivada = token.IPPrivada,
                    IPPublica = token.IPPublica,
                    ModuloID = moduloId,
                    ParametrosJson = JsonConvert.SerializeObject(clase, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    ParametroID = parametroId,
                    UsuarioID = token.UsuarioID
                };
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                string file = Path.Combine(directorio, $"{bitacoraId}.txt");
                File.WriteAllText(file, JsonConvert.SerializeObject(bitacoraExcepcion));
            }
        }

        //DatosPersonales

        /// <summary>
        /// Obtener datos personales por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DatoPersonal> GetDatosPersonalesByIdAsync(Guid id)
        {
            return await _context.DatosPersonales
                .Include(c => c.Estados)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Generos)
                .Include(c => c.Municipios)
                .Include(c => c.Puestos)
                .FirstOrDefaultAsync(p => p.UsuarioID == id);
        }

        /// <summary>
        /// Obtener datgos personales por CURP.
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public async Task<DatoPersonal> GetDatosPersonalesByCURPAsync(string curp)
        {
            curp = string.IsNullOrEmpty(curp) ? "" : curp.Trim().ToUpper();

            return await _context.DatosPersonales
                .FirstOrDefaultAsync(c => c.CURP == curp);
        }

        //Contacto (Clientes)

        /// <summary>
        /// Obtener Contacto del cliente por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClienteContacto> GetContactoClienteByIdAsync(Guid id)
        {
            return await _context.ClienteContactos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.ClienteContactoID == id);
        }

        /// <summary>
        /// Obtener Contacto del cliente por ID del cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClienteContacto> GetContactoClienteByClienteIdAsync(Guid id)
        {
            return await _context.ClienteContactos.FirstOrDefaultAsync(c => c.ClienteID == id);
        }

        //Contacto (Proveedores)

        /// <summary>
        /// Obtener Contacto del proveedor por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProveedorContacto> GetContactoProveedorByIdAsync(Guid id)
        {
            return await _context.ProveedorContactos
                .Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.ProveedorContactoID == id);
        }

        /// <summary>
        /// Obtener Contacto del proveedor por ID del cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProveedorContacto> GetContactoProveedorByProveedorIdAsync(Guid id)
        {
            return await _context.ProveedorContactos.FirstOrDefaultAsync(c => c.ProveedorID == id);
        }

        //Dashboard
        public async Task<EstadisticaMovimientoChartViewModel> GetMovementsDashboardAsync(List<int> años)
        {
            if (años.Count > 0)
            {
                List<TotalVentasPorAñoViewModel> totalVentasPorAño = new List<TotalVentasPorAñoViewModel>();
                Guid almacenId = Guid.Parse("8706EF28-2EBA-463A-BAB4-62227965F03F");
                foreach (var año in años)
                {
                    var estadisticas = await _context.EstadisticasMovimientosMensual
                        .Where(e => e.AlmacenID == almacenId && e.Año == año)
                        .ToListAsync();
                    if (estadisticas != null && estadisticas.Count > 0)
                    {
                        List<decimal> importe = new List<decimal>();
                        for (byte i = 1; i <= 12; i++)
                        {
                            var estadistica = estadisticas.FirstOrDefault(e => e.Mes == i);
                            if (estadistica != null)
                                importe.Add((decimal)estadistica.VentasImporte);
                            else
                                importe.Add(0);
                        }

                        totalVentasPorAño.Add(new TotalVentasPorAñoViewModel()
                        {
                            Name = $"Total Venta {año}",
                            Data = importe
                        });
                    }
                }

                return new EstadisticaMovimientoChartViewModel()
                {
                    TotalVentasPorAño = totalVentasPorAño
                };
            }
            return null;
        }

        //Estado

        /// <summary>
        /// Obtener Estado por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Estado> GetEstadosByIdAsync(short id)
        {
            return await _context.Estados.FindAsync(id);
        }

        //Entradas

        /// <summary>
        /// Obtener Entrada por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Entrada> GetEntradaByIdAsync(Guid id)
        {
            return await _context.Entradas
                .Include(e => e.Usuarios)
                .ThenInclude(e => e.DatosPersonales)
                .Include(e => e.Proveedores)
                .ThenInclude(e => e.Municipios)
                .ThenInclude(e => e.Estados)
                .FirstOrDefaultAsync(m => m.EntradaID == id);
        }

        /// <summary>
        /// obtener detalle de entrada por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EntradaDetalle> GetEntradaDetalleByIdAsync(Guid id)
        {
            return await _context.EntradasDetalle
                .Include(e => e.Almacenes)
                .Include(e => e.Entradas)
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(m => m.EntradaDetalleID == id);
        }

        /// <summary>
        /// Obtener lista de detalle de entrada por EntradaID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EntradaDetalle>> GetEntradaDetalleByEntradaIdAsync(Guid id)
        {
            return await _context.EntradasDetalle
                .Include(e => e.Entradas)
                .Include(e => e.Almacenes)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Unidades)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Paquete)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Existencias)
                .Where(m => m.EntradaID == id)
                .OrderBy(m => m.Productos.Nombre)
                .ThenBy(m => m.Almacenes.Nombre)
                .ToListAsync();
        }

        //Existencias

        /// <summary>
        /// Obtener Existencia por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Existencia> GetExistenciaByIdAsync(Guid id)
        {
            return await _context.Existencias
                .Include(e => e.Almacenes)
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(m => m.ExistenciaID == id);
        }

        /// <summary>
        /// Obtener existencia por ProductoID.
        /// </summary>
        /// <param name="productoId"></param>
        /// <returns>Sum(ExistenciaEnAlmacen)</returns>
        public async Task<decimal> GetExistenciaByProductoIdAsync(Guid productoId)
        {
            return await _context.Existencias
                .Include(e => e.Productos)
                .Where(m => m.ProductoID == productoId)
                .SumAsync(m => m.ExistenciaEnAlmacen);
        }

        /// <summary>
        /// Obtener existencia por ProductoID y AlmacenID.
        /// </summary>
        /// <param name="productoId"></param>
        /// <returns></returns>
        public async Task<Existencia> GetExistenciaByProductoIdAndAlmacenIdAsync(Guid productoId, Guid almacenId)
        {
            return await _context.Existencias
                .Include(e => e.Almacenes)
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(m => m.ProductoID == productoId && m.AlmacenID == almacenId);
        }

        //Inventarios

        /// <summary>
        /// Obtener Inventario por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Inventario> GetInventarioByIdAsync(Guid id)
        {
            return await _context.Inventarios
                .Include(e => e.Usuarios)
                .FirstOrDefaultAsync(m => m.InventarioID == id);
        }

        /// <summary>
        /// obtener detalle de inventario por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InventarioDetalle> GetInventarioDetalleByIdAsync(Guid id)
        {
            return await _context.InventariosDetalle
                .Include(e => e.Almacenes)
                .Include(e => e.Inventarios)
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(m => m.InventarioDetalleID == id);
        }

        /// <summary>
        /// Obtener lista de detalle de inventario por InventarioID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<InventarioDetalle>> GetInventarioDetalleByInventarioIdAsync(Guid id)
        {
            return await _context.InventariosDetalle
                .Include(e => e.Inventarios)
                .Include(e => e.Almacenes)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Unidades)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Paquete)
                .Include(e => e.Productos)
                .ThenInclude(e => e.Existencias)
                .Where(m => m.InventarioID == id)
                .OrderBy(m => m.Productos.Nombre)
                .ThenBy(m => m.Almacenes.Nombre)
                .ToListAsync();
        }

        //Marcas

        /// <summary>
        /// Obtener marca por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Marca> GetMarcaByIdAsync(Guid id)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(m => m.MarcaID == id);
        }

        /// <summary>
        /// Obtener lista de marcas de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<List<Marca>> GetMarcasByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Marca> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Marcas
                            .Where(p => p.Descripcion.Contains(_pattern) ||
                                        p.Nombre.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(p => p.Descripcion.Contains(_pattern) ||
                                                 p.Nombre.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Marcas;
            }

            return await query.OrderBy(m => m.Nombre).Skip(skip).Take(50).ToListAsync();
        }

        /// <summary>
        /// Obtener marca por nombre.
        /// </summary>
        /// <param name="marca"></param>
        /// <returns></returns>
        public async Task<Marca> GetMarcaByNombreAsync(string marca)
        {
            marca = string.IsNullOrEmpty(marca) ? "" : marca.Trim().ToUpper();

            return await _context.Marcas
                .FirstOrDefaultAsync(p => p.Nombre == marca);
        }

        //Módulos

        /// <summary>
        /// Obtener módulo por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Modulo> GetModuloByIdAsync(Guid id)
        {
            return await _context.Modulos
                .FirstOrDefaultAsync(m => m.ModuloID == id);
        }

        //Municipio

        /// <summary>
        /// Obtener Municipio por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Municipio> GetMunicipioByIdAsync(int id)
        {
            return await _context.Municipios.FindAsync(id);
        }

        /// <summary>
        /// Obtener Municipios por EstadoID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Municipio>> GetMunicipiosByEstadoIdAsync(short id)
        {
            return await _context.Municipios.Where(m => m.EstadoID == id)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        //Paquetes

        /// <summary>
        /// Obtener paquete por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Paquete> GetPaqueteByIdAsync(Guid id)
        {
            return await _context.Paquetes.FindAsync(id);
        }

        /// <summary>
        /// Obtener paquete por id del producto pieza
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Paquete> GetPaqueteByPieceID(Guid id)
        {
            return await _context.Paquetes.FirstOrDefaultAsync(p => p.PiezaProductoID == id);
        }

        //Proveedores

        /// <summary>
        /// Obtener proveedor por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Proveedor> GetProveedorByIdAsync(Guid id)
        {
            return await _context.Proveedores
                .Include(p => p.ProveedorContactos)
                .FirstOrDefaultAsync(p => p.ProveedorID == id);
        }

        /// <summary>
        /// Obtener lista de proveedores de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<Filtro<List<Proveedor>>> GetProveedoresByPatternAsync(Filtro<List<Proveedor>> filtro)
        {
            IQueryable<Proveedor> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Proveedores
                                    .Where(p => p.RFC.Contains(w) ||
                                                p.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.RFC.Contains(w) ||
                                                p.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Proveedores;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query
                .Include(p => p.Municipios)
                .Include(p => p.Municipios.Estados)
                .OrderBy(p => p.RFC)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            return filtro;
        }

        /// <summary>
        /// Obtener proveedor por RFC.
        /// </summary>
        /// <param name="rfc"></param>
        /// <returns></returns>
        public async Task<Proveedor> GetProveedorByRFCAsync(string rfc)
        {
            rfc = string.IsNullOrEmpty(rfc) ? "" : rfc.Trim().ToUpper();

            return await _context.Proveedores
                .FirstOrDefaultAsync(p => p.RFC == rfc);
        }

        //Salidas

        /// <summary>
        /// Obtener Salida por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Salida> GetSalidaByIdAsync(Guid id)
        {
            return await _context.Salidas
                .Include(e => e.Usuarios)
                .ThenInclude(e => e.DatosPersonales)
                .Include(e => e.SalidaTipo)
                .FirstOrDefaultAsync(m => m.SalidaID == id);
        }

        /// <summary>
        /// obtener detalle de salida por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SalidaDetalle> GetSalidaDetalleByIdAsync(Guid id)
        {
            return await _context.SalidasDetalle
                .Include(e => e.Almacenes)
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(m => m.SalidaDetalleID == id);
        }

        /// <summary>
        /// Obtener lista de detalle de salida por SalidaID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<SalidaDetalle>> GetSalidaDetalleBySalidaIdAsync(Guid id)
        {
            return await _context.SalidasDetalle
                .Include(s => s.Almacenes)
                .Include(s => s.Productos)
                .ThenInclude(s => s.Unidades)
                .Include(s => s.Productos)
                .ThenInclude(s => s.Paquete)
                .Include(s => s.Productos)
                .ThenInclude(s => s.Existencias)
                .Include(s => s.Salidas)
                .Where(s => s.SalidaID == id)
                .OrderBy(s => s.Productos.Nombre)
                .ThenBy(s => s.Almacenes.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener tipo de salida por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SalidaTipo> GetSalidaTipoByIdAsync(Guid id)
        {
            return await _context.SalidasTipo
                .FirstOrDefaultAsync(p => p.SalidaTipoID == id);
        }

        //Unidad

        /// <summary>
        /// Obtener unidad por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Unidad> GetUnidadByIdAsync(Guid id)
        {
            return await _context.Unidades.FindAsync(id);
        }

        //Usuarios

        /// <summary>
        /// Obtener usuario por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Usuario> GetUsuarioByIdAsync(Guid id)
        {
            return await _context.Usuarios
                .Include(c => c.DatosPersonales)
                .FirstOrDefaultAsync(c => c.UsuarioID == id);
        }

        /// <summary>
        /// Obtener token de usuario por SessionID.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="usuarioId"></param>
        /// <param name="directorio"></param>
        /// <param name="minutosCerrarSesion"></param>
        /// <returns></returns>
        public async Task<Resultado<Token>> GetTokenBySessionIdAndUsuarioIDAsync(string sessionId,
           string directorio)
        {
            //initialize token class
            var token = new Token()
            {
                Administrador = "",
                HostName = "",
                IPPrivada = "",
                IPPublica = "",
                Nombre = "",
                PrimerApellido = "",
                SegundoApellido = "",
                SessionID = "",
                UsuarioID = Guid.Empty
            };

            //validate session directory
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
            string ruta = $"{directorio}//{sessionId}.config";

            //initialize result class
            Resultado<Token> resultado = new Resultado<Token>
            {
                Datos = null,
                Error = false,
                Mensaje = ""
            };

            Sesion sesion = await _context.Sesiones.FirstOrDefaultAsync(s => s.SessionID == sessionId);

            if (File.Exists(ruta))
            {
                using (var file = new StreamReader(ruta))
                {
                    resultado = JsonConvert.DeserializeObject<Resultado<Token>>(file.ReadToEnd());
                };

                //validate if session exists
                if (sesion == null)
                {
                    resultado.Datos = null;
                    resultado.Error = true;
                    resultado.Mensaje = "Sesión no inicilizada (Redireccionar a página de inicio de aplicación)";
                }

                if (resultado.Error)
                {
                    if (sesion != null)
                    {
                        List<Sesion> sesiones = await _context.Sesiones
                            .Where(s => s.UsuarioID == sesion.UsuarioID).ToListAsync();

                        string rutaSesion = "";

                        if (sesiones != null)
                        {
                            foreach (var s in sesiones)
                            {
                                _context.Remove(s);
                                rutaSesion = $"{directorio}//{s.SessionID}.config";
                                if (File.Exists(rutaSesion))
                                    File.Delete(rutaSesion);
                            }

                            try
                            {
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else
                    {
                        if (File.Exists(ruta))
                            File.Delete(ruta);
                    }

                    return resultado;
                }
            }

            if (!File.Exists(ruta))
            {
                if (sesion == null)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Sesión no inicilizada (Redireccionar a página de inicio de aplicación)"
                    };
                }
                else
                {
                    sesion.Fecha = DateTime.Now;
                    try
                    {
                        _context.Update(sesion);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                    }
                }

                token = await (from u in _context.Usuarios
                               join a in _context.Administradores on u.AdministradorID equals a.AdministradorID
                               where u.UsuarioID == sesion.UsuarioID
                               select new Token()
                               {
                                   Activo = u.Activo,
                                   Administrador = a.AdministradorID,
                                   FechaInicio = u.FechaInicio,
                                   FechaTermino = u.FechaTermino,
                                   Nombre = u.Nombre,
                                   PrimerApellido = u.PrimerApellido,
                                   SegundoApellido = u.SegundoApellido,
                                   SessionID = sessionId,
                                   UsuarioID = u.UsuarioID
                               }).FirstOrDefaultAsync();

                if (token == null)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Usuario Inexistente (identificador del usuario incorrecto)"
                    };
                }

                if (!token.Activo)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Usuario Inactivo (verifique con el administrador del sistema)"
                    };
                }

                var fechaHoraActual = DateTime.Now;
                if (fechaHoraActual < token.FechaInicio)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema comenzará el {token.FechaInicio.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                if (fechaHoraActual > token.FechaTermino)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema expiró el {token.FechaTermino.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                token.UsuariosModulos = await (from m in _context.Modulos
                                               join u in _context.UsuariosModulos
                                               on m.ModuloID equals u.ModuloID
                                               where m.Activo == true &&
                                                     u.PermisoLectura == true &&
                                                     u.UsuarioID == sesion.UsuarioID &&
                                                     m.ModuloPadreID != Guid.Empty
                                               select m.ModuloPadreID).Distinct().ToListAsync();

                if (token.UsuariosModulos == null)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "No tiene privilegios de acceso a la aplicación"
                    };
                }

                token.HostName = GetHostName();
                token.IPPrivada = GetIPPrivada();
                token.IPPublica = GetIPPublica();

                resultado.Datos = token;

                try
                {
                    using (var file = new StreamWriter(ruta, false, System.Text.Encoding.UTF8))
                    {
                        string archivo = JsonConvert.SerializeObject(resultado);
                        file.WriteLine(archivo);
                    };
                }
                catch (Exception)
                {
                    //bitácora de error 
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Error al cargar archivo de sesión"
                    };
                }
            }

            return resultado;
        }

        /// <summary>
        /// configurar token de usuario.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="usuarioId"></param>
        /// <param name="directorio"></param>
        /// <returns></returns>
        public async Task<Resultado<Token>> SetTokenByUsuarioIDAsync(string sessionId,
            Guid usuarioId, string directorio)
        {

            //initialize token class
            var token = new Token()
            {
                Activo = false,
                Administrador = "",
                HostName = "",
                IPPrivada = "",
                IPPublica = "",
                Nombre = "",
                PrimerApellido = "",
                SegundoApellido = "",
                SessionID = "",
                UsuarioID = Guid.Empty
            };

            //validate session directory
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
            string ruta = $"{directorio}//{sessionId}.config";

            //initialize result class
            Resultado<Token> resultado = new Resultado<Token>
            {
                Datos = null,
                Error = false,
                Mensaje = ""
            };

            if (File.Exists(ruta))
            {
                bool iniciarToken = false;

                using (var file = new StreamReader(ruta))
                {
                    resultado = JsonConvert.DeserializeObject<Resultado<Token>>(file.ReadToEnd());
                };

                if (resultado == null || resultado.Error)
                {
                    iniciarToken = true;
                }
                else
                {
                    Sesion sesion = await _context.Sesiones
                        .FirstOrDefaultAsync(s => s.SessionID == resultado.Datos.SessionID);

                    //validate if session exists
                    if (sesion == null)
                    {
                        iniciarToken = true;
                    }
                    else
                    {
                        if (resultado.Datos.UsuarioID != usuarioId ||
                            sesion.UsuarioID != usuarioId)
                        {
                            iniciarToken = true;
                        }
                    }
                }

                if (iniciarToken)
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch (Exception)
                    {
                        //bitácora de error al eliminar el archivo
                    }
                }
            }

            if (!File.Exists(ruta))
            {
                var sesion = await _context.Sesiones.FirstOrDefaultAsync(s => s.SessionID == sessionId);
                if (sesion != null)
                {
                    _context.Remove(sesion);
                }
                else
                {
                    //delete the last records by user
                    var sesiones = _context.Sesiones.Where(s => s.UsuarioID == usuarioId &&
                                                                s.SessionID != sessionId);
                    if (sesiones != null)
                    {
                        string rutaSesion = "";
                        foreach (var s in sesiones)
                        {
                            _context.Remove(s);
                            rutaSesion = $"{directorio}//{s.SessionID}.config";
                            if (File.Exists(rutaSesion))
                                File.Delete(rutaSesion);
                        }
                    }
                }

                Guid sesionId = Guid.NewGuid();

                _context.Add(new Sesion()
                {
                    Fecha = DateTime.Now,
                    SesionID = sesionId,
                    SessionID = sessionId,
                    UsuarioID = usuarioId
                });

                token = await (from u in _context.Usuarios
                               join a in _context.Administradores on u.AdministradorID equals a.AdministradorID
                               where u.UsuarioID == usuarioId
                               select new Token()
                               {
                                   Activo = u.Activo,
                                   Administrador = a.AdministradorID,
                                   FechaInicio = u.FechaInicio,
                                   FechaTermino = u.FechaTermino,
                                   Nombre = u.Nombre,
                                   PrimerApellido = u.PrimerApellido,
                                   SegundoApellido = u.SegundoApellido,
                                   SessionID = sessionId,
                                   UsuarioID = u.UsuarioID
                               }).FirstOrDefaultAsync();

                if (token == null)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Usuario Inexistente (identificador del usuario incorrecto)"
                    };
                }

                if (!token.Activo)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Usuario Inactivo (verifique con el administrador del sistema)"
                    };
                }

                var fechaHoraActual = DateTime.Now;
                if (fechaHoraActual < token.FechaInicio)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema comenzará el {token.FechaInicio.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                if (fechaHoraActual > token.FechaTermino)
                {
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema expiró el {token.FechaTermino.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                if (token.Administrador != "SA" && token.Administrador != "GA")
                {
                    token.UsuariosModulos = await (from m in _context.Modulos
                                                   join u in _context.UsuariosModulos
                                                   on m.ModuloID equals u.ModuloID
                                                   where m.Activo == true &&
                                                         u.PermisoLectura == true &&
                                                         u.UsuarioID == usuarioId &&
                                                         m.ModuloPadreID != Guid.Empty
                                                   select m.ModuloPadreID).Distinct().ToListAsync();

                    if (token.UsuariosModulos == null)
                    {
                        return new Resultado<Token>()
                        {
                            Datos = null,
                            Error = true,
                            Mensaje = "No tiene privilegios de acceso a la aplicación"
                        };
                    }
                }

                token.HostName = GetHostName();
                token.IPPrivada = GetIPPrivada();
                token.IPPublica = GetIPPublica();

                resultado.Datos = token;

                //actualizar fecha de último acceso a la aplicación por el usuario.
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario != null)
                {
                    usuario.FechaUltimoAcceso = DateTime.Now;
                    _context.Update(usuario);
                }

                try
                {
                    using (var file = new StreamWriter(ruta, false, System.Text.Encoding.UTF8))
                    {
                        string archivo = JsonConvert.SerializeObject(resultado);
                        file.WriteLine(archivo);
                    };

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    //bitácora de error 
                    return new Resultado<Token>()
                    {
                        Datos = null,
                        Error = true,
                        Mensaje = "Error al cargar archivo de sesión"
                    };
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtener lista de usuarios de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<List<Usuario>> GetUsuariosByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Usuario> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Usuarios
                            .Where(c => c.PrimerApellido.Contains(_pattern) ||
                                        c.SegundoApellido.Contains(_pattern) ||
                                        c.Nombre.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(c => c.PrimerApellido.Contains(_pattern) ||
                                                 c.SegundoApellido.Contains(_pattern) ||
                                                 c.Nombre.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Usuarios;
            }

            return await query.OrderBy(e => e.NombreCompleto).Skip(skip).Take(50).ToListAsync();
        }

        /// <summary>
        /// Obtener permisos de módulo por usuario
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="moduloId"></param>
        /// <returns></returns>
        public async Task<UsuarioModulo> GetUsuarioModuloByUsuarioAndModuloIDAsync(Guid usuarioId, Guid moduloId)
        {
            return await _context.UsuariosModulos.FirstOrDefaultAsync(u => u.UsuarioID == usuarioId &&
                                                                     u.ModuloID == moduloId);
        }

        public async Task<List<Guid>> GetModulesByUsuarioIDAndModuloPadreID(Guid usuarioId, Guid moduloPadreId)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                return null;
            }

            if (usuario.AdministradorID == "SA" || usuario.AdministradorID == "GA")
            {
                return await _context.Modulos
                    .Where(m => m.ModuloPadreID == moduloPadreId && m.Activo == true)
                    .Select(m => m.ModuloID).ToListAsync();
            }
            else
            {
                return await (from m in _context.Modulos
                              join u in _context.UsuariosModulos on m.ModuloID equals u.ModuloID
                              where m.Activo == true && u.PermisoLectura == true &&
                                    m.ModuloPadreID == moduloPadreId && u.UsuarioID == usuarioId
                              select m.ModuloID).Distinct().ToListAsync();
            }
        }
        
    }
}
