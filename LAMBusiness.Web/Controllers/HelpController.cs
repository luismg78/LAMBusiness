namespace LAMBusiness.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewIndex()
        {
            return PartialView();
        }
        public IActionResult ViewCreate()
        {
            return PartialView();
        }
        public IActionResult ViewEdit()
        {
            return PartialView();
        }
    }
}
