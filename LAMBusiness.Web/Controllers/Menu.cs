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
                Estados = GetCountEstados(),
                Municipios = GetCountMunicipios(),
                Puestos = GetCountPuestos()
            };
            return View(catalogo);
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
    }
}
