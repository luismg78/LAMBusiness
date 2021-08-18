    namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;
    using Shared.Contacto;
    using Shared.Movimiento;
    using Shared.Aplicacion;
    using System.IO;
    using Newtonsoft.Json;

    public class GetHelper : IGetHelper
    {
        private readonly DataContext _context;

        public GetHelper(DataContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(p => p.AlmacenNombre == almacen);
        }

        /// <summary>
        /// Obtener lista de almacenes de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<List<Almacen>> GetAlmacenesByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Almacen> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Almacenes
                                .Where(p => p.AlmacenNombre.Contains(_pattern) ||
                                            p.AlmacenDescripcion.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(p => p.AlmacenNombre.Contains(_pattern) ||
                                                 p.AlmacenDescripcion.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Almacenes;
            }

            return await query.OrderBy(e => e.AlmacenNombre).Skip(skip).Take(50).ToListAsync();
        }

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

        //Colaborador

        /// <summary>
        /// Obtener colaborador por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Colaborador> GetColaboradorByIdAsync(Guid id)
        {
            return await _context.Colaboradores
                .Include(c => c.Estados)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Generos)
                .Include(c => c.Municipios)
                .Include(c => c.Puestos)
                .FirstOrDefaultAsync(p => p.ColaboradorID == id);
        }

        /// <summary>
        /// Obtener lista de colaboradores de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<List<Colaborador>> GetColaboradoresByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Colaborador> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Colaboradores
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
                query = _context.Colaboradores;
            }

            return await query.OrderBy(e => e.CURP).Skip(skip).Take(50).ToListAsync();
        }

        /// <summary>
        /// Obtener lista de colaboradores que no tienen cuenta de usuario de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<List<Colaborador>> GetColaboradoresSinCuentaUsuarioByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Colaborador> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Colaboradores
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
                query = _context.Colaboradores;
            }

            var usuarios = await _context.Usuarios.Select(u => u.ColaboradorID).ToListAsync();

            return await query.Where(q => !usuarios.Contains(q.ColaboradorID))
                .OrderBy(e => e.CURP).Skip(skip).Take(50).ToListAsync();
        }


        /// <summary>
        /// Obtener proveedor por CURP.
        /// </summary>
        /// <param name="curp"></param>
        /// <returns></returns>
        public async Task<Colaborador> GetColaboradorByCURPAsync(string curp)
        {
            curp = string.IsNullOrEmpty(curp) ? "" : curp.Trim().ToUpper();

            return await _context.Colaboradores
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
                .OrderBy(m => m.Productos.ProductoNombre)
                .ThenBy(m => m.Almacenes.AlmacenNombre)
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
                            .Where(p => p.MarcaDescripcion.Contains(_pattern) ||
                                        p.MarcaNombre.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(p => p.MarcaDescripcion.Contains(_pattern) ||
                                                 p.MarcaNombre.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Marcas;
            }

            return await query.OrderBy(m => m.MarcaNombre).Skip(skip).Take(50).ToListAsync();
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
                .FirstOrDefaultAsync(p => p.MarcaNombre == marca);
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
                .OrderBy(m => m.MunicipioDescripcion)
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
        public async Task<Paquete> GetPaqueteByPieceID(Guid id) {
            return await _context.Paquetes.FirstOrDefaultAsync(p => p.PiezaProductoID == id);
        }

        //Productos

        /// <summary>
        /// Obtener producto por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>String</returns>
        public async Task<Producto> GetProductByIdAsync(Guid id)
        {
            return await _context.Productos
                .Include(p => p.Existencias)
                .Include(p => p.Paquete)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(p => p.ProductoID == id);
        }

        /// <summary>
        /// Obtener producto por código
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public async Task<Producto> GetProductByCodeAsync(string codigo)
        {
            return await _context.Productos
                .Include(p => p.Existencias)
                .Include(p => p.Paquete)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        /// <summary>
        /// Obtener lista de productos de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<List<Producto>> GetProductosByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Producto> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Productos
                                .Where(p => p.Codigo.Contains(_pattern) ||
                                            p.ProductoNombre.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(p => p.Codigo.Contains(_pattern) ||
                                                 p.ProductoNombre.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Productos;
            }

            return await query.OrderBy(e => e.ProductoNombre).Skip(skip).Take(50).ToListAsync();
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
        public async Task<List<Proveedor>> GetProveedoresByPatternAsync(string pattern, int skip)
        {
            string[] patterns = pattern.Trim().Split(' ');
            IQueryable<Proveedor> query = null;
            foreach (var p in patterns)
            {
                string _pattern = p.Trim();
                if (_pattern != "")
                {
                    if (query == null)
                    {
                        query = _context.Proveedores
                            .Where(p => p.RFC.Contains(_pattern) ||
                                        p.Nombre.Contains(_pattern));
                    }
                    else
                    {
                        query = query.Where(p => p.RFC.Contains(_pattern) ||
                                                 p.Nombre.Contains(_pattern));
                    }
                }
            }

            if (query == null)
            {
                query = _context.Proveedores;
            }

            return await query.OrderBy(e => e.RFC).Skip(skip).Take(50).ToListAsync();
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
                .OrderBy(s => s.Productos.ProductoNombre)
                .ThenBy(s => s.Almacenes.AlmacenNombre)
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
                .Include(c => c.Colaborador)
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
                ColaboradorID = Guid.Empty,
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
            Resultado<Token> resultado = new Resultado<Token> { 
                Contenido = null,
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
                    resultado.Contenido = null;
                    resultado.Error = true;
                    resultado.Mensaje = "Sesión no inicilizada (Redireccionar a página de inicio de aplicación)";
                }
                
                if (resultado.Error)
                {
                    if(sesion != null)
                    {
                        List<Sesion> sesiones = await _context.Sesiones
                            .Where(s => s.UsuarioID == sesion.UsuarioID).ToListAsync();
                        
                        string rutaSesion = "";

                        if (sesiones != null)
                        {
                            foreach(var s in sesiones)
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
                        if(File.Exists(ruta))
                            File.Delete(ruta);
                    }

                    return resultado;
                }
            }

            if (!File.Exists(ruta))
            {
                if(sesion == null)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
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
                               join c in _context.Colaboradores on u.ColaboradorID equals c.ColaboradorID
                               join a in _context.Administradores on u.AdministradorID equals a.AdministradorID
                               where u.UsuarioID == sesion.UsuarioID
                               select new Token()
                               {
                                   Activo = u.Activo,
                                   Administrador = a.AdministradorID,
                                   ColaboradorID = c.ColaboradorID,
                                   FechaInicio = u.FechaInicio,
                                   FechaTermino = u.FechaTermino,
                                   Nombre = c.Nombre,
                                   PrimerApellido = c.PrimerApellido,
                                   SegundoApellido = c.SegundoApellido,
                                   SessionID = sessionId,
                                   UsuarioID = u.UsuarioID
                               }).FirstOrDefaultAsync();

                if (token == null)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = "Usuario Inexistente (identificador del usuario incorrecto)"
                    };
                }

                if (!token.Activo)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = "Usuario Inactivo (verifique con el administrador del sistema)"
                    };
                }

                var fechaHoraActual = DateTime.Now;
                if (fechaHoraActual < token.FechaInicio)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema comenzará el {token.FechaInicio.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                if (fechaHoraActual > token.FechaTermino)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
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
                    return new Resultado<Token>() {
                        Contenido = null,
                        Error = true,
                        Mensaje = "No tiene privilegios de acceso a la aplicación"
                    };
                }

                resultado.Contenido = token;

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
                        Contenido = null,
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
                Administrador = "",
                ColaboradorID = Guid.Empty,
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
                Contenido = null,
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
                        .FirstOrDefaultAsync(s => s.SessionID == resultado.Contenido.SessionID);

                    //validate if session exists
                    if (sesion == null)
                    {
                        iniciarToken = true;
                    }
                    else
                    {
                        if (resultado.Contenido.UsuarioID != usuarioId ||
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

                _context.Add(new Sesion() { 
                    Fecha = DateTime.Now,
                    SesionID = sesionId,
                    SessionID = sessionId,
                    UsuarioID = usuarioId
                });

                token = await (from u in _context.Usuarios
                               join c in _context.Colaboradores on u.ColaboradorID equals c.ColaboradorID
                               join a in _context.Administradores on u.AdministradorID equals a.AdministradorID
                               where u.UsuarioID == usuarioId
                               select new Token()
                               {
                                   Activo = u.Activo,
                                   Administrador = a.AdministradorID,
                                   ColaboradorID = c.ColaboradorID,
                                   FechaInicio = u.FechaInicio,
                                   FechaTermino = u.FechaTermino,
                                   Nombre = c.Nombre,
                                   PrimerApellido = c.PrimerApellido,
                                   SegundoApellido = c.SegundoApellido,
                                   SessionID = sessionId,
                                   UsuarioID = u.UsuarioID
                               }).FirstOrDefaultAsync();

                if (token == null)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = "Usuario Inexistente (identificador del usuario incorrecto)"
                    };
                }

                if (!token.Activo)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = "Usuario Inactivo (verifique con el administrador del sistema)"
                    };
                }

                var fechaHoraActual = DateTime.Now;
                if (fechaHoraActual < token.FechaInicio)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
                        Error = true,
                        Mensaje = $"Su periodo de acceso al sistema comenzará el {token.FechaInicio.ToString("dddd, dd \\de MMMM \\de yyyy a la\\s HH:mm")}"
                    };
                }

                if (fechaHoraActual > token.FechaTermino)
                {
                    return new Resultado<Token>()
                    {
                        Contenido = null,
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
                            Contenido = null,
                            Error = true,
                            Mensaje = "No tiene privilegios de acceso a la aplicación"
                        };
                    }
                }

                resultado.Contenido = token;
                
                //actualizar fecha de último acceso a la aplicación por el usuario.
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if(usuario != null)
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
                        Contenido = null,
                        Error = true,
                        Mensaje = "Error al cargar archivo de sesión"
                    };
                }
            }            

            return resultado;
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
            if(usuario == null)
            {
                return null;
            }

            if(usuario.AdministradorID == "SA" || usuario.AdministradorID == "GA")
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
