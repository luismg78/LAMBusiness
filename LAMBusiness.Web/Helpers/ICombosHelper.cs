namespace LAMBusiness.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboAlmacenesAsync();
        Task<IEnumerable<SelectListItem>> GetComboAdministradoresAsync(Guid usuarioId);
        Task<IEnumerable<SelectListItem>> GetComboEstadosAsync();
        Task<IEnumerable<SelectListItem>> GetComboEstadosCivilesAsync();
        Task<IEnumerable<SelectListItem>> GetComboGenerosAsync();
        Task<IEnumerable<SelectListItem>> GetComboMarcasAsync();
        Task<IEnumerable<SelectListItem>> GetComboModulosAsync();
        Task<IEnumerable<SelectListItem>> GetComboMunicipiosAsync(short estadoId);
        Task<IEnumerable<SelectListItem>> GetComboPuestosAsync();
        Task<IEnumerable<SelectListItem>> GetComboSalidasTipoAsync();
        Task<IEnumerable<SelectListItem>> GetComboTasaImpuestosAsync();
        Task<IEnumerable<SelectListItem>> GetComboUnidadesAsync();
    }
}