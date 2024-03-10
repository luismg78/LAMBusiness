using LAMBusiness.Contextos;
using LAMBusiness.Shared.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace LAMBusiness.Backend
{
    public class Almacenes
    {
        private readonly DataContext _contexto;

        public Almacenes(DataContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Obtener almacén por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Almacen (clase)</returns>
        public async Task<Almacen?> ObtenerRegistroPorIdAsync(Guid id)
        {
            var almacen = await _contexto.Almacenes.FirstOrDefaultAsync(p => p.AlmacenID == id);
            return almacen;
        }
    }
}
