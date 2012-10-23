using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.CMS.Web.Models
{
    public class GalleryContentImageModel
    {
        public int GalleryID { get; set; }

        /// <summary>
        /// Image identifier
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Image url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Image descriptor
        /// </summary>
        public string Text { get; set; }
    }
}