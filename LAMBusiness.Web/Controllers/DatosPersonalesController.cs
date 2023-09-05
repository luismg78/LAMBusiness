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
    using Shared.Catalogo;
    public class DatosPersonalesController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("F2839E65-FC0F-4E75-894C-85259A583E13");

        public DatosPersonalesController(DataContext context,
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
                return RedirectToAction("Inicio", "Home");
            }

            var colaboradores = _context.DatosPersonales
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .Where(c => c.CURP != "CURP781227HCSRNS00")
                .OrderBy(c => c.CURP);

            var filtro = new Filtro<List<DatoPersonal>>()
            {
                Datos = await colaboradores.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await colaboradores.CountAsync(),
                Skip = 0
            };

            return View(filtro);
        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<DatoPersonal>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<DatoPersonal> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.DatosPersonales
                                    .Include(c => c.Municipios.Estados)
                                    .Include(c => c.Municipios)
                                    .Where(c => c.CURP.Contains(w) ||
                                                c.Municipios.Estados.Nombre.Contains(w) ||
                                                c.Municipios.Nombre.Contains(w));
                        }
                        else
                        {
                            query = query
                                .Include(c => c.Municipios.Estados)
                                .Include(c => c.Municipios)
                                .Where(c => c.CURP.Contains(w) ||
                                            c.Municipios.Estados.Nombre.Contains(w) ||
                                            c.Municipios.Nombre.Contains(w));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.DatosPersonales
                    .Include(c => c.Municipios.Estados)
                    .Include(c => c.Municipios);
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.Where(c => c.CURP != "CURP781227HCSRNS00").OrderBy(p => p.CURP)
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
                            <Filtro<List<DatoPersonal>>>(ViewData, filtro)
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

            var colaborador = await _context.DatosPersonales
                .Include(c => c.Puestos)
                .Include(c => c.Generos)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
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

            DatoPersonalViewModel colaboradorViewModel = new DatoPersonalViewModel()
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
        public async Task<IActionResult> Create(DatoPersonalViewModel colaboradorViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }
            
            TempData["toast"] = "Falta información en algún campo, verifique.";
            await ValidarDatosDelColaborador(colaboradorViewModel);

            if (ModelState.IsValid)
            {
                var resultado = await _converterHelper.ToDatoPersonalAsync(colaboradorViewModel, true);
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
                    await BitacoraAsync("Alta", resultado.Contenido);
                    return RedirectToAction(nameof(Details), new { id = resultado.Contenido.UsuarioID });
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del colaborador no fueron almacenados.";
                    await BitacoraAsync("Alta", resultado.Contenido, excepcion);
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

            var colaborador = await _context.DatosPersonales
                .Include(c => c.Generos)
                .Include(c => c.Puestos)
                .Include(c => c.EstadosCiviles)
                .Include(c => c.Municipios)
                .Include(c => c.Municipios.Estados)
                .FirstOrDefaultAsync(c => c.UsuarioID == id);
            if (colaborador == null)
            {
                TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            var colaboradorViewModel = await _converterHelper.ToDatoPersonalViewModelAsync(colaborador);

            return View(colaboradorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DatoPersonalViewModel colaboradorViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != colaboradorViewModel.UsuarioID)
            {
                TempData["toast"] = "Identificador incorrecto.";
                return RedirectToAction(nameof(Index));
            }

            TempData["toast"] = "Falta información en algún campo.";
            await ValidarDatosDelColaborador(colaboradorViewModel);

            if (ModelState.IsValid)
            {                
                var resultado = await _converterHelper.ToDatoPersonalAsync(colaboradorViewModel, false);
                try
                {
                    if(resultado.Error)
                    {
                        TempData["toast"] = resultado.Mensaje;
                        return View(colaboradorViewModel);
                    }

                    _context.Update(resultado.Contenido);
                    await _context.SaveChangesAsync();

                    TempData["toast"] = "Los datos del colaborador fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", resultado.Contenido);
                    return RedirectToAction(nameof(Details), new { id = resultado.Contenido.UsuarioID });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    if (!ColaboradorExists(colaboradorViewModel.UsuarioID))
                        TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                    else
                        TempData["toast"] = "[Error] Los datos del colaborador no fueron actualizados.";
                    
                    await BitacoraAsync("Actualizar", resultado.Contenido, excepcion);
                }
                catch(Exception ex) {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "[Error] Los datos del colaborador no fueron actualizados.";
                    await BitacoraAsync("Actualizar", resultado.Contenido, excepcion);
                }

                return RedirectToAction(nameof(Index));
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

            var colaborador = await _getHelper.GetDatosPersonalesByIdAsync((Guid)id);
            if (colaborador == null)
            {
                TempData["toast"] = "Colaborador inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.DatosPersonales.Remove(colaborador);
                await _context.SaveChangesAsync();

                TempData["toast"] = "Los datos del colaborador fueron eliminados correctamente.";
                await BitacoraAsync("Baja", colaborador);
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "Los datos del colaborador no pueden ser eliminados por integridad de la información.";
                await BitacoraAsync("Baja", colaborador, excepcion);
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
            return _context.DatosPersonales.Any(e => e.UsuarioID == id);
        }

        private async Task BitacoraAsync(string accion, DatoPersonal colaborador, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                colaborador, colaborador.UsuarioID.ToString(), directorioBitacora, excepcion);
        }

        private async Task ValidarDatosDelColaborador(DatoPersonalViewModel colaborador)
        {
            colaborador.CURP = colaborador.CURP.Trim().ToUpper();
            //colaborador.Email = colaborador.Email.Trim().ToLower();

            var existeCURP = await _context.DatosPersonales
                .AnyAsync(c => c.CURP == colaborador.CURP && 
                               c.UsuarioID != colaborador.UsuarioID);
            if (existeCURP)
            {
                TempData["toast"] = "CURP previamente asignada a otro colaborador.";
                ModelState.AddModelError("CURP", "CURP previamente asignada.");
            }

            //var existeEmail = await _context.Colaboradores
            //    .AnyAsync(c => c.Email == colaborador.Email &&
            //                   c.UsuarioID != colaborador.UsuarioID);
            //if (existeEmail)
            //{
            //    TempData["toast"] = "Email previamente asignada a otro colaborador.";
            //    ModelState.AddModelError("Email", "Email previamente asignada.");
            //}
        }
    }
}
