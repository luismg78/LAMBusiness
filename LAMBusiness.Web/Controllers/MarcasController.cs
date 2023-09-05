namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Shared.Aplicacion;
    using Shared.Catalogo;

    public class MarcasController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private Guid moduloId = Guid.Parse("7FCF442E-5377-47E0-B5F5-1C6A5067609C");

        public MarcasController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
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
                return RedirectToAction("Inicio", "Home");
            }

            var marcas = _context.Marcas
                .OrderBy(p => p.Nombre);

            var filtro = new Filtro<List<Marca>>()
            {
                Datos = await marcas.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await marcas.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Marca>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Marca> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Marcas
                                    .Where(p => p.Nombre.Contains(w) ||
                                           p.Descripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.Nombre.Contains(w) ||
                                                p.Descripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Marcas;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.Nombre)
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
                            <Filtro<List<Marca>>>(ViewData, filtro)
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
        public async Task<IActionResult> Create([Bind("MarcaID,Nombre,Descripcion")] Marca marca)
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
                    marca.MarcaID = Guid.NewGuid();
                    _context.Add(marca);
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", marca);
                    TempData["toast"] = "Los datos de la marca fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la marca no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", marca, excepcion);
                }
            }
            
            return View(marca);
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

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MarcaID,Nombre,Descripcion")] Marca marca)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != marca.MarcaID)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo, verifique.";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos de la marca fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", marca);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MarcaExists(marca.MarcaID))
                    {
                        TempData["toast"] = "Registro inexistente.";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos de la marca no fueron actualizados.";
                    }
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", marca, excepcion);
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos de la marca no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", marca, excepcion);
                }
            }
            
            return View(marca);
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

            var marca = await _context.Marcas
                               .FirstOrDefaultAsync(m => m.MarcaID == id);

            if (marca == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Marcas.Remove(marca);
                await _context.SaveChangesAsync();
                await BitacoraAsync("Baja", marca);
                TempData["toast"] = "Los datos de la marca fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "[Error] Los datos de la marca no fueron eliminados.";
                await BitacoraAsync("Baja", marca, excepcion);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(Guid id)
        {
            return _context.Marcas.Any(e => e.MarcaID == id);
        }

        private async Task BitacoraAsync(string accion, Marca marca, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                marca, marca.MarcaID.ToString(), directorioBitacora, excepcion);
        }
    }
}
