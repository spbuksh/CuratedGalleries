using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corbis.Public.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.Public.Entity;
using Corbis.Public.Web.Code;
using Corbis.Public.Entity.Search;
using Corbis.Common;
using Corbis.Presentation.Common.Models;
using Corbis.Common.ObjectMapping.Interface;
using System.Globalization;

namespace Corbis.Pubic.Web.Controllers
{
    [Authorize]
    public class HomeController : PublicControllerBase
    {
        [Dependency]
        public ISearchRepository SearchRepository { get; set; }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

    }
}
