namespace LAMBusiness.Web.Interfaces
{
    using System.Threading.Tasks;
    using Models.ViewModels;
    using Shared.Aplicacion;

    public interface IDashboard
    {
        public Task GuardarEstadisticaDeMovimientoAsync(EstadisticaMovimientoViewModel estadisticaMovimiento);
    }
}
