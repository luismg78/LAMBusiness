namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Linq;  
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class ModulosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IGetHelper _getHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        //36a7c3fa-a7fe-4288-988f-adcdfef9ef63

        public ModulosController(DataContext context,
            IGetHelper getHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IConfiguration configuration)
        {
            _context = context;
            _getHelper = getHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA") {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu"); 
            }

            var modulos = _context.Modulos.OrderBy(m => m.Descripcion);

            var filtro = new Filtro<List<Modulo>>()
            {
                Datos = await modulos.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = true,
                PermisoImprimir = true,
                PermisoLectura = true,
                Registros = await modulos.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Modulo>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            IQueryable<Modulo> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Modulos
                                    .Where(p => p.Descripcion.Contains(w));
                        }
                        else
                        {
                            query = query.Where(p => p.Descripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Modulos;
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(m => m.Descripcion)
                .Skip(filtro.Skip)
                .Take(50)
                .ToListAsync();

            filtro.PermisoEscritura = true;
            filtro.PermisoImprimir = true;
            filtro.PermisoLectura = true;

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <Filtro<List<Modulo>>>(ViewData, filtro)
            };
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            var moduloViewModel = new ModuloViewModel()
            {
                ModulosPadresDDL = await _combosHelper.GetComboModulosAsync()
            };

            return View(moduloViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuloViewModel moduloViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            if (ModelState.IsValid)
            {
                var modulo = _converterHelper.ToModulo(moduloViewModel, true);
                _context.Add(modulo);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del módulo fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["toast"] = "[Error] Los datos del cliente no fueron almacenados.";
                }
            }

            moduloViewModel.ModulosPadresDDL = await _combosHelper.GetComboModulosAsync();
            TempData["toast"] = "Falta información en algún campo, verifique.";

            return View(moduloViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var modulo = await _context.Modulos.FindAsync(id);
            if (modulo == null)
            {
                TempData["toast"] = "Módulo inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            var moduloViewModel = await _converterHelper.ToModuloViewModelAsync(modulo);

            return View(moduloViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ModuloViewModel moduloViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            if (id != moduloViewModel.ModuloID)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var modulo = _converterHelper.ToModulo(moduloViewModel, false);
                    _context.Update(modulo);

                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del módulo fueron actualizados correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuloExists(moduloViewModel.ModuloID))
                    {
                        TempData["toast"] = "Módulo inexistente (identificador incorrecto).";
                    }
                    else
                    {
                        TempData["toast"] = "[Error] Los datos del módulo no fueron actualizados.";
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            return View(moduloViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Inicio", "Menu");
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var modulo = await _getHelper.GetModuloByIdAsync((Guid)id);
            if (modulo == null)
            {
                TempData["toast"] = "Módulo inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            _context.Modulos.Remove(modulo);
            await _context.SaveChangesAsync();

            TempData["toast"] = "Los datos del módulo fueron eliminados correctamente.";
            return RedirectToAction(nameof(Index));

        }

        private bool ModuloExists(Guid id)
        {
            return _context.Modulos.Any(e => e.ModuloID == id);
        }
    }
}
