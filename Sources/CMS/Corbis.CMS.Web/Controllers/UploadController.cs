using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Corbis.CMS.Web.Controllers
{
    public class UploadController : CMSControllerBase
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}