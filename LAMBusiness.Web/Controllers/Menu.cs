namespace LAMBusiness.Web.Controllers
{
    using LAMBusiness.Shared.Dashboard;
    using LAMBusiness.Web.Data;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    public class Menu : Controller
    {
        private readonly DataContext _context;

        public Menu(DataContext context)
        {
            _context = context;
        }
        
        public IActionResult Catalogo()
        {
            var catalogo = new Catalogo()
            {
                Almacenes = GetCountAlmacenes(),
                Generos = GetCountGeneros(),
                EstadosCiviles = GetCountEstadosCiviles(),
                Estados = GetCountEstados(),
                Marcas = GetCountMarcas(),
                Municipios = GetCountMunicipios(),
                Productos = GetCountProductos(),
                Puestos = GetCountPuestos(),
                TasasImpuestos = GetCountTasasImpuestos(),
                Unidades = GetCountUnidades()
            };
            return View(catalogo);
        }

        public IActionResult Contacto()
        {
            var contacto = new Contacto()
            {
                Clientes = GetCountClientes(),
                Colaboradores = GetCountColaboradores(),
                Proveedores = GetCountProveedores()
            };

            return View(contacto);
        }

        public IActionResult Movimiento()
        {
            var movimiento = new Movimiento()
            {
                Entradas = GetCountEntradas(),
                Salidas = GetCountSalidas(),
                Ventas = GetCountVentas()
            };

            return View(movimiento);
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
            return _context.Colaboradores.Count();
        }

        private int GetCountProveedores()
        {
            return _context.Proveedores.Count();
        }
        #endregion

        #region Movimiento's counts
        private int GetCountEntradas()
        {
            return _context.Entradas.Count();
        }

        private int GetCountSalidas()
        {
            return 0;
        }

        private int GetCountVentas()
        {
            return 0;
        }
        #endregion
    }
}
