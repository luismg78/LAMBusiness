﻿    namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;
    using LAMBusiness.Shared.Contacto;
    using LAMBusiness.Shared.Movimiento;

    public class GetHelper : IGetHelper
    {
        private readonly DataContext _context;

        public GetHelper(DataContext context)
        {
            _context = context;
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
        /// <param name="marca"></param>
        /// <returns></returns>
        public async Task<Proveedor> GetProveedorByRFCAsync(string rfc)
        {
            rfc = string.IsNullOrEmpty(rfc) ? "" : rfc.Trim().ToUpper();

            return await _context.Proveedores
                .FirstOrDefaultAsync(p => p.RFC == rfc);
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

    }
}
