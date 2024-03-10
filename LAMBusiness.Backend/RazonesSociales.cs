using LAMBusiness.Contextos;
using LAMBusiness.Shared.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace LAMBusiness.Backend
{
    public class RazonesSociales
    {
        private readonly DataContext _contexto;
        public RazonesSociales(DataContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Obtener razón social por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RazonSocial (clase)</returns>
        public async Task<RazonSocial?> ObtenerRegistroPorIdAsync(Guid id)
        {
            var razonSocial = await _contexto.RazonesSociales.FirstOrDefaultAsync(p => p.RazonSocialId == id);
            return razonSocial;
        }
    }
}
