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
    public class RetirosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("AA3E3482-0EC5-40B8-8C48-7E567DA135F6");

        public RetirosController(DataContext context,
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
        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Index", "Home");
            }

            var retiros = (from r in _context.RetirosCaja
                           join u in _context.Usuarios on r.UsuarioID equals u.UsuarioID
                           where u.AdministradorID != "SA" &&
                                 r.VentaCierreID == Guid.Empty
                           orderby r.Fecha descending
                           select new RetirosViewModel()
                           {
                               UsuarioID = u.UsuarioID,
                               Fecha = r.Fecha,
                               Importe = r.Importe,
                               Nombre = u.Nombre,
                               PrimerApellido = u.PrimerApellido,
                               SegundoApellido = u.SegundoApellido,
                               VentaPendiente = r.VentaCierreID,
                           });

            var filtro = new Filtro<List<RetirosViewModel>>()
            {
                Datos = await retiros.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await retiros.CountAsync(),
                Skip = 0
            };

            return View(filtro);

        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<RetirosViewModel>> filtro, bool todos)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<RetirosViewModel> query = (from r in _context.RetirosCaja
                                                  join u in _context.Usuarios on r.UsuarioID equals u.UsuarioID
                                                  where u.AdministradorID != "SA"
                                                  orderby r.Fecha descending
                                                  select new RetirosViewModel()
                                                  {
                                                      UsuarioID = u.UsuarioID,
                                                      Fecha = r.Fecha,
                                                      Importe = r.Importe,
                                                      Nombre = u.Nombre,
                                                      PrimerApellido = u.PrimerApellido,
                                                      SegundoApellido = u.SegundoApellido,
                                                      VentaPendiente = r.VentaCierreID,
                                                  });

            if (!todos)
                query = query.Where(q => q.VentaPendiente == Guid.Empty);

            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                            query = query.Where(c => c.Nombre.Contains(w) ||
                                                     c.PrimerApellido.Contains(w) ||
                                                     c.SegundoApellido.Contains(w));
                    }
                }
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderByDescending(q => q.Fecha)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = permisosModulo.PermisoEscritura;
            filtro.PermisoImprimir = permisosModulo.PermisoImprimir;
            filtro.PermisoLectura = permisosModulo.PermisoLectura;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<RetirosViewModel>>>(ViewData, filtro)
            };
        }
    }
}
