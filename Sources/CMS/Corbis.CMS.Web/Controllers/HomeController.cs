using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Corbis.CMS.Web.Controllers
{
    public class HomeController : CMSControllerBase
    {
        public ActionResult Index()
        {
            return this.RedirectToAction("Index", "Gallery");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
