namespace LAMBusiness.Web.Controllers
{
    using System.Diagnostics;
    using LAMBusiness.Web.Data;
    using LAMBusiness.Web.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.ViewModels;

    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly ICriptografiaHelper _criptografia;

        public HomeController(DataContext context, ICriptografiaHelper criptografia)
        {
            _context = context;
            _criptografia = criptografia;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn([Bind("Email, Password")] InicioSesionViewModel inicioSesionViewModel)
        {
            if (ModelState.IsValid)
            {
                var pwd = _criptografia.Encrypt(inicioSesionViewModel.Password);
                if(inicioSesionViewModel.Email.Trim().ToLower() != "luismg78@gmail.com")
                {
                    ModelState.AddModelError(string.Empty, "Credenciales Incorrectas, verifique");
                    return View(inicioSesionViewModel);
                }
                return View(nameof(Privacy));
            }
            ModelState.AddModelError(string.Empty, "Credenciales Incorrectas");
            return View(inicioSesionViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
