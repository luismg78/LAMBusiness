namespace LAMBusiness.Web.Controllers
{
    using Helpers;
    using LAMBusiness.Contextos;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class DashboardController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("50DECAF3-BFA7-42BB-B544-960178A93342");

        public DashboardController(DataContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IGetHelper getHelper,
            IConfiguration configuration)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
            _configuration = configuration;
        }

        public IActionResult Movement()
        {
            return View();
        }

        public async Task<IActionResult> GetMovement()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "dashboard");
            if (validateToken != null) { return Json(new { error = true, mensaje = "Ingrese de nuevo sus credenciales." }); }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
                return Json(new { error = true, mensaje = "Lo sentimos, su cuenta no tiene privilegios para consultar estadísticas de movimientos." });

            List<int> años = new List<int>() { 2022 };
            EstadisticaMovimientoChartViewModel estadisticaMovimiento = await _getHelper.GetMovementsDashboardAsync(años);

            if (estadisticaMovimiento != null)
            {
                return Json(
                    new
                    {
                        datos = estadisticaMovimiento.TotalVentasPorAño,
                        error = false
                    });
            }

            return Json(new { error = true, mensaje = "Por el momento no tenemos registros de ventas en el sistema." });
        }
    }
}
