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
    using Helpers;
    using Data;
    using Shared.Aplicacion;
    using Shared.Catalogo;

    public class EstadosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("F2C4DB86-8C15-46BD-B8DE-FB64DE3BFCFF");

        public EstadosController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            var estados = _context.Estados
                .Include(e => e.Municipios)
                .OrderBy(e => e.EstadoDescripcion);

            var filtro = new Filtro<List<Estado>>()
            {
                Datos = await estados.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await estados.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Estado>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }
            
            IQueryable<Estado> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Estados.Include(e => e.Municipios)
                                    .Where(e => e.EstadoClave.Contains(w) ||
                                           e.EstadoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(e => e.EstadoClave.Contains(w) ||
                                                e.EstadoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Estados.Include(e => e.Municipios);
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.EstadoDescripcion)
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
                            <Filtro<List<Estado>>>(ViewData, filtro)
            };

        }
        
        public async Task<IActionResult> Edit(short? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("EstadoID,EstadoClave,EstadoDescripcion")] Estado estado)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != estado.EstadoID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estado);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del Estado fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", estado);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EstadoExists(estado.EstadoID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del Estado no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", estado, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del Estado no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", estado, excepcion);
                }
            }

            return View(estado);
        }

        private bool EstadoExists(short id)
        {
            return _context.Estados.Any(e => e.EstadoID == id);
        }

        private async Task BitacoraAsync(string accion, Estado estado, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                estado, estado.EstadoID.ToString(), directorioBitacora, excepcion);
        }
    }
}
