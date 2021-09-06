﻿namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using LAMBusiness.Web.Models.ViewModels;

    public class SesionController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IConverterHelper _converterHelper;
        private readonly IGetHelper _getHelper;
        private readonly ICriptografiaHelper _criptografiaHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private Guid moduloId = Guid.Parse("CAED13FC-E9FF-4E0C-8D3D-9ECE76196EA2");
        public SesionController(DataContext context, 
            IConfiguration configuration, 
            IConverterHelper converterHelper,
            IGetHelper getHelper,
            ICriptografiaHelper criptografiaHelper,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _converterHelper = converterHelper;
            _getHelper = getHelper;
            _criptografiaHelper = criptografiaHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> CerrarAplicacion()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }

            //delete records by user
            var sesiones = _context.Sesiones.Where(s => s.UsuarioID == token.UsuarioID);
            if (sesiones != null)
            {
                string directorio = _configuration.GetValue<string>("DirectorioSesion");
                string rutaSesion = "";

                foreach (var s in sesiones)
                {
                    _context.Remove(s);
                    rutaSesion = $"{directorio}//{s.SessionID}.config";
                    if (System.IO.File.Exists(rutaSesion))
                        System.IO.File.Delete(rutaSesion);
                }

                try
                {
                    if(HttpContext.Session.Keys.Any(x => x.Contains("LAMBusiness")))
                    {
                        HttpContext.Session.Remove("LAMBusiness");
                    }
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("CerrarSesion", token, token.ColaboradorID);
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                    await BitacoraAsync("CerrarSesion", token, token.ColaboradorID, excepcion);
                }
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> ChangeProfilePicture()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ChangeProfilePicture(string img)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return null; }

            string path = "";
            int tamaño = 0;

            //Guid usuarioId = token.UsuarioID;
            string colaboradorId = $"{token.ColaboradorID.ToString()}.png";
            string directorioImagenPerfil = _configuration.GetValue<string>("DirectorioImagenPerfil");

            if (!Directory.Exists(directorioImagenPerfil))
            {
                Directory.CreateDirectory(directorioImagenPerfil);
            }
            if (!Directory.Exists($"{directorioImagenPerfil}//Perfil//"))
            {
                Directory.CreateDirectory($"{directorioImagenPerfil}//Perfil//");
            }
            if (!Directory.Exists($"{directorioImagenPerfil}//Mediana//"))
            {
                Directory.CreateDirectory($"{directorioImagenPerfil}//Mediana//");
            }
            if (!Directory.Exists($"{directorioImagenPerfil}//Original//"))
            {
                Directory.CreateDirectory($"{directorioImagenPerfil}//Original//");
            }

            try
            {
                var index = img.IndexOf(',') + 1;
                img = img.Substring(index);

                for (byte x = 1; x <= 3; x++)
                {
                    switch (x)
                    {
                        case 1:
                            path = Path.Combine($"{directorioImagenPerfil}//Perfil//", colaboradorId);
                            tamaño = 95;
                            break;
                        case 2:
                            path = Path.Combine($"{directorioImagenPerfil}//Mediana//", colaboradorId);
                            tamaño = 380;
                            break;
                        case 3:
                            path = Path.Combine($"{directorioImagenPerfil}//Original//", colaboradorId);
                            tamaño = 760;
                            break;
                    }

                    byte[] bi = _converterHelper.UploadImageBase64(img, tamaño);
                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bi, 0, bi.Length);
                    }
                }
                TempData["toast"] = "Imagen de perfil actualizada";
                return Json(new { Error = false });
            }
            catch (Exception)
            {
                return Json(new { Estatus = "Error: Imagen de perfil no actualizada" + "[" + path + "]", Error = true });
                //Session["result"] = "Error: Imagen de perfil no actualizada";
            }
        }

        public async Task<IActionResult> ChangePassword()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }

            var pwd = new ChangePasswordViewModel()
            {
                ConfirmPassword = "",
                ConfirmPasswordEncrypt = "",
                Password = "",
                PasswordEncrypt = "",
                UsuarioID = Guid.Empty
            };

            return View(pwd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }
            bool band = true;

            if (ModelState.IsValid)
            {
                if(changePasswordViewModel.PasswordEncrypt != changePasswordViewModel.ConfirmPasswordEncrypt)
                {
                    TempData["toast"] = "Contraseñas distintas, verifique su información";
                    band = false;
                }
                
                var usuario = await _context.Usuarios.FindAsync(token.UsuarioID);
                if(usuario == null)
                {
                    TempData["toast"] = "Identificador incorrecto, usuario inexistente.";
                    band = false;
                }

                if (band)
                {
                    try
                    {
                        usuario.Password = _criptografiaHelper.Encrypt(changePasswordViewModel.PasswordEncrypt);
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                        TempData["toast"] = "Su contraseña ha sido actualizada con éxito.";

                        return RedirectToAction("Sesion", "Menu");
                    }
                    catch (Exception)
                    {
                        TempData["toast"] = "Error al actualizar el cambio de contraseña.";
                    }
                }
            }
            else
            {
                TempData["toast"] = "Proporcione la contraseña y confirme la información.";
            }

            changePasswordViewModel.ConfirmPassword = "";
            changePasswordViewModel.ConfirmPasswordEncrypt = "";
            changePasswordViewModel.Password = "";
            changePasswordViewModel.PasswordEncrypt = "";

            return View(changePasswordViewModel);
        }

        public async Task<FileContentResult> GetProfilePicture(Guid id, string tipo)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return null; }

            string directorioImagenPerfil = _configuration.GetValue<string>("DirectorioImagenPerfil");
            switch (tipo)
            {
                case "profile":
                    tipo = "perfil";
                    break;
                case "median":
                    tipo = "mediana";
                    break;
                case "original":
                    tipo = "Original";
                    break;
            }

            string ruta = Path.Combine(directorioImagenPerfil,tipo,$"{id}.png");
            //string ruta = $"{directorioImagenPerfil}//{tipo}//{id.ToString()}.png";

            if (!System.IO.File.Exists(ruta))
            {
                ruta = Path.Combine(_webHostEnvironment.WebRootPath, "images", "perfil" ,"user-slash.png");
            }

            return _converterHelper.ToImageBase64(ruta);
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task BitacoraAsync(string accion, object clase, Guid id, string excepcion = "")
        {
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");
            await _getHelper.SetBitacoraAsync(token, accion, moduloId,
                clase, id.ToString(), directorioBitacora, excepcion);
        }
    }
}
