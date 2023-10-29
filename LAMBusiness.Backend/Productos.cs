using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace LAMBusiness.Backend
{
    public class Productos
    {
        private readonly DataContext _contexto;

        public Productos(DataContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Obtener producto por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>String</returns>
        public async Task<Producto?> ObtenerRegistroPorIdAsync(Guid id)
        {
            var registros = FiltrarRegistro();

            return await registros.FirstOrDefaultAsync(p => p.ProductoID == id);
        }

        /// <summary>
        /// Obtener producto por código
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public async Task<Producto?> ObtenerRegistroPorCodigoAsync(string codigo)
        {
            return await _contexto.Productos
                .Include(p => p.Existencias)
                .ThenInclude(p => p.Almacenes)
                .Include(p => p.Marcas)
                .Include(p => p.Paquete)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades)
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        /// <summary>
        /// Obtener lista de productos de acuerdo al patrón solicitado
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns>Filtro<List<Producto>></returns>
        public async Task<Filtro<List<Producto>>> ObtenerRegistros(Filtro<List<Producto>> filtro)
        {
            IQueryable<Producto> registros = FiltrarRegistro();

            if (!string.IsNullOrEmpty(filtro.Patron))
                registros = FiltrarPorPalabra(registros, filtro.Patron);


            filtro.Datos = await registros.OrderBy(r => r.Codigo).Skip(filtro.Skip).Take(100).ToListAsync();
            filtro.Registros = await registros.CountAsync();

            return filtro;
        }

        private static IQueryable<Producto> FiltrarPorPalabra(IQueryable<Producto> datos, string patron)
        {
            if (string.IsNullOrEmpty(patron))
                return datos;

            var palabras = patron.Trim().Split(' ');
            if (palabras.Length > 0)
            {
                foreach (var palabra in palabras)
                {
                    string p = palabra.Trim();
                    if (p != string.Empty)
                    {
                        datos = datos.Where(d => d.Codigo.Contains(p)
                                              || d.Nombre.Contains(p));
                    }
                }
            }

            return datos;
        }

        private IQueryable<Producto> FiltrarRegistro()
        {
            return _contexto.Productos
                .Include(p => p.Existencias)
                .ThenInclude(p => p.Almacenes)
                .Include(p => p.Marcas)
                .Include(p => p.Paquete)
                .Include(p => p.TasasImpuestos)
                .Include(p => p.Unidades);
        }
    }
}
