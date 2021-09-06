namespace LAMBusiness.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
