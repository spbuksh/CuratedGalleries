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
    public class GalleryContent
    {
        #region Gallery Content Data

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery font
        /// </summary>
        public GalleryFont Font { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TransitionsIncluded { get; set; }


        /// <summary>
        /// Contains gallery relevant file pathes which were created by the system and must be ignored during gallery output creation
        /// </summary>
        [XmlArray("SystemFiles")]
        [XmlArrayItem("File", typeof(string))]
        public List<string> SystemFilePathes 
        {
            get { return this.m_IgnoredFilePathes; } 
        }
        private readonly List<string> m_IgnoredFilePathes = new List<string>();

        /// <summary>
        /// Gallery images
        /// </summary>
        [XmlArray("Images")]
        [XmlArrayItem("Image", typeof(GalleryImageBase))]
        public List<GalleryImageBase> Images
        {
            get { return this.m_Images; }
        }
        private readonly List<GalleryImageBase> m_Images = new List<GalleryImageBase>();


        /// <summary>
        /// Gallery predefined image sizes. This data is gotten from the gallery descriptor and this data is used in css and so on
        /// </summary>
        [XmlArray()]
        [XmlArrayItem("ImageSize")]
        public List<GalleryImageSize> GalleryImageSizes
        {
            get { return this.m_ImageSizes; }
        }
        private readonly List<GalleryImageSize> m_ImageSizes = new List<GalleryImageSize>();


        #endregion Gallery Content Data

        #region Methods to work with content data

        [XmlIgnore]
        public GalleryCoverImage CoverImage
        {
            get { return this.Images == null || this.Images.Count == 0 ? null : this.Images[0] as GalleryCoverImage; }
        }

        public void Clear(HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            this.DeleteImages((IEnumerable<string>)null, context);
        }

        /// <summary>
        /// Use this method to delete images.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="context"></param>
        public void DeleteImages(IEnumerable<string> ids, HttpContextBase context = null)
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
                this.Images.Sort(delegate(GalleryImageBase x, GalleryImageBase y) { return x.Order - y.Order; });

                for (int i = 0; i < this.Images.Count; i++)
                    this.Images[i].Order = i + 1;
            }
        }

        public void DeleteImageContent(string url, HttpContextBase context)
        {
            var file = new FileInfo(Corbis.Common.Utils.VirtualToAbsolute(url, context));

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
        }

        public void DeleteImages(ImageUrlSet urls, HttpContextBase context)
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

            foreach (var vpath in new string[] { urls.Original, urls.Large, urls.Middle, urls.Small })
            {
                if (string.IsNullOrEmpty(vpath))
                    continue;

                string path = Corbis.Common.Utils.VirtualToAbsolute(vpath, context);
                delHandler(path);
                this.SystemFilePathes.Remove(this.SystemFilePathes.FirstOrDefault(path.EndsWith));
            }
        }

        #endregion Methods to work with content data
    }
}
