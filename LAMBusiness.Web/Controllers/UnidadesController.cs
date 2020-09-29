namespace LAMBusiness.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using LAMBusiness.Shared.Catalogo;
    using LAMBusiness.Web.Data;

    public class UnidadesController : Controller
    {
        private readonly DataContext _context;

        public UnidadesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var unidad = _context.Unidades
                .Include(u => u.Productos);

            return View(unidad);
        }
    }
}
