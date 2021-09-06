namespace LAMBusiness.Web.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Data;
    using Shared.Dashboard;
    using System;
    using System.Threading.Tasks;
    using LAMBusiness.Web.Helpers;

    public class MenuController : GlobalController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IGetHelper _getHelper;

        public MenuController(DataContext context, IConfiguration configuration, IGetHelper getHelper)
        {
            _context = context;
            _configuration = configuration;
            _getHelper = getHelper;
        }
        
        public async Task<IActionResult> Inicio()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "home");
            if (validateToken != null) { return validateToken; }
            
            return View(token);
        }

        public async Task<IActionResult> Catalogo()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "catalogo");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("50B65B8C-1CBA-47E4-8327-5F1A34375394");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Inicio));
            }

            var catalogo = new Catalogo()
            {
                Almacenes = GetCountAlmacenes(),
                Generos = GetCountGeneros(),
                EstadosCiviles = GetCountEstadosCiviles(),
                Estados = GetCountEstados(),
                FormasPago = GetCountFormasPago(),
                Marcas = GetCountMarcas(),
                Municipios = GetCountMunicipios(),
                Productos = GetCountProductos(),
                Puestos = GetCountPuestos(),
                SalidasTipo = GetCountSalidasTipo(),
                TasasImpuestos = GetCountTasasImpuestos(),
                Unidades = GetCountUnidades(),
                ModulosMenu = await _getHelper.GetModulesByUsuarioIDAndModuloPadreID(token.UsuarioID, moduloId)
            };
            return View(catalogo);
        }

        public async Task<IActionResult> Contacto()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "contacto");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("25C76712-5552-44C5-93A1-298590F337FA");
            if (! await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Inicio));
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
        
        public async Task<IActionResult> Movimiento()
        {
            var validateToken = await ValidatedToken(_configuration, _getHelper, "movimiento");
            if (validateToken != null) { return validateToken; }

            Guid moduloId = Guid.Parse("4BA5D993-8BEB-48AF-A45F-4813825A658F");
            if (!await ValidateModulePermissions(_getHelper, moduloId, eTipoPermiso.PermisoLectura))
            {
                return RedirectToAction(nameof(Inicio));
            }

            var movimiento = new Movimiento()
            {
                Devoluciones = GetCountDevoluciones(),
                Entradas = GetCountEntradas(),
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
            return _context.Colaboradores.Where(c => c.CURP != "CURP781227HCSRNS00").Count();
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
        private int GetCountDevoluciones()
        {
            //cambiar entradas por devoluciones
            return _context.Entradas.Count();
        }
        private int GetCountEntradas()
        {
            return _context.Entradas.Count();
        }

        private int GetCountSalidas()
        {
            return _context.Salidas.Count();
        }

        private int GetCountVentas()
        {
            return 0;
        }
        #endregion        
    }
}
