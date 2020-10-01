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
                Generos = GetCountGeneros(),
                EstadosCiviles = GetCountEstadosCiviles(),
                Estados = GetCountEstados(),
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

        #region Category's counts
        private int GetCountGeneros()
        {
            return _context.Generos.Count();
        }

        private int GetCountEstadosCiviles()
        {
            return _context.EstadosCiviles.Count();
        }

        private int GetCountProductos()
        {
            return _context.Productos.Count();
        }

        private int GetCountPuestos()
        {
            return _context.Puestos.Count();
        }

        private int GetCountEstados()
        {
            return _context.Estados.Count();
        }

        private int GetCountMunicipios()
        {
            return _context.Municipios.Count();
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

    }
}
