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
    using Shared.Aplicacion;
    using Shared.Catalogo;

    public class SalidasTipoController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("06239203-ACED-4600-9F70-784BF73281EC");

        public SalidasTipoController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var salidaTipo = _context.SalidasTipo
                .OrderBy(e => e.SalidaTipoDescripcion);

            var filtro = new Filtro<List<SalidaTipo>>()
            {
                Datos = await salidaTipo.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await salidaTipo.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<SalidaTipo>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<SalidaTipo> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.SalidasTipo
                                    .Where(e => e.SalidaTipoDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(e => e.SalidaTipoDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.SalidasTipo;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.SalidaTipoDescripcion)
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
                            <Filtro<List<SalidaTipo>>>(ViewData, filtro)
            };
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalidaTipoID,SalidaTipoDescripcion")] SalidaTipo salidaTipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            if (ModelState.IsValid)
            {
                try
                {
                    salidaTipo.SalidaTipoID = Guid.NewGuid();
                    _context.Add(salidaTipo);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", salidaTipo);
                    TempData["toast"] = "Los datos del tipo de salida fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del tipo de salida no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", salidaTipo, excepcion);
                }
            }

            return View(salidaTipo);
        }

        public async Task<IActionResult> Edit(Guid? id)
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

            var salidaTipo = await _context.SalidasTipo.FindAsync(id);
            if (salidaTipo == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(salidaTipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SalidaTipoID,SalidaTipoDescripcion")] SalidaTipo salidaTipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != salidaTipo.SalidaTipoID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salidaTipo);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del tipo de salida fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", salidaTipo);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SalidaTipoExists(salidaTipo.SalidaTipoID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del tipo de salida no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", salidaTipo, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del tipo de salida no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", salidaTipo, excepcion);
                }
            }

            return View(salidaTipo);
        }

        public async Task<IActionResult> Delete(Guid? id)
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

            var salidaTipo = await _context.SalidasTipo
                               .FirstOrDefaultAsync(m => m.SalidaTipoID == id);

            if (salidaTipo == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.SalidasTipo.Remove(salidaTipo);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", salidaTipo);
                TempData["toast"] = "Los datos del tipo de salida fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos del tipo de salida no fueron eliminados.";
                await BitacoraAsync("Baja", salidaTipo, excepcion);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SalidaTipoExists(Guid id)
        {
            return _context.SalidasTipo.Any(e => e.SalidaTipoID == id);
        }

        private async Task BitacoraAsync(string accion, SalidaTipo salidaTipo, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                salidaTipo, salidaTipo.SalidaTipoID.ToString(), directorioBitacora, excepcion);
        }
    }
}
