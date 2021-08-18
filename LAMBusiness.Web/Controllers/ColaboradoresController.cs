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
    using Shared.Contacto;
    using Shared.Catalogo;

    public class ColaboradoresController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("F2839E65-FC0F-4E75-894C-85259A583E13");

        public ColaboradoresController(DataContext context,
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
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction("Inicio", "Menu");
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            var dataContext = _context.Colaboradores
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .Where(c => c.CURP != "CURP781227HCSRNS00")
                .OrderBy(c => c.CURP);

            return View(await dataContext.ToListAsync());
        }

        public async Task<IActionResult> _AddRowsNextAsync(string searchby, int skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            IQueryable<Colaborador> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Colaboradores
                                    .Include(c => c.Municipios.Estados)
                                    .Include(c => c.Municipios)
                                    .Where(c => c.CURP.Contains(w) ||
                                                c.Nombre.Contains(w) ||
                                                c.PrimerApellido.Contains(w) ||
                                                c.SegundoApellido.Contains(w) ||
                                                c.Municipios.Estados.EstadoDescripcion.Contains(w) ||
                                                c.Municipios.MunicipioDescripcion.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(c => c.Municipios.Estados)
                                .Include(c => c.Municipios)
                                .Where(c => c.CURP.Contains(w) ||
                                            c.Nombre.Contains(w) ||
                                            c.PrimerApellido.Contains(w) ||
                                            c.SegundoApellido.Contains(w) ||
                                            c.Municipios.Estados.EstadoDescripcion.Contains(w) ||
                                            c.Municipios.MunicipioDescripcion.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Colaboradores
                    .Include(c => c.Municipios.Estados)
                    .Include(c => c.Municipios);
            }

            var colaboradores = await query.Where(c => c.CURP != "CURP781227HCSRNS00").OrderBy(p => p.CURP)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Colaborador>>(ViewData, colaboradores)
            };
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PermisoEscritura = permisosModulo.PermisoEscritura;

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var colaborador = await _context.Colaboradores
                .Include(c => c.Puestos)
                .Include(c => c.Generos)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .FirstOrDefaultAsync(m => m.ColaboradorID == id);
            if (colaborador == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            return View(colaborador);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            ColaboradorViewModel colaboradorViewModel = new ColaboradorViewModel()
            {
                EstadosDDL = await _combosHelper.GetComboEstadosAsync(),
                EstadosCivilesDDL = await _combosHelper.GetComboEstadosCivilesAsync(),
                EstadosNacimientoDDL = await _combosHelper.GetComboEstadosAsync(),
                GenerosDDL = await _combosHelper.GetComboGenerosAsync(),
                MunicipiosDDL = await _combosHelper.GetComboMunicipiosAsync(0),
                PuestosDDL = await _combosHelper.GetComboPuestosAsync()
            };

            return View(colaboradorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColaboradorViewModel colaboradorViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }
            
            TempData["toast"] = "Falta información en algún campo, verifique.";

            if (ModelState.IsValid)
            {
                var resultado = await _converterHelper.ToColaboradorAsync(colaboradorViewModel, true);
                if (resultado.Error)
                {
                    TempData["toast"] = resultado.Mensaje;
                    return View(colaboradorViewModel);
                }

                try
                {
                    _context.Add(resultado.Contenido);
                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del colaborador fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = resultado.Contenido.ColaboradorID });
                }
                catch (Exception)
                {
                    TempData["toast"] = "[Error] Los datos del colaborador no fueron almacenados.";
                }
            }

            colaboradorViewModel.EstadosDDL = await _combosHelper.GetComboEstadosAsync();
            colaboradorViewModel.MunicipiosDDL = await _combosHelper
                .GetComboMunicipiosAsync((short)colaboradorViewModel.EstadoID);
            colaboradorViewModel.EstadosCivilesDDL = await _combosHelper.GetComboEstadosCivilesAsync();
            colaboradorViewModel.EstadosNacimientoDDL = await _combosHelper.GetComboEstadosAsync();
            colaboradorViewModel.GenerosDDL = await _combosHelper.GetComboGenerosAsync();
            colaboradorViewModel.PuestosDDL = await _combosHelper.GetComboPuestosAsync();

            return View(colaboradorViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var colaborador = await _context.Colaboradores
                .Include(c => c.Generos)
                .Include(c => c.Puestos)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .FirstOrDefaultAsync(c => c.ColaboradorID == id);
            if (colaborador == null)
            {
                TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            var colaboradorViewModel = await _converterHelper.ToColaboradorViewModelAsync(colaborador);

            return View(colaboradorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ColaboradorViewModel colaboradorViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != colaboradorViewModel.ColaboradorID)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {                
                try
                {
                    var resultado = await _converterHelper.ToColaboradorAsync(colaboradorViewModel, false);
                    if(resultado.Error)
                    {
                        TempData["toast"] = resultado.Mensaje;
                        return View(colaboradorViewModel);
                    }

                    _context.Update(resultado.Contenido);

                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Los datos del colaborador fueron actualizados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = resultado.Contenido.ColaboradorID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorExists(colaboradorViewModel.ColaboradorID))
                        TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                    else
                        TempData["toast"] = "[Error] Los datos del colaborador no fueron actualizados.";
                }
                catch(Exception ex) {
                    string x = ex.Message;
                    TempData["toast"] = "Los datos del colaborador no fueron actualizados.";
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            return View(colaboradorViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            var colaborador = await _getHelper.GetColaboradorByIdAsync((Guid)id);
            if (colaborador == null)
            {
                TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Colaboradores.Remove(colaborador);
                await _context.SaveChangesAsync();

                TempData["toast"] = "Los datos del colaborador fueron eliminados correctamente.";
            }
            catch (Exception)
            {
                TempData["toast"] = "Los datos del colaborador no pueden ser eliminados por integridad de la información.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddMunicipios(short? id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (id == null)
            {
                return null;
            }

            var municipios = await _getHelper.GetMunicipiosByEstadoIdAsync((short)id);

            return new PartialViewResult
            {
                ViewName = "_AddComboMunicipios",
                ViewData = new ViewDataDictionary
                            <List<Municipio>>(ViewData, municipios)
            };
        }

        private bool ColaboradorExists(Guid id)
        {
            return _context.Colaboradores.Any(e => e.ColaboradorID == id);
        }
    }
}
