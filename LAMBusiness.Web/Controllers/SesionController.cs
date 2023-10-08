namespace LAMBusiness.Web.Controllers
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
                    rutaSesion = Path.Combine(directorio, $"{s.SessionID}.config");
                    if (System.IO.File.Exists(rutaSesion))
                        System.IO.File.Delete(rutaSesion);
                }

                try
                {
                    if (HttpContext.Session.Keys.Any(x => x.Contains("LAMBusiness")))
                    {
                        HttpContext.Session.Remove("LAMBusiness");
                    }
                    await _context.SaveChangesAsync();
                    await BitacoraAsync("CerrarSesion", token, token.UsuarioID);
                }
                catch (Exception ex)
                {
                    string excepcion = ex.InnerException != null ? ex.InnerException.Message.ToString() : ex.ToString();
                    await BitacoraAsync("CerrarSesion", token, token.UsuarioID, excepcion);
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
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<JsonResult> ChangeProfilePicture(string imagen)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return null; }

            if(string.IsNullOrEmpty(imagen))
                return Json(new { Estatus = $"Error: Imagen de perfil no actualizada.", Error = true });

            string path = "";
            int tamaño = 0;
            string UsuarioID = $"{token.UsuarioID}.png";
            string directorioImagenPerfil = _configuration.GetValue<string>("DirectorioImagenPerfil");

            if (!Directory.Exists(directorioImagenPerfil))
            {
                Directory.CreateDirectory(directorioImagenPerfil);
            }
            if (!Directory.Exists(Path.Combine(directorioImagenPerfil, "Perfil")))
            {
                Directory.CreateDirectory(Path.Combine(directorioImagenPerfil, "Perfil"));
            }
            if (!Directory.Exists(Path.Combine(directorioImagenPerfil, "Mediana")))
            {
                Directory.CreateDirectory(Path.Combine(directorioImagenPerfil, "Mediana"));
            }
            if (!Directory.Exists(Path.Combine(directorioImagenPerfil, "Original")))
            {
                Directory.CreateDirectory(Path.Combine(directorioImagenPerfil, "Original"));
            }

            try
            {
                var index = imagen.IndexOf(',') + 1;
                imagen = imagen.Substring(index);

                for (byte x = 1; x <= 3; x++)
                {
                    switch (x)
                    {
                        case 1:
                            path = Path.Combine(directorioImagenPerfil, "Perfil", UsuarioID);
                            tamaño = 95;
                            break;
                        case 2:
                            path = Path.Combine(directorioImagenPerfil, "Mediana", UsuarioID);
                            tamaño = 380;
                            break;
                        case 3:
                            path = Path.Combine(directorioImagenPerfil, "Original", UsuarioID);
                            tamaño = 760;
                            break;
                    }

                    byte[] bi = _converterHelper.UploadImageBase64(imagen, tamaño);
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
                return Json(new { Estatus = $"Error: Imagen de perfil no actualizada [{path}]", Error = true });
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
                if (changePasswordViewModel.PasswordEncrypt != changePasswordViewModel.ConfirmPasswordEncrypt)
                {
                    TempData["toast"] = "Contraseñas distintas, verifique su información";
                    band = false;
                }

                var usuario = await _context.Usuarios.FindAsync(token.UsuarioID);
                if (usuario == null)
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
                        usuario.CambiarPassword = false;

                        await _context.SaveChangesAsync();
                        TempData["toast"] = "Su contraseña ha sido actualizada con éxito.";

                        return RedirectToAction("Sesion", "Home");
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

        public async Task<IActionResult> ResetPassword(Guid id)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            var rand = new Random();

            int pwd = rand.Next(1000, 10000);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                TempData["toast"] = "Contraseñas distintas, verifique su información";
                return RedirectToAction("Index", "Usuarios");
            }

            string pwdSha512 = _criptografiaHelper.GenerateSHA512String(pwd.ToString());
            try
            {
                usuario.Password = _criptografiaHelper.Encrypt(pwdSha512);
                usuario.CambiarPassword = true;
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["toast"] = $"La contraseña ha sido reiniciada con éxito. Contraseña temporal: {pwd}";
            }
            catch (Exception)
            {
                TempData["toast"] = "Error al reiniciar la contraseña.";
            }
            
            return RedirectToAction("Index", "Usuarios");
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

            string ruta = Path.Combine(directorioImagenPerfil, tipo, $"{id}.png");

            if (!System.IO.File.Exists(ruta))
            {
                ruta = Path.Combine(_webHostEnvironment.WebRootPath, "images", "perfil", "user-slash.png");
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
