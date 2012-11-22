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
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;

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
            {
                var model = this.ObjectMapper.DoMapping<GalleryItemModel>(item);
                model.TemplateName = GalleryRuntime.GetTemplate(item.TemplateID).Name;
                galleries.Add(model);
            }

            return View("Index", galleries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public ActionResult GetGalleryCover(int id, int? height, int? width)
        {
            Image src = null;

            //it is necessary to ensure that cover is not being deleting or changing at the same time
            lock(GalleryRuntime.GetGallerySyncRoot(id))
            {
                var content = GalleryRuntime.LoadGalleryContent(id, false);

                if(content.CoverImage != null)
                    src = Image.FromFile(Utils.VirtualToAbsolute(content.CoverImage.SiteUrls.Original, this.HttpContext));
            }
            
            if(src == null)
                src = Image.FromFile(GalleryRuntime.GetDefaultGalleryCoverPath(this.HttpContext));

            byte[] buffer = null;

            using (var output = Common.Utilities.Image.ImageHelper.ResizeImage(src, new Size(width.Value, height.Value)))
            {
                using (var memory = new MemoryStream())
                {
                    output.Save(memory, ImageFormat.Jpeg);
                    memory.Position = 0;
                    buffer = memory.ToArray();
                }
            }

            return this.File(buffer, "image/png");
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
            var model = new GalleryContentImageModel() { GalleryID = galleryID, ID = item.ID, Urls = item.EditUrls, Text = item.Name, Order = item.Order };

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

                model.TextContent.Height = item.TextContent.Size.HasValue ? item.TextContent.Size.Value.Height : (int?)null;
                model.TextContent.Width = item.TextContent.Size.HasValue ? item.TextContent.Size.Value.Width : (int?)null;
            }
            else
            {
                model.TextContent = new EmptyTextContentModel() { Position = null };
            }

            return model;
        }
        protected GalleryCoverImageModel Convert(GalleryCoverImage item, int galleryID)
        {
            var model = new GalleryCoverImageModel() 
            { 
                GalleryID = galleryID, 
                ID = item.ID, 
                Urls = item.EditUrls, 
                Text = item.Name, 
                Order = item.Order, 
                Biography = item.Biography,
                TextPosition = item.TextPosition
            };

            model.HeadlineCopyText = item.Headline.Text;
            model.HeadlineCopyFontSize = item.Headline.FontSize;

            model.StandfirstText = item.Standfirst.Text;
            model.StandfirstFontSize = item.Standfirst.FontSize;

            return model;
        }

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LockGallery(int id)
        {
            var rslt = this.GalleryRepository.LockGallery(id, this.CurrentUser.ID);
 
            switch(rslt.Result)
            {
                case OperationResults.Success:
                    return this.Json(new { success = true });
                case OperationResults.NotFound:
                case OperationResults.Failure:
                    return this.Json(new { success = false });
                default:
                    throw new Exception();
            }
        }
        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        public ActionResult UnLockGallery(int id)
        {
            this.GalleryRepository.UnLockGallery(id);
            return this.RedirectToAction("Index", "Gallery");
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

                if (gallery == null)
                    return this.RedirectToAction("Index", "Gallery");
            }
            else
            {
                gallery = GalleryRuntime.CreateGallery("New gallery", GalleryRuntime.GetTemplates().First().ID);
            }
            var template = GalleryRuntime.GetTemplate(gallery.TemplateID);

            var model = this.ObjectMapper.DoMapping<GalleryModel>(gallery);

            var content = gallery.LoadContent();

            model.FontFamily = content.Font.FamilyName;
            model.FontFamilies.AddRange(template.GallerySettings.FontFamilies);
            model.TransitionsIncluded = content.TransitionsIncluded;

            if (content.CoverImage != null)
                model.CoverImage = this.Convert(content.CoverImage, gallery.ID);

            if (content.Images != null && content.Images.Count > 0)
            {
                model.ContentImages = new List<GalleryContentImageModel>();

                for (int i = 1; i < content.Images.Count; i++)
                    model.ContentImages.Add(this.Convert(content.Images[i] as GalleryContentImage, gallery.ID));

                model.ContentImages.Sort(delegate(GalleryContentImageModel x, GalleryContentImageModel y) { return x.Order - y.Order; });
            }

            return this.View("Gallery", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public ActionResult ChangeImageOrder(int galleryID, string firstImageID, string secondImageID)
        {
            GalleryRuntime.ChangeContentImageOrder(galleryID, firstImageID, secondImageID);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Gallery id</param>
        /// <param name="name">New gallery name</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveGalleryAttributes([Required]Nullable<int> id, [Required]string fontFamily, [Required]string name, bool transitionsIncluded)
        {
            if (!this.ModelState.IsValid)
                throw new NotImplementedException();

            var gallery = GalleryRuntime.GetGallery(id.Value);
            gallery.Name = name;
            var result = GalleryRuntime.UpdateGallery(gallery);

            switch(result)
            {
                case OperationResults.Success:
                    {
                        var content = gallery.LoadContent();
                        content.Name = name;
                        content.Font.FamilyName = fontFamily;
                        content.TransitionsIncluded = transitionsIncluded;
                        gallery.SaveContent(content);
                        return this.Json(new { success = true });
                    }
                case OperationResults.NotFound:
                    {
                        return this.Json(new { success = true, error = "The gallery was not found in the storage. Please update the page" });
                    }
                case OperationResults.Failure:
                    {
                        return this.Json(new { success = true, error = "Internal server error" });
                    }
                default:
                    throw new NotImplementedException();

            }
        }

        /// <summary>
        /// Returns all available font families
        /// It is done for page optimization not to load all families at once
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFontFamilies()
        { 
            int lcid = Thread.CurrentThread.CurrentCulture.LCID;
            return this.Json(FontFamily.Families.Select(x => new { text = x.GetName(lcid), value = x.Name }).ToArray(), JsonRequestBehavior.AllowGet);
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
            content.Clear(this.HttpContext);
            GalleryRuntime.SaveGalleryContent(id.Value, content);
            return this.Json(new { success = true });
        }

        
        [HttpPost]
        public ActionResult SetEmptyContent(int galleryID, string imageID, EmptyTextContentModel model)
        {
            Action<GalleryImageBase> handler = delegate(GalleryImageBase item) { (item as GalleryContentImage).TextContent = this.ObjectMapper.DoMapping<EmptyTextContent>(model); };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetQnAContent(int galleryID, string imageID, QnATextContentModel model)
        {
            if (!this.ModelState.IsValid)
                this.PartialView("QnATextContentPartial", model);

            Action<GalleryImageBase> handler = delegate(GalleryImageBase item) 
            {
                (item as GalleryContentImage).TextContent = this.ObjectMapper.DoMapping<QnATextContent>(model);
                (item as GalleryContentImage).TextContent.Size = new Size(model.Width.Value, model.Height.Value);
            };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetPullQuotedContent(int galleryID, string imageID, PullQuotedTextContentModel model)
        {
            if(!this.ModelState.IsValid)
                return this.PartialView("PullQuotedTextContentPartial", model);

            Action<GalleryImageBase> handler = delegate(GalleryImageBase item) 
            {
                (item as GalleryContentImage).TextContent = this.ObjectMapper.DoMapping<PullQuotedTextContent>(model);
                (item as GalleryContentImage).TextContent.Size = new Size(model.Width.Value, model.Height.Value);
            };
            GalleryRuntime.UpdateGalleryContentImage(galleryID, imageID, handler);

            return this.Json(new { success = true });
        }
        [HttpPost]
        public ActionResult SetBodyCopyContent(int galleryID, string imageID, BodyCopyTextContentModel model)
        {
            if(!this.ModelState.IsValid)
                this.PartialView("BodyCopyTextContentPartial", model);

            Action<GalleryImageBase> handler = delegate(GalleryImageBase item) 
            { 
                (item as GalleryContentImage).TextContent = this.ObjectMapper.DoMapping<BodyCopyTextContent>(model);
                (item as GalleryContentImage).TextContent.Size = new Size(model.Width.Value, model.Height.Value);
            };
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

            content.Images.Sort(delegate(GalleryImageBase x, GalleryImageBase y) { return x.Order - y.Order; });

            GalleryRuntime.SaveGalleryContent(galleryID, content);

            return this.Json(new { success = true });
        }

        [HttpPost]
        public ActionResult SetCoverContent(int id, GalleryCoverImageModel model)
        {
            if(!this.ModelState.IsValid)
                return this.Json(new { success = false });

            var content = GalleryRuntime.LoadGalleryContent(id);
            content.CoverImage.Biography = model.Biography;
            content.CoverImage.Headline.Text = model.HeadlineCopyText;
            content.CoverImage.Headline.FontSize = model.HeadlineCopyFontSize;
            content.CoverImage.Standfirst.Text = model.StandfirstText;
            content.CoverImage.Standfirst.FontSize = model.StandfirstFontSize;
            content.CoverImage.TextPosition = model.TextPosition;
            GalleryRuntime.SaveGalleryContent(id, content);

            return this.Json(new { success = true });
        }

        [HttpPost]
        public ActionResult SetEmptyCoverContent(int id)
        {
            if (!this.ModelState.IsValid)
                return this.Json(new { success = false });

            var content = GalleryRuntime.LoadGalleryContent(id);
            content.CoverImage.Biography = string.Empty;
            content.CoverImage.Headline.Text = string.Empty;
            content.CoverImage.Headline.FontSize = 0;
            content.CoverImage.Standfirst.Text = string.Empty;
            content.CoverImage.Standfirst.FontSize = 0;
            content.CoverImage.TextPosition = CoverTextContentPositions.TopLeft;
            GalleryRuntime.SaveGalleryContent(id, content);

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

            GalleryRuntime.BuildGalleryOutput(id);

            var content = GalleryRuntime.LoadGalleryContent(id);

            using (var package = new ZipFile())
            {
                var root = GalleryRuntime.GetGalleryPath(id);

                package.AddFiles(Directory.GetFiles(root));

                foreach (var item in Directory.GetDirectories(root))
                    package.AddDirectory(item, item.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar));

                foreach (var item in content.SystemFilePathes)
                {
                    try
                    {
                        package.RemoveEntry(item);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.WriteError(ex);
                    }
                }

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

            return this.PartialView("ContentImagePartial", this.Convert(image as GalleryContentImage, galleryID.Value));
        }
        [HttpPost]
        public ActionResult GetCoverImage([Required]Nullable<int> galleryID, [Required]string id)
        {
            var content = GalleryRuntime.LoadGalleryContent(galleryID.Value);

            var image = content.CoverImage;

            if (image == null)
                return this.Json(new { success = false, error = string.Format("Image with id='{0}' was not found. Gallery id='{1}'", id, galleryID.Value) });

            return this.PartialView("CoverImagePartial", this.Convert(image, galleryID.Value));
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
            content.DeleteImages(new string[] { id }, this.HttpContext);
            GalleryRuntime.SaveGalleryContent(galleryID.Value, content);
            return this.Json(new { success = true });
        }


        [HttpGet]
        public ActionResult EditImagePopup_GET(EditImagePopupModel model)
        {
            return this.PartialView("EditImagePopupPartial", model);
        }

        [HttpGet]
        public ActionResult UploadMultipleImages(int galleryID, string swfElementID)
        {
            this.ViewBag.GalleryID = galleryID;
            this.ViewBag.SwfElementID = swfElementID;
            return this.PartialView("UploadMultipleImagesPopup");
        }
    }
}
