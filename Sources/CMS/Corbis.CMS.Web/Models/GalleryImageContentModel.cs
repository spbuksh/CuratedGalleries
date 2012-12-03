using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Models
{
    public class GalleryImageContentModel
    {
        /// <summary>
        /// Unique gallery image identifier for usage in gallery output. 
        /// </summary>

        public string ID { get; set; }

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
        /// Image text content attached to the image
        /// </summary>
        public ImageTextContentModelBase TextContent { get; set; }
    }
}