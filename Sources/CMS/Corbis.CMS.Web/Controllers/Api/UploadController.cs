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
        public HttpResponseMessage GalleryImageUpload(HttpRequestMessage message)
        {
            if (this.HttpContext.Request.Files.Count != 1)
                return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, "There are no files or more then one for uploading in the request");

            var parameters = System.Web.HttpUtility.ParseQueryString(message.RequestUri.Query);

            //get gallery identifier
            var pmtr = parameters.Get("id");

            if (string.IsNullOrEmpty(pmtr))
                throw new Exception("Target gallery is not pointed. Query strin parameter 'id' is required");

            int id = int.Parse(pmtr);

            string uploadRoot = Path.Combine(GalleryRuntime.GetGalleryPath(id), "Images");

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            var file = HttpContext.Request.Files[0];

            if (GalleryRuntime.MaxImageSize.HasValue && GalleryRuntime.MaxImageSize.Value < file.ContentLength)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Max file size {1}bytes is exceeded", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            if (GalleryRuntime.MinImageSize.HasValue && file.ContentLength < GalleryRuntime.MinImageSize.Value)
                return this.Request.CreateResponse<string>(HttpStatusCode.BadRequest, string.Format("Uploading file size is {0}byte. Min file size is {1}bytes", file.ContentLength, GalleryRuntime.MaxImageSize.Value));

            string filepath = Path.Combine(uploadRoot, file.FileName);

            try
            {
                file.SaveAs(filepath);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex, string.Format("Uploaded file cannot be saved to '{0}'", filepath));
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return message.CreateResponse(HttpStatusCode.OK, new Uri(filepath).AbsoluteUri);
        }

    }
}