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
using System.Text.RegularExpressions;

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

            string contentpath = GalleryRuntime.GetGalleryContentPath(id);

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
                int index = filename.LastIndexOf('.');

                string nameonly = index < 0 ? filename : filename.Substring(0, index);
                string ext = filename.Substring(nameonly.Length);

                List<int> postfixes = new List<int>();

                int findx = 0;

                foreach (var item in Directory.GetFiles(contentpath, nameonly + "*"))
                {
                    if(item == filepath)
                        continue;

                    string fname = Path.GetFileName(item);
                    string pfx = fname.Substring(0, fname.Length - ext.Length).Substring(nameonly.Length);
                    pfx = pfx.Trim().TrimStart('(').TrimEnd(')').Trim();

                    if (int.TryParse(pfx, out findx))
                        postfixes.Add(findx);
                }

                filename = string.Format("{0}({1}){2}", nameonly, postfixes.Count == 0 ? 1 : postfixes.Max() + 1, ext);
                filepath = Path.Combine(contentpath, filename);
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
                var content = GalleryRuntime.LoadGalleryContent(id);

                var img = new GalleryContentImage()
                {
                    ID = string.Format("gallery-image{0}", content.Images.Count + 1),
                    ImageID = string.Format("gallery-image_{0}", Guid.NewGuid().ToString("N")),
                    Order = content.Images.Count + 1,
                    ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id).Length) },
                    Url = string.Format("{0}/{1}", 
                        this.RelativePath(GalleryRuntime.GetGalleryOutputPath(id), contentpath).TrimEnd(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), 
                        filename)
                };
                content.Images.Add(img);

                //TODO: We must synchronize file updating
                GalleryRuntime.SaveGalleryContent(id, content);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                File.Delete(filepath);
                return message.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return message.CreateResponse(HttpStatusCode.OK, Corbis.Common.Utils.AbsoluteToVirtual(filepath, this.HttpContext));
        }

        public string RelativePath(string absPath, string relTo)
        {
            string[] absDirs = absPath.Split(Path.DirectorySeparatorChar);
            string[] relDirs = relTo.Split(Path.DirectorySeparatorChar);

            // Get the shortest of the two paths
            int len = absDirs.Length < relDirs.Length ? absDirs.Length :
            relDirs.Length;

            // Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            // Find common root
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
                else break;
            }

            // If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                throw new ArgumentException("Paths do not have a common base");

            // Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // Add on the ..
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0) relativePath.Append(@"..\");
            }

            // Add on the folders
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + @"\");
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);

            return relativePath.ToString();
        }


    }
}