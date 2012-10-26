using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Corbis.CMS.Web.Code;
using System.Text;
using Corbis;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Controllers.Api
{
    internal class UploadedImageResponse
    {
        public int GalleryID { get; set; }

        public string ID { get; set; }

        public ImageUrlSet Urls { get; set; }
    }

    public class UploadController : ApiControllerBase
    {
        /// <summary>
        /// Method to Upload File to the system.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadGalleryImage(HttpRequestMessage message, [System.Web.Http.FromUri, System.ComponentModel.DataAnnotations.Required]Nullable<int> id)
        {
            if (this.HttpContext.Request.Files.Count != 1)
                return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, "There are no files or more then one for uploading in the request");

            if (!id.HasValue)
                throw new Exception("Target gallery is not pointed. Query strin parameter 'id' is required");

            var gallery = GalleryRuntime.GetGallery(id.Value);
            var template = GalleryRuntime.GetTemplate(gallery.TemplateID);

            string contentpath = gallery.GetContentPath();

            if (!Directory.Exists(contentpath))
                Directory.CreateDirectory(contentpath);

            var file = this.HttpContext.Request.Files[0];

            //TODO: CHANGE VALIDATE LOGIC
            if (GalleryRuntime.MaxImageSize.HasValue && GalleryRuntime.MaxImageSize.Value < file.ContentLength)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Max file size {1}bytes is exceeded", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            if (GalleryRuntime.MinImageSize.HasValue && file.ContentLength < GalleryRuntime.MinImageSize.Value)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Min file size is {1}bytes", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            string filename = file.FileName;
            string filepath = Path.Combine(contentpath, filename);

            if (File.Exists(filepath))
            {
                filepath = Common.Utils.GenerateFilePathForDuplicate(filepath);
                filename = Path.GetFileName(filepath);
            }

            //try to save original image
            try
            {
                file.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex, string.Format("Uploaded file cannot be saved to '{0}'", filepath));
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            finally
            {
                file.InputStream.Close();
            }

            //this handler return gallery relative image url
            Common.ActionHandler<string, string> gllrUrlHandler =
                delegate(string fname)
                {
                    return string.Format("{0}/{1}", Common.Utils.GetRelativePath(GalleryRuntime.GetGalleryOutputPath(id.Value), contentpath).TrimEnd('\\').Replace('\\', '/'), fname);
                };

            var siteUrls = new ImageUrlSet() { Original = Common.Utils.AbsoluteToVirtual(filepath, this.HttpContext) };
            var gllrUrls = new ImageUrlSet() { Original = gllrUrlHandler(filename) };

            string extension = Path.GetExtension(filename);
            string filenameonly = extension.Length > 0 ? filename.Remove(filename.Length - extension.Length) : filename;

            System.Drawing.Image originalImage = null;

            try
            {

                if (template.GallerySettings.RequiredImageSizes.HasFlag(GalleryImageSizes.Large))
                {
                    string fname = string.Format("{0}.lrg", filenameonly);
                    string largepath = Path.Combine(contentpath, string.Format("{0}{1}", fname, extension));

                    if (originalImage == null)
                        originalImage = System.Drawing.Image.FromFile(filepath);

                    ResizeImage(originalImage, largepath, template.GallerySettings.GetImageSize(GalleryImageSizes.Large).Value);

                    siteUrls.Large = Common.Utils.AbsoluteToVirtual(largepath, this.HttpContext);
                    gllrUrls.Large = gllrUrlHandler(Path.GetFileName(largepath));
                }
                if (template.GallerySettings.RequiredImageSizes.HasFlag(GalleryImageSizes.Middle))
                {
                    string fname = string.Format("{0}.mdl", filenameonly);
                    string middlepath = Path.Combine(contentpath, string.Format("{0}{1}", fname, extension));

                    if (originalImage == null)
                        originalImage = System.Drawing.Image.FromFile(filepath);

                    ResizeImage(originalImage, middlepath, template.GallerySettings.GetImageSize(GalleryImageSizes.Middle).Value);

                    siteUrls.Middle = Common.Utils.AbsoluteToVirtual(middlepath, this.HttpContext);
                    gllrUrls.Middle = gllrUrlHandler(Path.GetFileName(middlepath));
                }
                if (template.GallerySettings.RequiredImageSizes.HasFlag(GalleryImageSizes.Small))
                {
                    string fname = string.Format("{0}.sml", filenameonly);
                    string smallpath = Path.Combine(contentpath, string.Format("{0}{1}", fname, extension));

                    if (originalImage == null)
                        originalImage = System.Drawing.Image.FromFile(filepath);

                    ResizeImage(originalImage, smallpath, template.GallerySettings.GetImageSize(GalleryImageSizes.Small).Value);

                    siteUrls.Small = Common.Utils.AbsoluteToVirtual(smallpath, this.HttpContext);
                    gllrUrls.Small = gllrUrlHandler(Path.GetFileName(smallpath));
                }
            }
            finally
            {
                if (originalImage != null)
                    originalImage.Dispose();
            }
            
            GalleryContentImage img = null;

            try
            {
                var content = gallery.LoadContent();

                img = new GalleryContentImage()
                {
                    ID = string.Format("gallery-image_{0}", Guid.NewGuid().ToString("N")),
                    Name = filename,
                    Order = content.Images.Count + 1,
                    ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) },
                    GalleryUrls = gllrUrls,
                    SiteUrls = siteUrls
                };
                content.Images.Add(img);

                //TODO: We must synchronize file updating
                gallery.SaveContent(content);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                File.Delete(filepath);
                return message.CreateResponse(HttpStatusCode.InternalServerError);
            }

            var output = new UploadedImageResponse() { GalleryID = id.Value, ID = img.ID, Urls = img.SiteUrls };
            return message.CreateResponse <UploadedImageResponse>(HttpStatusCode.OK, output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="toImage"></param>
        /// <param name="size"></param>
        protected static void ResizeImage(string srcImage, string toImage, System.Drawing.Size size)
        {
            ResizeImage(System.Drawing.Image.FromFile(srcImage), toImage, size);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="toImage"></param>
        /// <param name="size"></param>
        protected static void ResizeImage(System.Drawing.Image imgFrom, string toImage, System.Drawing.Size size)
        {
            if ((imgFrom.Size.Height < size.Height) && (imgFrom.Size.Width < size.Width))
                size = imgFrom.Size;

            using(var imgTo = Common.Utilities.Image.ImageHelper.ResizeImage(imgFrom, size))
            {
                imgTo.Save(toImage);
            }
        }

    }
}