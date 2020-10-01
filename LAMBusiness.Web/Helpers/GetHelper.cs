namespace LAMBusiness.Web.Helpers
{
    using Data;
    using LAMBusiness.Shared.Catalogo;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class GetHelper : IGetHelper
    {
        private readonly DataContext _context;

        public GetHelper(DataContext context)
        {
            _context = context;
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
