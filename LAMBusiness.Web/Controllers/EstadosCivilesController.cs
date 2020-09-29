namespace LAMBusiness.Web.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Data;

    public class EstadosCivilesController : Controller
    {
        private readonly DataContext _context;

        public EstadosCivilesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.EstadosCiviles.OrderBy(e => e.EstadoCivilDescripcion));
        }
        
    }
}
