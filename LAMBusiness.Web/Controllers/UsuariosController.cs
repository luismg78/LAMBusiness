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
                return RedirectToAction("Inicio", "Home");
            }

            var usuarios = _context.Usuarios
                .Where(e => e.AdministradorID != "SA")
                .OrderBy(e => e.PrimerApellido)
                .ThenBy(e => e.SegundoApellido)
                .ThenBy(e => e.Nombre);

            var filtro = new Filtro<List<Usuario>>()
            {
                Datos = await usuarios.Take(50).ToListAsync(),
                Patron = "",
                PermisoEscritura = permisosModulo.PermisoEscritura,
                PermisoImprimir = permisosModulo.PermisoImprimir,
                PermisoLectura = permisosModulo.PermisoLectura,
                Registros = await usuarios.CountAsync(),
                Skip = 0
            };

            return View(filtro);

        }

        public async Task<IActionResult> _AddRowsNextAsync(Filtro<List<Usuario>> filtro)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return null; }

            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return null;
            }

            IQueryable<Usuario> query = null;
            if (filtro.Patron != null && filtro.Patron != "")
            {
                var words = filtro.Patron.Trim().ToUpper().Split(' ');
                foreach (var w in words)
                {
                    if (w.Trim() != "")
                    {
                        if (query == null)
                        {
                            query = _context.Usuarios
                                    .Where(c => c.AdministradorID != "AS" &&
                                                (c.Nombre.Contains(w) ||
                                                 c.PrimerApellido.Contains(w) ||
                                                 c.SegundoApellido.Contains(w)));
                        }
                        else
                        {
                            query = query
                                .Where(c => c.AdministradorID != "SA" &&
                                            (c.Nombre.Contains(w) ||
                                             c.PrimerApellido.Contains(w) ||
                                             c.SegundoApellido.Contains(w)));
                        }
                    }
                }
            }
            if (query == null)
            {
                query = _context.Usuarios.Where(c => c.AdministradorID != "SA");
            }

            filtro.Registros = await query.CountAsync();

            filtro.Datos = await query.OrderBy(c => c.NombreCompleto)
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
                            <Filtro<List<Usuario>>>(ViewData, filtro)
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

            if (id == null)
            {
                TempData["toast"] = "Identificacor incorrecto, verifique.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _context.Usuarios
                .Include(c => c.Administrador)
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
                UsuarioID = usuario.UsuarioID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                PermisoEscritura = permisosModulo.PermisoEscritura,
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

            TempData["toast"] = "Falta información en algún campo.";
            if (ModelState.IsValid)
            {
                var usuario = await _converterHelper.ToUsuarioAsync(usuarioViewModel, true);
                _context.Add(usuario);

                try
                {
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("Alta", usuario);
                    TempData["toast"] = "Los datos del usuario fueron almacenados correctamente.";
                    return RedirectToAction(nameof(Details), new { id = usuario.UsuarioID });
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del usuario no fueron almacenados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Alta", usuario, excepcion);
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
                UsuarioID = usuario.UsuarioID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                AdministradorID = usuario.AdministradorID,
                AdministradoresDDL = await _combosHelper.GetComboAdministradoresAsync(token.UsuarioID),
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

            TempData["toast"] = "Falta información en algún campo, verifique.";
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
                    TempData["toast"] = "Los datos del usuario fueron actualizados correctamente.";
                    await BitacoraAsync("Actualizar", usuario);
                    return RedirectToAction(nameof(Details), new { id = usuario.UsuarioID });
                }
                catch (Exception ex)
                {
                    TempData["toast"] = "[Error] Los datos del usuario no fueron actualizados.";
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("Actualizar", usuario, excepcion);
                }
            }
            
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
                await BitacoraAsync("Baja", usuario);
                TempData["toast"] = "Los datos del usuario fueron eliminados correctamente.";
            }
            catch (Exception ex)
            {
                string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                TempData["toast"] = "Los datos del usuario no pueden ser eliminados por la integridad de la información.";
                await BitacoraAsync("Baja", usuario, excepcion);
            }

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> GetColaborador(string curp)
        //{
        //    var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
        //    if (validateToken != null) { return null; }

        //    if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
        //    {
        //        return null;
        //    }

        //    if (curp == null || curp == "")
        //    {
        //        return null;
        //    }

        //    var colaborador = await _getHelper.GetDatosPersonalesByCURPAsync(curp);
        //    if (colaborador != null)
        //    {
        //        return Json(
        //            new
        //            {
        //                colaborador.UsuarioID,
        //                colaborador.PrimerApellido,
        //                colaborador.SegundoApellido,
        //                colaborador.Nombre,
        //                error = false
        //            });
        //    }

        //    return Json(new { error = true, message = "Colaborador inexistente" });

        //}

        //public async Task<IActionResult> GetColaboradores(string pattern, int? skip)
        //{
        //    var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
        //    if (validateToken != null) { return null; }

        //    if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoEscritura))
        //    {
        //        return null;
        //    }

        //    if (pattern == null || pattern == "" || skip == null)
        //    {
        //        return null;
        //    }

        //    var colaboradores = await _getHelper.GetColaboradoresSinCuentaUsuarioByPatternAsync(pattern, (int)skip);

        //    return new PartialViewResult
        //    {
        //        ViewName = "_GetColaboradores",
        //        ViewData = new ViewDataDictionary
        //                    <List<DatoPersonal>>(ViewData, colaboradores)
        //    };
        //}

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
                UsuarioID = usuario.UsuarioID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
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
                    await BitacoraAsync("Permisos", usuarioDetailsViewModelUsuario.UsuarioModulos, usuarioDetailsViewModelUsuario.UsuarioID.ToString());
                    return RedirectToAction(nameof(Details), new { id = usuarioDetailsViewModelUsuario.UsuarioID});
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    TempData["toast"] = "Error al actualizar los permisos seleccionados.";
                    await BitacoraAsync("Permisos", usuarioDetailsViewModelUsuario.UsuarioModulos, usuarioDetailsViewModelUsuario.UsuarioID.ToString(), excepcion);
                } 
            }
            else
            {
                TempData["toast"] = "Permisos no actualizados";
            }

            var usuario = await _context.Usuarios
                .Include(c => c.Administrador)
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
                UsuarioID = usuario.UsuarioID,
                FechaInicio = usuario.FechaInicio,
                FechaTermino = usuario.FechaTermino,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                UsuarioModulos = usuarioDetailsViewModelUsuario.UsuarioModulos
            };

            return View(usuarioDetails);
        }

        private async Task BitacoraAsync(string accion, Usuario usuario, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                usuario, usuario.UsuarioID.ToString(), directorioBitacora, excepcion);
        }
        private async Task BitacoraAsync(string accion, List<UsuarioModulo> usuarioModulo, string usuarioId, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                usuarioModulo, usuarioId.ToString(), directorioBitacora, excepcion);
        }
    }
}
