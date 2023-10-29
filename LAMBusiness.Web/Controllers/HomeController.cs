namespace LAMBusiness.Web.Controllers
{
    using Helpers;
    using LAMBusiness.Backend;
    using LAMBusiness.Contextos;
    using LAMBusiness.Shared.Dashboard;
    using LAMBusiness.Shared.DTO.Sesion;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Shared.Aplicacion;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;
        private readonly Sesiones _sesion;

        public HomeController(DataContext context,
            IConfiguration configuration,
            IGetHelper getHelper)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
            _sesion = new Sesiones(context);
        }

        public IActionResult ErrorDeConexion()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null)
            {
                TempData["toast"] = "Ingrese sus credenciales";
                return validateToken;
            }

            return View(token);
        }

        public async Task<IActionResult> SignIn()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "signin", false);
            if (validateToken != null) { return validateToken; }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("Email, Password")] InicioDeSesionDTO inicioDeSesion)
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "signin", false);
            if (validateToken != null) { return validateToken; }

            if (ModelState.IsValid)
            {
                string email = inicioDeSesion.Email.Trim().ToLower();

                var resultado = await _sesion.IniciarSesion(inicioDeSesion);

                if (resultado.Error)
                {
                    TempData["toast"] = resultado.Mensaje;
                    return View(inicioDeSesion);
                }

                var usuario = resultado.Datos;

                HttpContext.Session.SetString("LAMBusiness", HttpContext.Session.Id);
                string sessionId = HttpContext.Session.GetString("LAMBusiness");

                string directorioSesion = _configuration.GetValue<string>("DirectorioSesion");

                var resultadoToken = await _getHelper
                    .SetTokenByUsuarioIDAsync(sessionId, usuario.UsuarioID, directorioSesion);

                if (resultadoToken.Error)
                {
                    ModelState.AddModelError(string.Empty, resultadoToken.Mensaje);
                    TempData["toast"] = resultadoToken.Mensaje;
                    return View(inicioDeSesion);
                }

                TempData["toast"] = "¡Qué gusto tenerte de vuelta!";
                await BitacoraAsync("InicioSesion", resultadoToken);

                if (usuario.CambiarPassword)
                    return RedirectToAction("ChangePassword", "Sesion");
                else
                    return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Credenciales Incorrectas");
            return View(inicioDeSesion);
        }

        public async Task<IActionResult> PaginaEnConstruccion()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }
            return View();
        }

        public async Task<IActionResult> Aplicacion()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "aplicacion");
            if (validateToken != null) { return validateToken; }

            if (token.Administrador != "SA")
            {
                TempData["toast"] = "No tiene privilegios de acceso en el módulo";
                return RedirectToAction("Index", "Home");
            }

            Guid moduloId = Guid.Parse("37A8C12A-254F-44FB-BE68-67AF358B0610");

            var aplicacion = new Aplicacion()
            {
                Modulos = GetCountModulos(),
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)
            };

            return View(aplicacion);
        }

        public async Task<IActionResult> Catalogo()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("50B65B8C-1CBA-47E4-8327-5F1A34375394");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            var catalogo = new Catalogo()
            {
                Almacenes = GetCountAlmacenes(),
                //Generos = GetCountGeneros(),
                //EstadosCiviles = GetCountEstadosCiviles(),
                //Estados = GetCountEstados(),
                FormasPago = GetCountFormasPago(),
                Marcas = GetCountMarcas(),
                //Municipios = GetCountMunicipios(),
                Productos = GetCountProductos(),
                Puestos = GetCountPuestos(),
                SalidasTipo = GetCountSalidasTipo(),
                TasasImpuestos = GetCountTasasImpuestos(),
                //Unidades = GetCountUnidades(),
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)
            };
            return View(catalogo);
        }

        public async Task<IActionResult> Contacto()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("25C76712-5552-44C5-93A1-298590F337FA");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            var contacto = new Contacto()
            {
                Clientes = GetCountClientes(),
                Colaboradores = GetCountColaboradores(),
                Proveedores = GetCountProveedores(),
                Usuarios = GetCountUsuarios(),
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)

            };

            return View(contacto);
        }

        public async Task<IActionResult> Dashboard()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "dashboard");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("C803EECE-79A9-4B7F-955C-3A0CC70BFEDB");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            var dashboard = new Dashboard()
            {
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)
            };

            return View(dashboard);
        }

        public async Task<IActionResult> Movimiento()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("4BA5D993-8BEB-48AF-A45F-4813825A658F");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Index));
            }

            var movimiento = new Movimiento()
            {
                CorteDeCaja = GetCountCortesDeCajas(),
                Devoluciones = GetCountDevoluciones(),
                Entradas = GetCountEntradas(),
                RetiroDeCaja = GetCountRetiros(),
                Salidas = GetCountSalidas(),
                Ventas = GetCountVentas(),
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)
            };

            return View(movimiento);
        }

        public async Task<IActionResult> Sesion()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "sesion");
            if (validateToken != null) { return validateToken; }

            return View();
        }

        #region Aplicacation's counts
        private int GetCountModulos()
        {
            return _context.Modulos.Count();
        }
        #endregion

        #region Category's counts
        private int GetCountAlmacenes()
        {
            return _context.Almacenes.Count();
        }

        private int GetCountGeneros()
        {
            return _context.Generos.Count();
        }
        private int GetCountEstados()
        {
            return _context.Estados.Count();
        }

        private int GetCountEstadosCiviles()
        {
            return _context.EstadosCiviles.Count();
        }

        private int GetCountFormasPago()
        {
            return _context.FormasPago.Count();
        }

        private int GetCountMarcas()
        {
            return _context.Marcas.Count();
        }

        private int GetCountMunicipios()
        {
            return _context.Municipios.Count();
        }

        private int GetCountProductos()
        {
            return _context.Productos.Count();
        }

        private int GetCountPuestos()
        {
            return _context.Puestos.Count();
        }

        private int GetCountSalidasTipo()
        {
            return _context.SalidasTipo.Count();
        }

        private int GetCountTasasImpuestos()
        {
            return _context.TasasImpuestos.Count();
        }

        private int GetCountUnidades()
        {
            return _context.Unidades.Count();
        }
        #endregion

        #region Contact's counts
        private int GetCountClientes()
        {
            return _context.Clientes.Count();
        }

        private int GetCountColaboradores()
        {
            return _context.DatosPersonales.Where(c => c.CURP != "CURP781227HCSRNS00").Count();
        }

        private int GetCountProveedores()
        {
            return _context.Proveedores.Count();
        }

        private int GetCountUsuarios()
        {
            return _context.Usuarios.Where(u => u.AdministradorID != "SA").Count();
        }

        #endregion

        #region Movimiento's counts
        private int GetCountCortesDeCajas()
        {
            return 0; //_context.corte.Where(r => r.VentaCierreID == Guid.Empty).Count();
        }
        private int GetCountDevoluciones()
        {
            //cambiar entradas por devoluciones
            return _context.Entradas.Count();
        }
        private int GetCountEntradas()
        {
            return _context.Entradas.Count();
        }
        private int GetCountRetiros()
        {
            return _context.RetirosCaja.Where(r => r.VentaCierreID == Guid.Empty).Count();
        }
        private int GetCountSalidas()
        {
            return _context.Salidas.Count();
        }
        private int GetCountVentas()
        {
            return _context.Ventas.Count();
        }
        #endregion        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task BitacoraAsync(string accion, Resultado<Token> token, string excepcion = "")
        {
            Guid moduloId = Guid.Parse("4C4CD77D-E11F-4A69-AC1C-331A022A5718");
            string directorioBitacora = _configuration.GetValue<string>("DirectorioBitacora");

            await _getHelper.SetBitacoraAsync(token.Datos, accion, moduloId,
                token, token.Datos.UsuarioID.ToString(), directorioBitacora, excepcion);
        }
    }
}
