﻿using System;
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

namespace Corbis.CMS.Web.Controllers
{
    public class GalleryController : CMSControllerBase
    {
        /// <summary>
        /// Gallety index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
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
                //TODO: get existing gallery for editing
                throw new NotImplementedException();
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
