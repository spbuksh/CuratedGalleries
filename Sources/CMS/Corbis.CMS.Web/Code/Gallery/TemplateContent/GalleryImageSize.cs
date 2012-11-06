using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    public interface IGalleryImageSize
    {
        /// <summary>
        /// Image size type
        /// </summary>
        GalleryImageSizes Type { get; }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        int Height { get; }

    }
    [Serializable]
    public class GalleryImageSize : IGalleryImageSize
    {
        /// <summary>
        /// Image size type
        /// </summary>
        [XmlAttribute("sztype")]
        public virtual GalleryImageSizes Type { get; set; }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        [XmlAttribute("width")]
        public virtual int Width { get; set; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        [XmlAttribute("height")]
        public virtual int Height { get; set; }
    }
}