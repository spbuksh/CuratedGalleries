using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

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
        public HttpResponseMessage FileUpload(HttpRequestMessage message)
        {
            try
            {
                HttpResponseMessage responseMessage = null;
                var uploadPath = ConfigurationManager.AppSettings["UploadPath"];
                var path = HttpContext.Server.MapPath(uploadPath);
                if (HttpContext.Request.Files.Count <= 0)
                {
                    responseMessage = message.CreateResponse(HttpStatusCode.NotAcceptable);
                }
                else
                {
                    for (var i = 0; i < HttpContext.Request.Files.Count; ++i)
                    {
                        var file = HttpContext.Request.Files[i];
                        file.SaveAs(Path.Combine(path, file.FileName));
                        responseMessage = message.CreateResponse(HttpStatusCode.OK, new Uri(Path.Combine(path, file.FileName)).AbsoluteUri);
                    }
                }

                return responseMessage;

            }
            catch
            {
                throw;
            }
        }
    }
}