namespace LAMBusiness.Web.Helpers
{
    using Data;
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

        /// <summary>
        /// Obtener el código del producto por medio del ID.
        /// </summary>
        /// <param name="productoID"></param>
        /// <returns>String</returns>
        public async Task<string> GetCodigoProductoAsync(Guid productoID)
        {
            var producto = await _context.Productos.FindAsync(productoID);
            if (producto == null)
            {
                return "";
            }
            else
            {
                return producto.Codigo.Trim().ToUpper();
            }
        }

        public async Task<Guid> GetProductoIDAsync(string codigo)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (producto == null)
            {
                return Guid.Empty;
            }
            else
            {
                return producto.ProductoID;
            }
        }

    }
}
