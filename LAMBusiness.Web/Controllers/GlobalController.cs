namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Helpers;
    using Shared.Aplicacion;
    using Shared.Contacto;

    public class GlobalController : Controller
    {
        public enum eTipoPermiso
        {
            PermisoLectura = 0,
            PermisoEscritura = 1,
            PermisoImprimir = 2
        }

        public Token token { get; set; }
        public UsuarioModulo permisosModulo { get; set; }

        /// <summary>
        /// Menú seleccionado por el usuario
        /// </summary>
        private void SetMenuSelected(string value)
        {
            if (TempData != null)
            {
                if (TempData.ContainsKey("Home"))
                {
                    TempData["Home"] = value;
                }
                else
                {
                    TempData.Add("Home", value);
                }
            }
            else
            {
                TempData.Add("Home", value);
            }
        }
    
        /// <summary>
        /// Validar si el equipo cliente tiene acceso a la red e internet.
        /// </summary>
        /// <returns></returns>
        private bool SetConnectionActive(IConfiguration configuration)
        {
            bool conexionRedLocal = configuration.GetValue<bool>("ConexionRedLocal");
            bool conexionInternet = configuration.GetValue<bool>("ConexionInternet");
            bool redActiva = true;

            if (conexionRedLocal)
            {
                //validar si la red local está activa
                redActiva = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                if (redActiva)
                {
                    //validar la conexión a internet con la cuenta de google.
                    if (conexionInternet)
                    {
                        Uri url = new Uri("https://www.google.com/");
                        System.Net.WebRequest WebRequest;
                        WebRequest = System.Net.WebRequest.Create(url);
                        System.Net.WebResponse objetoResp;

                        try
                        {
                            objetoResp = WebRequest.GetResponse();
                            objetoResp.Close();
                        }
                        catch (Exception)
                        {
                            TempData["toast"] = "Verifique su conexión a Internet";
                            redActiva = false;
                        }

                        WebRequest = null;
                    }
                }
                else
                {
                    TempData["toast"] = "Verifique su conexión a la red";
                }
            }
            return redActiva;
        }

        /// <summary>
        /// Validar el token del usuario.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="getHelper"></param>
        /// <returns></returns>
        private async Task<Token> GetTokenActive(IConfiguration configuration, IGetHelper getHelper)
        {
            string sessionId = HttpContext.Session.GetString("LAMBusiness");
            string directorioSesion = configuration.GetValue<string>("DirectorioSesion");

            if (sessionId == null)
                return null;

            var resultado = await getHelper
                .GetTokenBySessionIdAndUsuarioIDAsync(sessionId, directorioSesion);

            if (resultado.Error)
            {
                TempData["toast"] = "Por favor, ingrese sus credenciales, para accesar al sistema.";
                //Response.Cookies.Delete("LAMBusiness_SessionId");
                return null;
            }
            else
            {
                token = resultado.Contenido;
                ViewData["token"] = token;
                ViewBag.Id = token.UsuarioID;
            }

            return resultado.Contenido;
        }

        public async Task<bool> ValidateModulePermissions(IGetHelper getHelper, Guid moduloId, eTipoPermiso tipoPermiso)
        {
            permisosModulo = new UsuarioModulo()
            {
                ModuloID = moduloId,
                PermisoEscritura = false,
                PermisoImprimir = false,
                PermisoLectura = false
            };

            if (token.Administrador == "SA" || token.Administrador == "GA")
            {
                permisosModulo.PermisoEscritura = true;
                permisosModulo.PermisoImprimir = true;
                permisosModulo.PermisoLectura = true;
                return true;
            }

            if (token.UsuariosModulos.Contains(moduloId))
            {
                return true;
            }

            var permisos = await getHelper.GetUsuarioModuloByUsuarioAndModuloIDAsync(token.UsuarioID, moduloId);
            if (permisos == null)
            {
                TempData["toast"] = "No tiene privilegios en el módulo";
                return false;
            }

            permisosModulo = permisos;

            switch (tipoPermiso)
            {
                case eTipoPermiso.PermisoLectura:
                    if (!permisos.PermisoLectura)
                    {
                        TempData["toast"] = "No tiene privilegios de acceso al módulo";
                        return false;
                    }
                    break;
                case eTipoPermiso.PermisoEscritura:
                    if (!permisos.PermisoLectura || !permisos.PermisoEscritura)
                    {
                        TempData["toast"] = "No tiene privilegios de escritura en el módulo";
                        return false;
                    }
                    break;
                case eTipoPermiso.PermisoImprimir:
                    if (!permisos.PermisoLectura || !permisos.PermisoImprimir)
                    {
                        TempData["toast"] = "No tiene privilegios de imprimir en el módulo";
                        return false;
                    }
                    break;
            }

            return true;
        }

        public async Task<IActionResult> ValidatedToken(IConfiguration configuration, IGetHelper getHelper, string menuSelected, bool validateToken = true)
        {
            if (!SetConnectionActive(configuration))
            {
                return RedirectToAction("ErrorDeConexion", "Home");
            }

            if (validateToken)
            {
                if (await GetTokenActive(configuration, getHelper) == null)
                {
                    return RedirectToAction("SignIn", "Home"); 
                }
            }

            SetMenuSelected(menuSelected);

            return null;
        }
    }
}
