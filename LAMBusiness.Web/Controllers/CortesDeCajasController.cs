using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Movimiento;
using LAMBusiness.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAMBusiness.Web.Controllers
{
    public class CortesDeCajasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("7b848778-36f0-4254-b2a1-b822c9ab87b3");

        public CortesDeCajasController(DataContext context,
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

            var cierres = _context.VentasCierre
                .Include(u => u.Usuarios)
                .Include(u => u.UsuarioCaja)
                .Where(u => u.Fecha >= fechaInicio && u.Fecha <= fechaFin && u.Usuarios.AdministradorID != "SA");

            var administrador = await EsAdministradorAsync();
            if (!administrador)
                cierres = cierres.Where(c => c.UsuarioID == token.UsuarioID);

            var filtro = new Filtro<List<VentaCierre>>()
            {
                Datos = await cierres.Take(50)
                .OrderByDescending(u => u.Fecha)
                .ThenByDescending(u => u.Usuarios.PrimerApellido)
                .ThenByDescending(u => u.Usuarios.SegundoApellido)
                .ThenByDescending(u => u.Usuarios.Nombre)
                .ToListAsync(),
                Patron = "",
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                PermisoEscritura = administrador,
                PermisoImprimir = administrador,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await cierres.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<VentaCierre>> filtro, bool todos)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
                return null;

            var esAdministrador = await EsAdministradorAsync();
            IQueryable<VentaCierre> query;

            if (todos)
            {
                var fi = filtro.FechaInicio;
                var ff = filtro.FechaFin;
                var fechaInicio = new DateTime(fi.Year, fi.Month, fi.Day, 0, 0, 0);
                var fechaFin = new DateTime(ff.Year, ff.Month, ff.Day, 23, 59, 59);

                query = _context.VentasCierre
                    .Include(u => u.Usuarios)
                    .Include(u => u.UsuarioCaja)
                    .Where(u => u.Fecha >= fechaInicio && u.Fecha <= fechaFin
                             && u.Usuarios.AdministradorID != "SA")
                    .AsQueryable();

                filtro.PermisoLectura = true;
                filtro.PermisoEscritura = false;
            }
            else
            {
                var usuarios = await _context.Ventas
                    .Where(u => u.Usuarios.AdministradorID != "SA" && (u.VentaCierreID == null || u.VentaCierreID == Guid.Empty))
                    .Select(u => u.UsuarioID).Distinct().ToListAsync();

                query = _context.Usuarios
                    .Where(u => usuarios.Contains(u.UsuarioID))
                    .Select(u => new VentaCierre()
                    {
                        Fecha = DateTime.Now,
                        ImporteSistema = 0,
                        ImporteUsuario = 0,
                        UsuarioCajaID = u.UsuarioID,
                        UsuarioCaja = u,
                        UsuarioID = u.UsuarioID,
                        Usuarios = u,
                        VentaCierreID = Guid.NewGuid(),
                    }).AsQueryable();

                filtro.PermisoLectura = false;
                filtro.PermisoEscritura = true;
            }

            if (!esAdministrador)
                query = query.Where(c => c.UsuarioID == token.UsuarioID);

            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        query = query.Where(c => c.Usuarios.Nombre.Contains(w) ||
                                                 c.Usuarios.PrimerApellido.Contains(w) ||
                                                 c.Usuarios.SegundoApellido.Contains(w));
                    }
                }
            }

            filtro.Registros = await query.CountAsync();
            filtro.Datos = await query.OrderByDescending(q => q.Fecha)
                .ThenByDescending(u => u.Usuarios.PrimerApellido)
                .ThenByDescending(u => u.Usuarios.SegundoApellido)
                .ThenByDescending(u => u.Usuarios.Nombre)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<VentaCierre>>>(ViewData, filtro)
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
