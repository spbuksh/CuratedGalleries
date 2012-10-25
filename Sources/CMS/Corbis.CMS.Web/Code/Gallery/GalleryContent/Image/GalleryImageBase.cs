using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Base class for any gallery image 
    /// </summary>
    [Serializable]
    public class GalleryImageBase
    {
        /// <summary>
        /// Unique gallery image identifier for usage in gallery output. 
        /// </summary>
        public string ID { get; set; }

        ///// <summary>
        ///// Relative gallery image url. Gallery will reference to the image using this url
        ///// </summary>
        //public string Url { get; set; }

        /// <summary>
        /// Image urls for gallery based on gallery file system
        /// </summary>
        public ImageUrlSet GalleryUrls { get; set; }

        /// <summary>
        /// Image urls for site
        /// </summary>
        public ImageUrlSet SiteUrls { get; set; }

        ///// <summary>
        ///// Image url 
        ///// </summary>
        //public string ImageUrl { get; set; }

        /// <summary>
        /// Image fime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image source
        /// </summary>
        public GalleryImageSource ImageSource { get; set; }
    }
}
