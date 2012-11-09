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
using System.Runtime.Serialization;

namespace Corbis.CMS.Web.Controllers.Api
{
    [Serializable, DataContract]
    internal class UploadedImageResponse
    {
        [DataMember]
        public int GalleryID { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public ImageUrlSet Urls { get; set; }
        [DataMember]
        public ImageUrlSet EditUrls { get; set; }
        [DataMember]
        public bool IsCover { get; set; }
        [DataMember]
        public string FileName { get; set; }
    }

    public class UploadController : ApiControllerBase
    {
        /// <summary>
        /// Method to Upload File to the system.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadGalleryImage(HttpRequestMessage message,
            [System.Web.Http.FromUri, System.ComponentModel.DataAnnotations.Required]Nullable<int> id)
        {
            return this.UploadGalleryImage(message, id, null);
        }

        /// <summary>
        /// Method to Upload File to the system.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadGalleryImage(HttpRequestMessage message,
            [System.Web.Http.FromUri, System.ComponentModel.DataAnnotations.Required]Nullable<int> id, [System.Web.Http.FromUri]string imageID)
        {
            if (this.HttpContext.Request.Files.Count != 1)
                return this.Request.CreateResponse(HttpStatusCode.NotAcceptable, "There are no files or more then one for uploading in the request");

            if (!id.HasValue)
                throw new Exception("Target gallery is not pointed. Query strin parameter 'id' is required");

            var gallery = GalleryRuntime.GetGallery(id.Value);
            var template = GalleryRuntime.GetTemplate(gallery.TemplateID);

            string contentpath = gallery.GetContentPath();
            string rootpath = gallery.GetRootPath();

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

            var content = gallery.LoadContent();

            var siteUrls = new ImageUrlSet() { Original = Common.Utils.AbsoluteToVirtual(filepath, this.HttpContext) };
            var editUrls = new ImageUrlSet() { Original = Common.Utils.AbsoluteToVirtual(filepath, this.HttpContext) };
            var gllrUrls = new ImageUrlSet() { Original = gllrUrlHandler(filename) };

            string extension = Path.GetExtension(filename);
            string filenameonly = extension.Length > 0 ? filename.Remove(filename.Length - extension.Length) : filename;

            System.Drawing.Image originalImage = null;

            //TODO: Implement async image resizing for performance
            try
            {
                originalImage = System.Drawing.Image.FromFile(filepath);

                //resize images for development
                {
                    string editpath = Path.Combine(contentpath, string.Format("{0}.lrgedit{1}", filenameonly, extension));
                    ResizeImage(originalImage, editpath, GalleryRuntime.EditedLargeImageSize);

                    editUrls.Large = Common.Utils.AbsoluteToVirtual(editpath, this.HttpContext);

                    content.SystemFilePathes.Add(editpath.Substring(rootpath.Length));
                }
                {
                    string editpath = Path.Combine(contentpath, string.Format("{0}.smledit{1}", filenameonly, extension));
                    ResizeImage(originalImage, editpath, GalleryRuntime.EditedSmallImageSize);

                    editUrls.Small = Common.Utils.AbsoluteToVirtual(editpath, this.HttpContext);

                    content.SystemFilePathes.Add(editpath.Substring(rootpath.Length));
                }

                //images for galleries
                foreach (var isize in template.GallerySettings.ImageSizes)
                { 
                    switch(isize.Type)
                    {
                        case GalleryImageSizes.Small:
                            {
                                string smallpath = Path.Combine(contentpath, string.Format("{0}.sml{1}", filenameonly, extension));
                                ResizeImage(originalImage, smallpath, new System.Drawing.Size(isize.Width, isize.Height));

                                siteUrls.Small = Common.Utils.AbsoluteToVirtual(smallpath, this.HttpContext);
                                gllrUrls.Small = gllrUrlHandler(Path.GetFileName(smallpath)); 
                            }
                            break;
                        case GalleryImageSizes.Middle:
                            { 
                                string middlepath = Path.Combine(contentpath, string.Format("{0}.mdl{1}", filenameonly, extension));
                                ResizeImage(originalImage, middlepath, new System.Drawing.Size(isize.Width, isize.Height));

                                siteUrls.Middle = Common.Utils.AbsoluteToVirtual(middlepath, this.HttpContext);
                                gllrUrls.Middle = gllrUrlHandler(Path.GetFileName(middlepath));
                            }
                            break;
                        case GalleryImageSizes.Large:
                            { 
                                string largepath = Path.Combine(contentpath, string.Format("{0}.lrg{1}", filenameonly, extension));
                                ResizeImage(originalImage, largepath, new System.Drawing.Size(isize.Width, isize.Height));

                                siteUrls.Large = Common.Utils.AbsoluteToVirtual(largepath, this.HttpContext);
                                gllrUrls.Large = gllrUrlHandler(Path.GetFileName(largepath));
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            finally
            {
                if (originalImage != null)
                    originalImage.Dispose();
            }

            GalleryImageBase img = null;

            try
            {
                if (!string.IsNullOrEmpty(imageID))
                {
                    if (content.CoverImage.ID == imageID)
                    {
                        content.CoverImage.Name = filename;
                        content.CoverImage.ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) };
                        content.DeleteImages(content.CoverImage.GalleryUrls, this.HttpContext);
                        content.CoverImage.GalleryUrls = gllrUrls;
                        content.DeleteImages(content.CoverImage.SiteUrls, this.HttpContext);
                        content.CoverImage.SiteUrls = siteUrls;
                        content.DeleteImages(content.CoverImage.EditUrls, this.HttpContext);
                        content.CoverImage.EditUrls = editUrls;

                        img = content.CoverImage;
                    }
                    else
                    {
                        var item = content.Images.Where(x => x.ID == imageID).SingleOrDefault();

                        item.Name = filename;
                        item.ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) };
                        content.DeleteImages(item.GalleryUrls, this.HttpContext);
                        item.GalleryUrls = gllrUrls;
                        content.DeleteImages(item.SiteUrls, this.HttpContext);
                        item.SiteUrls = siteUrls;
                        content.DeleteImages(item.EditUrls, this.HttpContext);
                        item.EditUrls = editUrls;

                        img = item;
                    }
                }
                else
                {
                    if ((content.Images != null && content.Images.Count != 0) || content.CoverImage != null)
                    {
                        var cimg = new GalleryContentImage()
                        {
                            ID = string.Format("gallery-image_{0}", Guid.NewGuid().ToString("N")),
                            Name = filename,
                            Order = content.Images.Count + 1,
                            ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) },
                            GalleryUrls = gllrUrls,
                            SiteUrls = siteUrls,
                            EditUrls = editUrls
                        };
                        cimg.TextContent = new EmptyTextContent();

                        content.Images.Add(cimg);

                        img = cimg;
                    }
                    else
                    {
                        var cimg = new GalleryCoverImage()
                        {
                            ID = string.Format("gallery-image_{0}", Guid.NewGuid().ToString("N")),
                            Name = filename,
                            Order = 0,
                            ImageSource = new GalleryImageSource() { Type = ImageSourceTypes.LocalFile, Source = filepath.Substring(GalleryRuntime.GetGalleryPath(id.Value).Length) },
                            GalleryUrls = gllrUrls,
                            SiteUrls = siteUrls,
                            EditUrls = editUrls
                        };

                        //TODO: we can get default values from gallery template
                        cimg.Headline = new CoverTextItem() { FontSize = 40 };
                        cimg.Standfirst = new CoverTextItem() { FontSize = 12 };

                        content.CoverImage = cimg;

                        img = cimg;
                    }
                }

                //TODO: We must synchronize file updating
                gallery.SaveContent(content);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
                File.Delete(filepath);
                return message.CreateResponse(HttpStatusCode.InternalServerError);
            }

            var output = new UploadedImageResponse() { GalleryID = id.Value, ID = img.ID, Urls = img.SiteUrls, EditUrls = img.EditUrls, IsCover = img is GalleryCoverImage, FileName = img.Name };
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