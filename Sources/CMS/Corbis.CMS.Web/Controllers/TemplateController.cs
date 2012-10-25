using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corbis.CMS.Web.Code;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Controllers
{
    public class TemplateController : CMSControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index", GalleryRuntime.GetTemplates());
        }

        [HttpPost]
        public ActionResult UploadTemplate(HttpPostedFileBase file)
        {
            byte[] content = new byte[file.ContentLength];

            file.InputStream.Read(content, 0, file.ContentLength);
            file.InputStream.Close();

            var template = GalleryRuntime.AddTemplate(new ZipArchivePackage() { FileName = file.FileName, FileContent = content });

            return this.Index();
        }

    }
}
