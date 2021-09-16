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

    public class TasasImpuestosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("1B7183E2-E51B-4091-A99A-9BB38D462D81");

        public TasasImpuestosController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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

            var tasaImpuesto = _context.TasasImpuestos
                .Include(t => t.Productos)
                .OrderBy(t => t.Tasa);

            var filtro = new Filtro<List<TasaImpuesto>>()
            {
                Datos = await tasaImpuesto.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await tasaImpuesto.CountAsync(),
                Skip = 0
            };

            return View(filtro);

        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<TasaImpuesto>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<TasaImpuesto> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.TasasImpuestos
                                    .Include(t => t.Productos)
                                    .Where(t => t.Tasa.Contains(w) ||
                                           t.TasaDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Include(t => t.Productos)
                                    .Where(t => t.Tasa.Contains(w) ||
                                           t.TasaDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.TasasImpuestos.Include(t => t.Productos);
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(t => t.Tasa)
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
                            <Filtro<List<TasaImpuesto>>>(ViewData, filtro)
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
        public async Task<IActionResult> Create([Bind("TasaID,Tasa,Porcentaje,TasaDescripcion")] TasaImpuesto tasaImpuesto)
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
                    tasaImpuesto.TasaID = Guid.NewGuid();
                    _context.Add(tasaImpuesto);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", tasaImpuesto);
                    TempData["toast"] = "Los datos de la tasa de impuesto fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la tasa de impuesto no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", tasaImpuesto, excepcion);
                }
            }

            return View(tasaImpuesto);
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

            var tasaImpuesto = await _context.TasasImpuestos.FindAsync(id);
            if (tasaImpuesto == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(tasaImpuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TasaID,Tasa,Porcentaje,TasaDescripcion")] TasaImpuesto tasaImpuesto)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != tasaImpuesto.TasaID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasaImpuesto);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la tasa de impuesto fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", tasaImpuesto);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TasaImpuestoExists(tasaImpuesto.TasaID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos de la tasa de impuesto no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", tasaImpuesto, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la tasa de impuesto no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", tasaImpuesto, excepcion);
                }
            }

            return View(tasaImpuesto);
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

            var tasaImpuesto = await _context.TasasImpuestos
                .Include(t => t.Productos)
                .FirstOrDefaultAsync(t => t.TasaID == id);
            
            if (tasaImpuesto == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            if (tasaImpuesto.Productos.Count > 0)
            {
                TempData["toast"] = $"La Tasa de impuesto no se puede eliminar, porque está asignado a {tasaImpuesto.Productos.Count} producto(s).";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.TasasImpuestos.Remove(tasaImpuesto);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", tasaImpuesto);
                TempData["toast"] = "Los datos la tasa de impuesto fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos la tasa de impuesto no fueron eliminados.";
                await BitacoraAsync("Baja", tasaImpuesto, excepcion);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool TasaImpuestoExists(Guid id)
        {
            return _context.TasasImpuestos.Any(e => e.TasaID == id);
        }

        private async Task BitacoraAsync(string accion, TasaImpuesto tasaImpuesto, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                tasaImpuesto, tasaImpuesto.TasaID.ToString(), directorioBitacora, excepcion);
        }
    }
}
