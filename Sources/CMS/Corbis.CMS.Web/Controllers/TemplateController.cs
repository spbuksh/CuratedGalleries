using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corbis.CMS.Web.Code;
using Corbis.CMS.Entity;
using Corbis.CMS.Web.Models;
using Corbis.Common;

namespace Corbis.CMS.Web.Controllers
{
    public class TemplateController : CMSControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index", Convert(GalleryRuntime.GetTemplates()));
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

        /// <summary>
        /// Deletes template
        /// </summary>
        /// <param name="id">Template identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTemplate(int id)
        {
            return this.Json(new { success = GalleryRuntime.DeleteTemplate(id) });
        }


        private IList<GalleryTemplateModel> Convert(IList<IGalleryTemplate> template)
        {
            var result = new List<GalleryTemplateModel>();

            foreach (var galleryTemplate in template)
            {
                var output = this.ObjectMapper.DoMapping<GalleryTemplateModel>(galleryTemplate);

                if (galleryTemplate.Icon != null)
                {
                    switch (galleryTemplate.Icon.Type)
                    {
                        case ImageSourceTypes.Url:
                            output.ImageUrl = galleryTemplate.Icon.Source;
                            break;
                        case ImageSourceTypes.LocalFile:
                            output.ImageUrl = Utils.AbsoluteToVirtual(Path.Combine(GalleryRuntime.GetTemplatePath(galleryTemplate.ID), galleryTemplate.Icon.Source), this.HttpContext);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    output.ImageUrl = GalleryRuntime.DefaultTemplateImageUrl.Small;
                }

                result.Add(output);
            }


            return result;
        }

    }
}
