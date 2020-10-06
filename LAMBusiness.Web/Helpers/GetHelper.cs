namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Shared.Catalogo;
    using LAMBusiness.Shared.Contacto;

    public class GetHelper : IGetHelper
    {
        private readonly DataContext _context;

        public GetHelper(DataContext context)
        {
            _context = context;
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
            return await _context.Productos.FindAsync(id);
        }

        /// <summary>
        /// Obtener producto por código
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public async Task<Producto> GetProductByCodeAsync(string codigo)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.Codigo == codigo);
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
