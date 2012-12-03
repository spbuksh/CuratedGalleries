using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    [Serializable]
    public class GalleryImageContent : GalleryImageContentBase
    {
        /// <summary>
        /// Image text content
        /// </summary>
        public ImageTextContentBase TextContent { get; set; }
    }
}