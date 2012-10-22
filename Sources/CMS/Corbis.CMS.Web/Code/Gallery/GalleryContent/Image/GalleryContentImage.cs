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
        /// Image text content
        /// </summary>
        public ImageTextContentBase TextContent { get; set; }

        /// <summary>
        /// Image order number in the gallery
        /// </summary>
        public int Order { get; set; }

    }
}
