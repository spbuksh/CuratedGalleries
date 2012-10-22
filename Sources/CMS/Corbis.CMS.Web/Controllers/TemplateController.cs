using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Corbis.CMS.Web.Controllers
{
    public class TemplateController : CMSControllerBase
    {
        //
        // GET: /Template/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateTemplate()
        {
            return View("Index");
        }

    }
}
