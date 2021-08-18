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

    public class UsuariosController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly IConfiguration _configuration;
        private Guid moduloId = Guid.Parse("0C36B7F4-02DF-459A-9606-CAEEB137D9B1");

        public UsuariosController(DataContext context,
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

            var usuarios = _context.Usuarios
                .Include(e => e.Colaborador)
                .Where(e => e.AdministradorID != "SA")
                .OrderBy(e => e.Colaborador.CURP);

            return View(usuarios);
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

            IQueryable<Usuario> query = null;
            if (searchby != null && searchby != "")
            {
                var words = searchby.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Usuarios
                                    .Include(c => c.Colaborador)
                                    .Where(c => c.AdministradorID != "AS" &&
                                                (c.Colaborador.CURP.Contains(w) ||
                                                 c.Colaborador.Nombre.Contains(w) ||
                                                 c.Colaborador.PrimerApellido.Contains(w) ||
                                                 c.Colaborador.SegundoApellido.Contains(w)));
                        }
                        else
                        {
                            query = query
                                .Include(c => c.Colaborador)
                                .Where(c => c.AdministradorID != "SA" &&
                                            (c.Colaborador.CURP.Contains(w) ||
                                             c.Colaborador.Nombre.Contains(w) ||
                                             c.Colaborador.PrimerApellido.Contains(w) ||
                                             c.Colaborador.SegundoApellido.Contains(w)));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Usuarios
                    .Include(c => c.Colaborador)
                    .Where(c => c.AdministradorID != "SA");
            }

            var usuarios = await query.OrderBy(c => c.Colaborador.CURP)
                .Skip(skip)
                .Take(50)
                .ToListAsync();

            return new PartialViewResult
            {
                ViewName = "_AddRowsNextAsync",
                ViewData = new ViewDataDictionary
                            <List<Usuario>>(ViewData, usuarios)
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

            var usuario = await _context.Usuarios
                .Include(c => c.Administrador)
                .Include(c => c.Colaborador)
                .FirstOrDefaultAsync(u => u.UsuarioID == id);

            if (usuario == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioModulos = await (from um in _context.UsuariosModulos
                                        join m in _context.Modulos on um.ModuloID equals m.ModuloID
                                        where um.UsuarioID == id && m.Activo == true &&
                                        m.ModuloPadreID != Guid.Empty
                                        select um)
                                        .Include(m => m.Modulo)
                                        .OrderBy(m => m.Modulo.Descripcion)
                                        .ToListAsync();

            var usuarioDetails = new UsuarioDetailsViewModelUsuario()
            {
                Activo = usuario.Activo,
                Administrador = usuario.Administrador,
                AdministradorID = usuario.AdministradorID,
                Colaborador = usuario.Colaborador,
                ColaboradorID = usuario.ColaboradorID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                UsuarioID = usuario.UsuarioID,
                UsuarioModulos = usuarioModulos
            };

            return View(usuarioDetails);
        }

        public async Task<IActionResult> Create()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var usuarioViewModel = new UsuarioViewModel()
            {
                Activo = true,
                FechaInicio = DateTime.Now,
                FechaTermino = Convert.ToDateTime("9998-12-31"),
                AdministradoresDDL = await _combosHelper.GetComboAdministradoresAsync(token.UsuarioID),
            };

            return View(usuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel usuarioViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            usuarioViewModel.Password = "";
            usuarioViewModel.FechaUltimoAcceso = DateTime.Now;

            if (ModelState.IsValid)
            {
                var usuario = await _converterHelper.ToUsuarioAsync(usuarioViewModel, true);
                _context.Add(usuario);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = usuario.UsuarioID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            
            usuarioViewModel.AdministradoresDDL = await _combosHelper.GetComboAdministradoresAsync(token.UsuarioID);
            return View(usuarioViewModel);
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
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioViewModel = new UsuarioViewModel()
            {
                Activo = usuario.Activo,
                ColaboradorID = usuario.ColaboradorID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                AdministradorID = usuario.AdministradorID,
                AdministradoresDDL = await _combosHelper.GetComboAdministradoresAsync(token.UsuarioID),
                UsuarioID = usuario.UsuarioID
            };

            return View(usuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioViewModel usuarioViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var usuario = await _converterHelper.ToUsuarioAsync(usuarioViewModel, false);
                if (usuario == null)
                {
                    TempData["toast"] = "Identificacor incorrecto, verifique.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Update(usuario);

                try
                {
                    var sesiones = _context.Sesiones.Where(s => s.UsuarioID == usuario.UsuarioID);
                    if (sesiones != null)
                    {
                        string directorioSesion = _configuration.GetValue<string>("DirectorioSesion");
                        string rutaSesion = "";

                        foreach (var s in sesiones)
                        {
                            rutaSesion = $"{directorioSesion}//{s.SessionID}.config";
                            if (System.IO.File.Exists(rutaSesion))
                                System.IO.File.Delete(rutaSesion);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = usuario.UsuarioID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            
            TempData["toast"] = "Falta información en algún campo, verifique.";
            usuarioViewModel.AdministradoresDDL = await _combosHelper.GetComboAdministradoresAsync(token.UsuarioID);
            return View(usuarioViewModel);
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

            var usuario = await _getHelper.GetUsuarioByIdAsync((Guid)id);
            if (usuario == null)
            {
                TempData["toast"] = "Usuario inexistente (identificador incorrecto).";
                return RedirectToAction(nameof(Index));
            }

            var sesiones = await _context.Sesiones.Where(s => s.UsuarioID == (Guid)id).ToListAsync();
            if (sesiones != null)
            {
                string rutaSesion = "";
                string directorio = _configuration.GetValue<string>("DirectorioSesion");

                foreach (var sesion in sesiones)
                {
                    _context.Remove(sesion);
                    rutaSesion = $"{directorio}//{sesion.SessionID}.config";
                    if (System.IO.File.Exists(rutaSesion))
                        System.IO.File.Delete(rutaSesion);
                }
            }

            var usuariosModulos = await _context.UsuariosModulos.Where(u => u.UsuarioID == (Guid)id).ToListAsync();
            if (usuariosModulos != null)
            {
                foreach (var item in usuariosModulos)
                {
                    _context.Remove(item);
                }
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["toast"] = "Los datos del usuario fueron eliminados correctamente.";
            }
            catch (Exception)
            {
                TempData["toast"] = "Los datos del usuario no pueden ser eliminados por la integridad de la información.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetColaborador(string curp)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (curp == null || curp == "")
            {
                return null;
            }

            var colaborador = await _getHelper.GetColaboradorByCURPAsync(curp);
            if (colaborador != null)
            {
                return Json(
                    new
                    {
                        colaborador.ColaboradorID,
                        colaborador.PrimerApellido,
                        colaborador.SegundoApellido,
                        colaborador.Nombre,
                        error = false
                    });
            }

            return Json(new { error = true, message = "Colaborador inexistente" });

        }

        public async Task<IActionResult> GetColaboradores(string pattern, int? skip)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return null;
            }

            if (pattern == null || pattern == "" || skip == null)
            {
                return null;
            }

            var colaboradores = await _getHelper.GetColaboradoresSinCuentaUsuarioByPatternAsync(pattern, (int)skip);

            return new PartialViewResult
            {
                ViewName = "_GetColaboradores",
                ViewData = new ViewDataDictionary
                            <List<Colaborador>>(ViewData, colaboradores)
            };
        }

        //Edit Permissions
        public async Task<IActionResult> EditPermissions(Guid id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _context.Usuarios
                .Include(c => c.Administrador)
                .Include(c => c.Colaborador)
                .FirstOrDefaultAsync(u => u.UsuarioID == id);

            if (usuario == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            if (usuario.AdministradorID == "SA" || usuario.AdministradorID == "GA")
            {
                TempData["toast"] = "Usuario administrador no requiere asignación de permisos.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var usuarioModulos = await (from um in _context.UsuariosModulos
                                        join m in _context.Modulos on um.ModuloID equals m.ModuloID
                                        where um.UsuarioID == id && m.Activo == true &&
                                        m.ModuloPadreID != Guid.Empty
                                        select um)
                                        .Include(m => m.Modulo)
                                        .OrderBy(m => m.Modulo.Descripcion)
                                        .ToListAsync();
            
            var modulosAsignados = usuarioModulos.Select(m => m.ModuloID);
            var modulosNoAsignados = await _context.Modulos
                .Where(m => !modulosAsignados.Contains(m.ModuloID) &&
                       m.ModuloPadreID != Guid.Empty)
                .ToListAsync();

            if (modulosNoAsignados.Any())
            {
                foreach(var modulo in modulosNoAsignados)
                {
                    usuarioModulos.Add(new UsuarioModulo
                    {
                        Modulo = modulo,
                        ModuloID = modulo.ModuloID,
                        PermisoEscritura = false,
                        PermisoImprimir = false,
                        PermisoLectura = false,
                        UsuarioID = id,
                        UsuarioModuloID = Guid.Empty
                    });
                }
            }

            var usuarioDetails = new UsuarioDetailsViewModelUsuario()
            {
                Activo = usuario.Activo,
                Administrador = usuario.Administrador,
                AdministradorID = usuario.AdministradorID,
                Colaborador = usuario.Colaborador,
                ColaboradorID = usuario.ColaboradorID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                UsuarioID = usuario.UsuarioID,
                UsuarioModulos = usuarioModulos.OrderBy(m => m.Modulo.Descripcion).ToList()
            };

            return View(usuarioDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPermissions(UsuarioDetailsViewModelUsuario usuarioDetailsViewModelUsuario)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
            {
                return RedirectToAction(nameof(Index));
            }

            if(usuarioDetailsViewModelUsuario.UsuarioModulos != null)
            {
                foreach(var modulo in usuarioDetailsViewModelUsuario.UsuarioModulos)
                {
                    if(modulo.UsuarioModuloID == Guid.Empty)
                    {
                        if (modulo.PermisoEscritura || modulo.PermisoImprimir || modulo.PermisoLectura)
                        {
                            _context.UsuariosModulos.Add(new UsuarioModulo { 
                                ModuloID = modulo.ModuloID,
                                PermisoEscritura = modulo.PermisoEscritura,
                                PermisoImprimir = modulo.PermisoImprimir,
                                PermisoLectura = modulo.PermisoLectura,
                                UsuarioID = usuarioDetailsViewModelUsuario.UsuarioID,
                                UsuarioModuloID = Guid.NewGuid()
                            });
                        }
                    }
                    else
                    {
                        var moduloExist = _context.UsuariosModulos.Find(modulo.UsuarioModuloID);
                        if(moduloExist != null)
                        {
                            if(!modulo.PermisoEscritura && !modulo.PermisoImprimir && !modulo.PermisoLectura)
                            {
                                _context.Remove(moduloExist);
                            }
                            else
                            {
                                moduloExist.PermisoEscritura = modulo.PermisoEscritura;
                                moduloExist.PermisoImprimir = modulo.PermisoImprimir;
                                moduloExist.PermisoLectura = modulo.PermisoLectura;
                                
                                _context.Update(moduloExist);
                            }
                        }
                    }
                }

                try
                {
                    var sesiones = _context.Sesiones.Where(s => s.UsuarioID == usuarioDetailsViewModelUsuario.UsuarioID);
                    if (sesiones != null)
                    {
                        string directorioSesion = _configuration.GetValue<string>("DirectorioSesion");
                        string rutaSesion = "";

                        foreach (var s in sesiones)
                        {
                            //_context.Remove(s);
                            rutaSesion = $"{directorioSesion}//{s.SessionID}.config";
                            if (System.IO.File.Exists(rutaSesion))
                                System.IO.File.Delete(rutaSesion);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["toast"] = "Permisos actualizados con éxito.";

                    return RedirectToAction(nameof(Details), new { id = usuarioDetailsViewModelUsuario.UsuarioID});
                }
                catch (Exception)
                {
                    TempData["toast"] = "Error al actualizar los permisos seleccionados.";
                    //bitacora
                } 
            }
            else
            {
                TempData["toast"] = "Permisos no actualizados";
            }

            var usuario = await _context.Usuarios
                .Include(c => c.Administrador)
                .Include(c => c.Colaborador)
                .FirstOrDefaultAsync(u => u.UsuarioID == usuarioDetailsViewModelUsuario.UsuarioID);

            if (usuario == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioDetails = new UsuarioDetailsViewModelUsuario()
            {
                Activo = usuario.Activo,
                Administrador = usuario.Administrador,
                AdministradorID = usuario.AdministradorID,
                Colaborador = usuario.Colaborador,
                ColaboradorID = usuario.ColaboradorID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                UsuarioID = usuario.UsuarioID,
                UsuarioModulos = usuarioDetailsViewModelUsuario.UsuarioModulos
            };

            return View(usuarioDetails);
        }
    }
}
