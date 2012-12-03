using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Entity;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Base class for any gallery image 
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(GalleryContentImage))]
    [XmlInclude(typeof(GalleryCoverImage))]
    public class GalleryImageBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public GalleryImageBase()
        {
            //TODO: It is temporary due to bad gallery template
            //this.ImageContentName = string.Empty;
            //this.ImageContentUrl = string.Empty;
        }

        /// <summary>
        /// Unique gallery image identifier for usage in gallery output. 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Image order number in the gallery
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Image urls for gallery based on gallery file system
        /// </summary>
        public ImageUrlSet GalleryUrls { get; set; }

        /// <summary>
        /// Image urls for site
        /// </summary>
        public ImageUrlSet SiteUrls { get; set; }

        /// <summary>
        /// Image urls for edit views inside gallery creation logic
        /// </summary>
        public ImageUrlSet EditUrls { get; set; }

        /// <summary>
        /// Image fime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image source
        /// </summary>
        public GalleryImageSource ImageSource { get; set; }

        public GalleryImageContent ContentImage { get; set; }

        ///// <summary>
        ///// Png image urls for gallery based on gallery file system.
        ///// </summary>
        //public ImageUrlSet ImageContentUrls { get; set; }

        ///// <summary>
        ///// png image urls for gallery based on gallery file system (likewise 'GalleryUrls' property)
        ///// </summary>
        //public string ImageContentUrl { get; set; }

        ///// <summary>
        ///// png file name
        ///// </summary>
        //public string ImageContentName { get; set; }
    }
}
