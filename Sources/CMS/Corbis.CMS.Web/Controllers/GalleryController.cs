using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corbis.CMS.Web.Code;
using Corbis.CMS.Entity;
using System.ComponentModel.DataAnnotations;
using Corbis.Presentation.Common;
using Corbis.CMS.Web.Models;
using System.IO;
using System.Net;
using Ionic.Zip;
using Corbis.CMS.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.Common;
using Corbis.CMS.Repository.Interface.Communication;

namespace Corbis.CMS.Web.Controllers
{
    public class GalleryController : CMSControllerBase
    {
        [Dependency]
        public ICuratedGalleryRepository GalleryRepository { get; set; }

        /// <summary>
        /// Gallety index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            OperationResult<OperationResults, List<CuratedGallery>> rslt = null;

            try
            {
                rslt = this.GalleryRepository.GetGalleries();
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                throw;
            }

            switch(rslt.Result)
            {
                case OperationResults.Success:
                case OperationResults.NotFound:
                    break;
                case OperationResults.Failure:
                    throw new Exception("...");
                default:
                    throw new NotImplementedException();
            }

            var galleries = new List<GalleryItemModel>();

            foreach (var item in rslt.Output)
                galleries.Add(this.ObjectMapper.DoMapping<GalleryItemModel>(item));

            return View("Index", galleries);
        }

        #region Create Gallery

        [HttpGet]
        [ActionName("CreateGallery")]
        public ActionResult CreateGallery_GET()
        {
            var model = new CreateGalleryModel() { Name = null };

            foreach (var template in GalleryRuntime.GetTemplates())
            {
                var item = this.Convert(template);

                if (template.IsDefault)
                    model.TemplateID = item.ID;

                model.Templates.Add(item);
            }

            return this.View("CreateGallery", model);
        }
        [HttpPost]
        [ActionName("CreateGallery")]
        public ActionResult CreateGallery_POST([Bind(Exclude = "Templates")]CreateGalleryModel model)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var template in GalleryRuntime.GetTemplates())
                    model.Templates.Add(this.Convert(template));

                return this.View("CreateGallery", model);
            }

            var gallery = GalleryRuntime.CreateGallery(model.Name, model.TemplateID);
            return this.RedirectToAction("EditGallery", "Gallery", new { id = gallery.ID });
        }

        private GalleryTemplateModel Convert(IGalleryTemplate template)
        {
            var output = this.ObjectMapper.DoMapping<GalleryTemplateModel>(template);

            if (template.Icon != null)
            {
                switch (template.Icon.Type)
                {
                    case ImageSourceTypes.Url:
                        output.ImageUrl = template.Icon.Source;
                        break;
                    case ImageSourceTypes.LocalFile:
                        output.ImageUrl = Utils.AbsoluteToVirtual(Path.Combine(GalleryRuntime.GetTemplatePath(template.ID), template.Icon.Source), this.HttpContext);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                output.ImageUrl = GalleryRuntime.DefaultTemplateImageUrl.Small;
            }

            return output;
        }

        #endregion Create/Edit Gallery

        #region Edit Gallery

        protected GalleryContentImageModel Convert(GalleryContentImage item, int galleryID)
        {
            var model = new GalleryContentImageModel() { GalleryID = galleryID, ID = item.ID, Urls = item.SiteUrls, Text = item.Name, Order = item.Order };

            if (item.TextContent != null)
            {
                switch (item.TextContent.ContentType)
                {
                    case TextContents.None:
                        model.TextContent = this.ObjectMapper.DoMapping<EmptyTextContentModel>(item.TextContent);
                        break;
                    case TextContents.QnA:
                        model.TextContent = this.ObjectMapper.DoMapping<QnATextContentModel>(item.TextContent);
                        break;
                    case TextContents.Pullquote:
                        model.TextContent = this.ObjectMapper.DoMapping<PullQuotedTextContentModel>(item.TextContent);
                        break;
                    case TextContents.BodyCopy:
                        model.TextContent = this.ObjectMapper.DoMapping<BodyCopyTextContentModel>(item.TextContent);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                model.TextContent = new EmptyTextContentModel() { Position = null };
            }

            return model;
        }

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("EditGallery")]
        public ActionResult EditGallery_GET(Nullable<int> id)
        {
            CuratedGallery gallery = null;

            if (id.HasValue)
            {
                gallery = GalleryRuntime.GetGallery(id.Value);
            }
            else
            {
                gallery = GalleryRuntime.CreateGallery("New gallery", GalleryRuntime.GetTemplates().First().ID);
            }

            //
            var model = this.ObjectMapper.DoMapping<GalleryModel>(gallery);

            var content = gallery.LoadContent();

            if (content.Images != null && content.Images.Count != 0)
            {
                model.ContentImages = new List<GalleryContentImageModel>();

                foreach (var item in content.Images)
                    model.ContentImages.Add(this.Convert(item, gallery.ID));

                model.ContentImages.Sort(delegate(GalleryContentImageModel x, GalleryContentImageModel y) { return x.Order - y.Order; });
            }

            return this.View("Gallery", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Gallery id</param>
        /// <param name="name">New gallery name</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RenameGallery(int id, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears gallery content
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClearGalleryContent([Required]Nullable<int> id)
        {
            var content = GalleryRuntime.LoadGalleryContent(id.Value);
            content.DeleteContentImages(content.Images.Select(x => x.ID), this.HttpContext);
            GalleryRuntime.SaveGalleryContent(id.Value, content);
            return this.Json(new { success = true });
        }

        
        [HttpPost]
        public ActionResult SetEmptyContent(int galleryID, string imageID, EmptyTextContentModel model)
        {
            Action<GalleryContentImage> handler = delegate(GalleryContentImage item) { item.TextContent = this.ObjectMapper.DoMapping<EmptyTextContent>(model); };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetQnAContent(int galleryID, string imageID, QnATextContentModel model)
        {
            if (!this.ModelState.IsValid)
                this.PartialView("QnATextContentPartial", model);

            Action<GalleryContentImage> handler = delegate(GalleryContentImage item) { item.TextContent = this.ObjectMapper.DoMapping<QnATextContent>(model); };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetPullQuotedContent(int galleryID, string imageID, PullQuotedTextContentModel model)
        {
            if(!this.ModelState.IsValid)
                return this.PartialView("PullQuotedTextContentPartial", model);

            Action<GalleryContentImage> handler = delegate(GalleryContentImage item) { item.TextContent = this.ObjectMapper.DoMapping<PullQuotedTextContent>(model); };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetBodyCopyContent(int galleryID, string imageID, BodyCopyTextContentModel model)
        {
            if(!this.ModelState.IsValid)
                this.PartialView("BodyCopyTextContentPartial", model);

            Action<GalleryContentImage> handler = delegate(GalleryContentImage item) { item.TextContent = this.ObjectMapper.DoMapping<BodyCopyTextContent>(model); };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }

        public ActionResult SwapImageOrder(int galleryID, string imageID1, string imageID2)
        {
            var content = GalleryRuntime.LoadGalleryContent(galleryID);

            var images = content.Images.Where(x => x.ID == imageID1 || x.ID == imageID2).ToArray();

            if (images.Length != 2)
                return this.Json(new { success = false, error = "Page data is obsolete. Please refresh page and try again"});

            var order = images[0].Order;
            images[0].Order = images[1].Order;
            images[1].Order = order;

            content.Images.Sort(delegate(GalleryContentImage x, GalleryContentImage y) { return x.Order - y.Order; });

            GalleryRuntime.SaveGalleryContent(galleryID, content);

            return this.Json(new { success = true });
        }

        #endregion Edit Gallery

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Template identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GalleryTemplatePreview(int id)
        {
            return this.PartialView("GalleryTemplatePreviewPartial", this.Convert(GalleryRuntime.GetTemplate(id)));
        }

        /// <summary>
        /// Deletes gallery
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteGallery(int id)
        {
            return this.Json(new { success = GalleryRuntime.DeleteGallery(id) });
        }

        /// <summary>
        /// NOTE: This ation is used explicitly in route registration. So if you want to rename it then you have to rename in Global.asax.RegisterRoutes too
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuildGallery([Required]Nullable<int> id)
        {
            var gallery = GalleryRuntime.BuildGalleryOutput(id.Value);

            //gallery was not found
            if (gallery == null)
                return this.HttpNotFound(string.Format("Gallery with identifier '{0}' was not found", id.Value));

            string url = gallery.GetPreviewUrl(this.HttpContext);

            //TODO: Transfer problem!!!
            //return new TransferActionResult(string.Format("~/{0}", url.TrimStart('/')), true);
            return this.Redirect(url);
        }

        /// <summary>
        /// Downloads gallery as zip archive
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public ActionResult Download(int id)
        {
            //TODO: File/folder filter for package has not been implemented. For example we do not need xml gallery state file.
            byte[] output = null;

            using (var package = new ZipFile())
            {
                var root = GalleryRuntime.GetGalleryPath(id);

                package.AddFiles(Directory.GetFiles(root));

                foreach (var item in Directory.GetDirectories(root))
                    package.AddDirectory(item, item.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar));

                using(MemoryStream stream = new MemoryStream())
                {
                    package.Save(stream);

                    stream.Position = 0;
                    output = stream.ToArray();
                }
            }

            return this.File(output, "application/zip");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="galleryID">Gallery identifier</param>
        /// <param name="id">Image identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetContentImage([Required]Nullable<int> galleryID, [Required]string id)
        {
            var content = GalleryRuntime.LoadGalleryContent(galleryID.Value);

            var image = content.Images.Where(x => string.Equals(x.ID, id, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (image == null)
                return this.Json(new { success = false, error = string.Format("Image with id='{0}' was not found. Gallery id='{1}'", id, galleryID.Value) });

            return this.PartialView("ContentImagePartial", this.Convert(image, galleryID.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="galleryID">Gallery identifier</param>
        /// <param name="id">Image identifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteContentImage([Required]Nullable<int> galleryID, [Required]string id)
        {
            var content = GalleryRuntime.LoadGalleryContent(galleryID.Value);
            content.DeleteContentImages(new string[] { id }, this.HttpContext);
            GalleryRuntime.SaveGalleryContent(galleryID.Value, content);
            return this.Json(new { success = true });
        }


        /// <summary>
        /// Uploads images into the gallery
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UploadImage(Nullable<int> id)
        {
            //TODO: This method does not work // WHY?

            if (!this.ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Request parameters are not valid");

            if (this.Request.Files.Count != 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Upload image request must have 1 image for uploading");

            string uploadRoot = Path.Combine(GalleryRuntime.GetGalleryPath(id.Value), "Images");

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            var file = this.Request.Files[0];

            if (GalleryRuntime.MaxImageSize.HasValue && GalleryRuntime.MaxImageSize.Value < file.ContentLength)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Max file size {1}bytes is exceeded", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            if (GalleryRuntime.MinImageSize.HasValue && file.ContentLength < GalleryRuntime.MinImageSize.Value)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Min file size is {1}bytes", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            string filepath = Path.Combine(uploadRoot, file.FileName);

            try
            {
                file.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex, string.Format("Uploaded file cannot be saved to '{0}'", filepath));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK, new Uri(filepath).AbsoluteUri);
        }
    }
}
