namespace LAMBusiness.Web.Models.ViewModels
{
    using System.Collections.Generic;

    public class EstadisticaMovimientoChartViewModel
    {
        public List<TotalVentasPorAñoViewModel> TotalVentasPorAño { get; set; }
    }

    public class TotalVentasPorAñoViewModel
    {
        public string Name { get; set; }
        public List<decimal> Data { get; set; }
    }
}
