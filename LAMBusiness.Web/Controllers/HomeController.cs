namespace LAMBusiness.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Helpers;
    using Models;
    using Models.ViewModels;
    using Shared.Aplicacion;
    using Shared.Contacto;
    using System;
    using Newtonsoft.Json;

    public class HomeController : GlobalController
    {
        private readonly DataContext _context;
        private readonly ICriptografiaHelper _criptografia;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;

        public HomeController(DataContext context, 
            ICriptografiaHelper criptografia,
            IConfiguration configuration,
            IGetHelper getHelper)
        {
            _context = context;
            _criptografia = criptografia;
            _configuration = configuration;
            _getHelper = getHelper;;
        }

        public IActionResult ErrorDeConexion()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SignIn()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "signin", false);
            if (validateToken != null) { return validateToken; }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("Email, Password")] InicioSesionViewModel inicioSesionViewModel)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "signin", false);
            if (validateToken != null) { return validateToken; }

            if (ModelState.IsValid)
            {
                string email = inicioSesionViewModel.Email.Trim().ToLower();

                Usuario usuario = await (from u in _context.Usuarios
                                         join c in _context.Colaboradores 
                                         on u.ColaboradorID equals c.ColaboradorID
                                         where c.Email == email
                                         select u).FirstOrDefaultAsync();

                if(usuario == null)
                {
                    TempData["toast"] = "Correo electrónico inexistente, verifique";
                    return View(inicioSesionViewModel);
                }

                var pwd = _criptografia.Encrypt(inicioSesionViewModel.Password);
                if(usuario.Password != pwd)
                {
                    TempData["toast"] = "Credenciales Incorrectas, verifique";
                    return View(inicioSesionViewModel);
                }
                
                HttpContext.Session.SetString("LAMBusiness", HttpContext.Session.Id);
                string sessionId = HttpContext.Session.GetString("LAMBusiness");

                string directorioSesion = _configuration.GetValue<string>("DirectorioSesion");

                var resultado = await _getHelper
                    .SetTokenByUsuarioIDAsync(sessionId, usuario.UsuarioID, directorioSesion);

                if (resultado.Error)
                {
                    ModelState.AddModelError(string.Empty, resultado.Mensaje);
                    return View(inicioSesionViewModel);
                }

                TempData["toast"] = "¡Qué gusto tenerte de vuelta!";
                await BitacoraAsync("InicioSesion", resultado);

                return RedirectToAction("Inicio", "menu");
            }

            ModelState.AddModelError(string.Empty, "Credenciales Incorrectas");
            return View(inicioSesionViewModel);
        }

        public async Task<IActionResult> PaginaEnConstruccion()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task BitacoraAsync(string accion, Resultado<Token> token, string excepcion = "")
        {
            Guid moduloId = Guid.Parse("4C4CD77D-E11F-4A69-AC1C-331A022A5718");
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token.Contenido, accion, moduloId,
                token, token.Contenido.ColaboradorID.ToString(), directorioBitacora, excepcion);
        }
    }
}
