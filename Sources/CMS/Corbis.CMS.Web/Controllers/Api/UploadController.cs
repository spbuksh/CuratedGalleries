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

namespace Corbis.CMS.Web.Controllers.Api
{
    public class UploadController : ApiControllerBase
    {
        /// <summary>
        /// Method to Upload File to the system.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GalleryImageUpload(HttpRequestMessage message, [System.Web.Http.FromUri, System.ComponentModel.DataAnnotations.Required]Nullable<int> id)
        {
            if (this.HttpContext.Request.Files.Count != 1)
                return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, "There are no files or more then one for uploading in the request");

            var parameters = System.Web.HttpUtility.ParseQueryString(message.RequestUri.Query);

            if (!id.HasValue)
                throw new Exception("Target gallery is not pointed. Query strin parameter 'id' is required");

            string contentpath = GalleryRuntime.GetGalleryContentPath(id.Value);

            if (!Directory.Exists(contentpath))
                Directory.CreateDirectory(contentpath);

            var file = HttpContext.Request.Files[0];

            if (GalleryRuntime.MaxImageSize.HasValue && GalleryRuntime.MaxImageSize.Value < file.ContentLength)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Max file size {1}bytes is exceeded", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            if (GalleryRuntime.MinImageSize.HasValue && file.ContentLength < GalleryRuntime.MinImageSize.Value)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Min file size is {1}bytes", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            string filename = file.FileName;
            string filepath = Path.Combine(contentpath, filename);

            if (File.Exists(filepath))
            {
                filepath = Corbis.Common.Utils.GenerateFilePathForDuplicate(filepath);
                filename = Path.GetFileName(filepath);
            }

            try
            {
                file.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex, string.Format("Uploaded file cannot be saved to '{0}'", filepath));
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            try
            {
                var content = GalleryRuntime.LoadGalleryContent(id.Value);

                var img = new GalleryContentImage()
                {
                    ID = string.Format("gallery-image{0}", content.Images.Count + 1),
                    ImageID = string.Format("gallery-image_{0}", Guid.NewGuid().ToString("N")),
                    Name = filename,
                    Order = content.Images.Count + 1,
                    ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) },
                    Url = string.Format("{0}/{1}", 
                        Corbis.Common.Utils.GetRelativePath(GalleryRuntime.GetGalleryOutputPath(id.Value), contentpath).TrimEnd(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                        filename)
                };
                content.Images.Add(img);

                //TODO: We must synchronize file updating
                GalleryRuntime.SaveGalleryContent(id.Value, content);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                File.Delete(filepath);
                return message.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return message.CreateResponse(HttpStatusCode.OK, Corbis.Common.Utils.AbsoluteToVirtual(filepath, this.HttpContext));
        }

    }
}