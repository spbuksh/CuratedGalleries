using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    [Serializable]
    public enum ImageSourceTypes
    {
        /// <summary>
        /// Local file path. It is absolute or relevant (relativly gallery root path) path
        /// </summary>
        LocalFile,
        /// <summary>
        /// Image 
        /// </summary>
        Url
    }

    public interface IImageSource
    {
        /// <summary>
        /// Image source type
        /// </summary>
        ImageSourceTypes Type { get; }

        /// <summary>
        /// Image source value
        /// </summary>
        string Source { get; }

    }

    [Serializable]
    public class GalleryImageSource : IImageSource
    {
        /// <summary>
        /// Image source type
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public ImageSourceTypes Type { get; set; }

        /// <summary>
        /// Image source value
        /// </summary>
        [XmlAttribute(AttributeName = "src")]
        public string Source { get; set; }
    }
}
