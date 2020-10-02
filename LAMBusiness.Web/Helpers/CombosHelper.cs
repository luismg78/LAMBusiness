namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Data;

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        //Estados
        public async Task<IEnumerable<SelectListItem>> GetComboEstadosAsync()
        {
            var list = await _context.Estados.Select(e => new SelectListItem()
            {
                Text = e.EstadoDescripcion,
                Value = e.EstadoID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Estado]",
                Value = "0"
            });

            return list;
        }

        //Municipios
        public async Task<IEnumerable<SelectListItem>> GetComboMunicipiosAsync(short estadoId)
        {
            var list = await _context.Municipios
                .Where(m=> m.EstadoID == estadoId)
                .Select(m => new SelectListItem()
            {
                Text = m.MunicipioDescripcion,
                Value = m.MunicipioID.ToString()
            }).OrderBy(m => m.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Municipio]",
                Value = "0"
            });

            return list;
        }

        //Tasas de Impuestos
        public async Task<IEnumerable<SelectListItem>> GetComboTasaImpuestosAsync()
        {
            var list = await _context.TasasImpuestos.Select(t => new SelectListItem()
            {
                Text = t.Tasa,
                Value = t.TasaID.ToString()
            }).OrderBy(t => t.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Tasa de Impuesto]",
                Value = Guid.Empty.ToString()
            });

            return list;
        }

        //Unidades
        public async Task<IEnumerable<SelectListItem>> GetComboUnidadesAsync()
        {
            var list = await _context.Unidades.Select(u => new SelectListItem()
            {
                Text = u.UnidadNombre,
                Value = u.UnidadID.ToString()
            }).OrderBy(t => t.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Unidad]",
                Value = Guid.Empty.ToString()
            });

            return list;
        }
    }
}
