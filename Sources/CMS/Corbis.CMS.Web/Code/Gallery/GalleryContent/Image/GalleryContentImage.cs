using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery content image
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(QnATextContent))]
    [XmlInclude(typeof(PullQuotedTextContent))]
    public class GalleryContentImage : GalleryImageBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public GalleryContentImage()
        {
            //TODO: It is temporary due to bad gallery template
            this.ImageContentName = string.Empty;
            this.ImageContentUrl = string.Empty;
        }

        /// <summary>
        /// Image text content
        /// </summary>
        public ImageTextContentBase TextContent { get; set; }

        /// <summary>
        /// png image urls for gallery based on gallery file system (likewise 'GalleryUrls' property)
        /// </summary>
        public string ImageContentUrl { get; set; }

        /// <summary>
        /// png file name
        /// </summary>
        public string ImageContentName { get; set; }
    }
}
