namespace LAMBusiness.Web.Controllers
{
    using Data;
    using Helpers;
    using LAMBusiness.Shared.Movimiento;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class RetirosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("AA3E3482-0EC5-40B8-8C48-7E567DA135F6");

        public RetirosController(DataContext context,
            IGetHelper getHelper,
            IConfiguration configuration)
        {
            _context = context;
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

            var f = DateTime.Now;
            var fechaInicio = new DateTime(f.Year, f.Month - 1, 1, 0, 0, 0);
            var fechaFin = new DateTime(f.Year, f.Month, f.Day, 23, 59, 59);

            var retiros = (from r in _context.RetirosCaja
                           join u in _context.Usuarios on r.UsuarioID equals u.UsuarioID
                           where u.AdministradorID != "SA" &&
                                 r.Fecha >= fechaInicio && r.Fecha <= fechaFin &&
                                 (r.VentaCierreID == Guid.Empty || r.VentaCierreID == null)
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

            var administrador = await EsAdministradorAsync();
            if (!administrador)
                retiros = retiros.Where(c => c.UsuarioID == token.UsuarioID);

            var filtro = new Filtro<List<RetirosViewModel>>()
            {
                Datos = await retiros.OrderByDescending(q => q.Fecha)
                .ThenByDescending(u => u.PrimerApellido)
                .ThenByDescending(u => u.SegundoApellido)
                .ThenByDescending(u => u.Nombre)
                .Take(50).ToListAsync(),
                Patron = "",
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
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
                return null;

            var esAdministrador = await EsAdministradorAsync();

            IQueryable<RetirosViewModel> query = (from r in _context.RetirosCaja
                                                  join u in _context.Usuarios on r.UsuarioID equals u.UsuarioID
                                                  where u.AdministradorID != "SA"
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
            if (todos)
            {
                var fi = filtro.FechaInicio;
                var ff = filtro.FechaFin;
                var fechaInicio = new DateTime(fi.Year, fi.Month, fi.Day, 0, 0, 0);
                var fechaFin = new DateTime(ff.Year, ff.Month, ff.Day, 23, 59, 59);

                query = query.Where(r => r.Fecha >= fechaInicio && r.Fecha <= fechaFin);
            }
            else
                query = query.Where(q => q.VentaPendiente == Guid.Empty || q.VentaPendiente == null);

            if (!esAdministrador)
                query = query.Where(c => c.UsuarioID == token.UsuarioID);


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
                .ThenByDescending(u => u.PrimerApellido)
                .ThenByDescending(u => u.SegundoApellido)
                .ThenByDescending(u => u.Nombre)
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

        private async Task<bool> EsAdministradorAsync()
        {
            return await _context.Usuarios
                .Where(u => u.UsuarioID == token.UsuarioID && (u.AdministradorID == "SA" || u.AdministradorID == "GA"))
                .AnyAsync();
        }
    }
}
