namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using LAMBusiness.Contextos;

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        //Administradores
        public async Task<IEnumerable<SelectListItem>> GetComboAdministradoresAsync(Guid usuarioId)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario != null)
            {
                List<string> administradorTipo = new List<string>();
                administradorTipo.Add("SA");
                if (usuario.AdministradorID == "NA")
                    administradorTipo.Add("GA");

                list = await _context.Administradores
                    .Where(a => !administradorTipo.Contains(a.AdministradorID))
                    .Select(a => new SelectListItem()
                    {
                        Text = a.Nombre,
                        Value = a.AdministradorID.ToString()
                    }).OrderBy(e => e.Text).ToListAsync();
            }

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Tipo Administrador]",
                Value = ""
            });

            return list;
        }

        //Estados
        public async Task<IEnumerable<SelectListItem>> GetComboEstadosAsync()
        {
            var list = await _context.Estados.Select(e => new SelectListItem()
            {
                Text = e.Nombre,
                Value = e.EstadoID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Estado]",
                Value = "0"
            });

            return list;
        }

        //Estados Civiles
        public async Task<IEnumerable<SelectListItem>> GetComboEstadosCivilesAsync()
        {
            var list = await _context.EstadosCiviles.Select(e => new SelectListItem()
            {
                Text = e.Nombre,
                Value = e.EstadoCivilID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Estado Civil]",
                Value = "0"
            });

            return list;
        }

        //Géneros
        public async Task<IEnumerable<SelectListItem>> GetComboGenerosAsync()
        {
            var list = await _context.Generos.Select(e => new SelectListItem()
            {
                Text = e.Nombre,
                Value = e.GeneroID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Género]",
                Value = ""
            });

            return list;
        }

        //Módulos (Padres)
        public async Task<IEnumerable<SelectListItem>> GetComboModulosAsync()
        {
            var list = await _context.Modulos
                .Where(m => m.Activo == true && m.ModuloPadreID == Guid.Empty)
                .Select(m => new SelectListItem()
            {
                Text = m.Descripcion,
                Value = m.ModuloID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Módulo Padre]",
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
                Text = m.Nombre,
                Value = m.MunicipioID.ToString()
            }).OrderBy(m => m.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Municipio]",
                Value = "0"
            });

            return list;
        }

        //Puestos
        public async Task<IEnumerable<SelectListItem>> GetComboPuestosAsync()
        {
            var list = await _context.Puestos.Select(e => new SelectListItem()
            {
                Text = e.Nombre,
                Value = e.PuestoID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Puesto]",
                Value = ""
            });

            return list;
        }

        //Salidas
        public async Task<IEnumerable<SelectListItem>> GetComboSalidasTipoAsync()
        {
            var list = await _context.SalidasTipo.Select(e => new SelectListItem()
            {
                Text = e.Nombre,
                Value = e.SalidaTipoID.ToString()
            }).OrderBy(e => e.Text).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "[Seleccionar Tipo de salida]",
                Value = "0"
            });

            return list;
        }

        //Tasas de Impuestos
        public async Task<IEnumerable<SelectListItem>> GetComboTasaImpuestosAsync()
        {
            var list = await _context.TasasImpuestos.Select(t => new SelectListItem()
            {
                Text = t.Nombre,
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
                Text = u.Nombre,
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
