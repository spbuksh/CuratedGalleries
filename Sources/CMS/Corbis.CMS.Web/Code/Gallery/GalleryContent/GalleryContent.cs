using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using System.IO;
using System.Web;

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


        /// <summary>
        /// Use this method to delete images.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="context"></param>
        public void DeleteContentImages(IEnumerable<string> ids, HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            var items = ids.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.ToLower()).ToList();

            foreach (var image in this.Images.Where(x => items.Contains(x.ID.ToLower())).ToArray())
            {
                if (image.SiteUrls == null)
                    continue;

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

                delHandler(Corbis.Common.Utils.VirtualToAbsolute(image.SiteUrls.Original, context));
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(image.SiteUrls.Large, context));
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(image.SiteUrls.Middle, context));
                delHandler(Corbis.Common.Utils.VirtualToAbsolute(image.SiteUrls.Small, context));

                this.Images.Remove(image);
            }

            if (this.Images.Count != 0)
            {
                this.Images.Sort(delegate(GalleryContentImage x, GalleryContentImage y) { return x.Order - y.Order; });

                for (int i = 0; i < this.Images.Count; i++)
                    this.Images[i].Order = i + 1;
            }
        }
    }
}
