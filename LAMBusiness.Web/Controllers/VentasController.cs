namespace LAMBusiness.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System.Threading.Tasks;
    using Helpers;
    using Models.ViewModels;

    public class VentasController : Controller
    {
        private readonly IGetHelper _getHelper;

        public VentasController(IGetHelper getHelper)
        {
            _getHelper = getHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProductByCode(string codigo, decimal cantidad)
        {
            codigo = codigo.Trim().ToUpper();

            if (cantidad == 0)
                return new EmptyResult();

            var producto = await _getHelper.GetProductByCodeAsync(codigo);
            if (producto == null)
                return new EmptyResult();

            if (!producto.Activo)
                return new EmptyResult();

            var _producto = new ProductoViewModel()
            {
                CantidadProducto = cantidad,
                Codigo = producto.Codigo,
                ProductoID = producto.ProductoID,
                ProductoNombre = producto.ProductoNombre,
                PrecioVenta = producto.PrecioVenta
            };

            return PartialView(_producto);
        }

    }
}
