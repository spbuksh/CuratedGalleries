using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using System.IO;
using System.Web;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery content. It defines gallery view state.
    /// </summary>
    [Serializable]
    [KnownType(typeof(QnATextContent))]
    [KnownType(typeof(PullQuotedTextContent))]
    public class GalleryContent
    {
        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery font
        /// </summary>
        public GalleryFont Font { get; set; }

        /// <summary>
        /// Gallery cover image
        /// </summary>
        public GalleryCoverImage CoverImage { get; set; }

        /// <summary>
        /// Gallery images
        /// </summary>
        [XmlArray("Images")]
        [XmlArrayItem("Image", typeof(GalleryContentImage))]
        public List<GalleryContentImage> Images
        {
            get { return this.m_Images; }
        }
        private readonly List<GalleryContentImage> m_Images = new List<GalleryContentImage>();

        public void Clear(HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            this.DeleteCoverImage(context);
            this.DeleteContentImages(null, context);
        }

        /// <summary>
        /// Use this method to delete images.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="context"></param>
        public void DeleteCoverImage(HttpContextBase context = null)
        {
            if (this.CoverImage == null)
                return;

            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            if (this.CoverImage.SiteUrls != null)
                this.DeleteImages(this.CoverImage.SiteUrls, context);

            if (this.CoverImage.EditUrls != null)
                this.DeleteImages(this.CoverImage.EditUrls, context);

            this.CoverImage = null;
        }
        /// <summary>
        /// Use this method to delete images.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="context"></param>
        public void DeleteContentImages(IEnumerable<string> ids, HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            var items = ((ids != null) ? (ids.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.ToLower())) : (this.Images.Select(x => x.ID.ToLower()))).ToList();

            foreach (var image in this.Images.Where(x => items.Contains(x.ID.ToLower())).ToArray())
            {
                if (image.SiteUrls != null)
                    DeleteImages(image.SiteUrls, context);

                if (image.EditUrls != null)
                    DeleteImages(image.EditUrls, context);

                this.Images.Remove(image);
            }

            if (this.Images.Count != 0)
            {
                this.Images.Sort(delegate(GalleryContentImage x, GalleryContentImage y) { return x.Order - y.Order; });

                for (int i = 0; i < this.Images.Count; i++)
                    this.Images[i].Order = i + 1;
            }
        }

        protected void DeleteImages(ImageUrlSet urls, HttpContextBase context)
        {
            Action<string> delHandler = delegate(string filepath)
            {
                if (string.IsNullOrEmpty(filepath))
                    return;

                var file = new FileInfo(filepath);

                if (file.Exists)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
#if DEBUG
                        //generate exception not to hide problems (for example file locks)
                        throw;
#endif
                    }
                }
            };

            if (!string.IsNullOrEmpty(urls.Original))
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(urls.Original, context));

            if (!string.IsNullOrEmpty(urls.Large))
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(urls.Large, context));

            if (!string.IsNullOrEmpty(urls.Middle))
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(urls.Middle, context));

            if (!string.IsNullOrEmpty(urls.Small))
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(urls.Small, context));
        }
    }
}
