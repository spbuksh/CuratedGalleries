using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using Corbis.CMS.Web.Code;

namespace Corbis.CMS.Web.Models
{
    public class GalleryImageModelBase
    {
        public int GalleryID { get; set; }

        /// <summary>
        /// Image identifier
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Image order number
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Image url
        /// </summary>
        public ImageUrlSet Urls { get; set; }

        /// <summary>
        /// Image descriptor
        /// </summary>
        public string Text { get; set; }
    }

    public class GalleryContentImageModel : GalleryImageModelBase
    {
        /// <summary>
        /// Image text content attached to the image
        /// </summary>
        public ImageTextContentModelBase TextContent { get; set; }
    }

    public class GalleryCoverImageModel : GalleryImageModelBase
    {
    }

}