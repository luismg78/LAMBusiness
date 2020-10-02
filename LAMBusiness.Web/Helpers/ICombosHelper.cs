namespace LAMBusiness.Web.Helpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboEstadosAsync();
        Task<IEnumerable<SelectListItem>> GetComboMunicipiosAsync(short estadoId);
        Task<IEnumerable<SelectListItem>> GetComboTasaImpuestosAsync();
        Task<IEnumerable<SelectListItem>> GetComboUnidadesAsync();
    }
}