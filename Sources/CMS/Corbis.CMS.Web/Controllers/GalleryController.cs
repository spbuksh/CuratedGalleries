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
            CuratedGalleryFilter filter = null;

            OperationResult<OperationResults, List<CuratedGallery>> rslt = null;

            try
            {
                rslt = this.GalleryRepository.GetGalleries(filter);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                throw;
            }

            switch(rslt.Result)
            {
                case OperationResults.Success:
                    break;
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
                galleries.Add(this.ObjectMapper.DoMapping<GalleryItemModel>(item));
            }

            return View("Index", galleries);
        }

        #region Create/Edit Gallery

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Gallery(Nullable<int> id)
        {
            CuratedGallery gallery = null;

            if (id.HasValue)
            {
                var rslt = this.GalleryRepository.GetGallery(id.Value);

                gallery = rslt.Output;
            }
            else
            {
                gallery = GalleryRuntime.CreateGallery("New gallery");
            }

            //
            var model = this.ObjectMapper.DoMapping<GalleryModel>(gallery);

            return this.View("Gallery", model);
        }

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Gallery")]
        public ActionResult UpdateGallery()
        {
            //save updates and transform xslt to html

            throw new NotImplementedException();
        }

        #endregion Create/Edit Gallery

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
            return this.Redirect(string.Format("~/{0}", url.TrimStart('~', '/')));
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

            var image = content.Images.Where(x => string.Equals(x.ImageID, id, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (image == null)
                return this.Json(new { success = false, error = string.Format("Image with id='{0}' was not found. Gallery id='{1}'", id, galleryID.Value) });

            //TODO: create model 
            var model = new GalleryContentImageModel() { GalleryID = galleryID.Value, ID = image.ImageID, Url = image.ImageUrl, Text = image.Name };

            return this.PartialView("ContentImagePartial", model);
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
            var image = content.Images.Where(x => string.Equals(x.ImageID, id, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (image != null)
            {
                content.Images.Remove(image);
                content.Images.Sort(delegate(GalleryContentImage x, GalleryContentImage y) { return x.Order - y.Order; });

                for (int i = 0; i < content.Images.Count; i++)
                    content.Images[i].Order = i + 1;

                GalleryRuntime.SaveGalleryContent(galleryID.Value, content);
            }

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
