namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Contacto;
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

        public async Task<IActionResult> Movimientos()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "dashboard");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            return View();
        }
    }
}
