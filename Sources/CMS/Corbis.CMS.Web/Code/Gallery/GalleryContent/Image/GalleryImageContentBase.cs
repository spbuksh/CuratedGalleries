using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    [Serializable]
    public class GalleryImageContentBase
    {

        /// <summary>
        /// Unique gallery image identifier for usage in gallery output. 
        /// </summary>
        [XmlElement]
        public string ID { get; set; }

        /// <summary>
        /// Image urls for gallery based on gallery file system
        /// </summary>
        [XmlElement]
        public ImageUrlSet GalleryUrls { get; set; }

        /// <summary>
        /// Image urls for site
        /// </summary>
        [XmlElement]
        public ImageUrlSet SiteUrls { get; set; }

        /// <summary>
        /// Image urls for edit views inside gallery creation logic
        /// </summary>
        [XmlElement]
        public ImageUrlSet EditUrls { get; set; }

        /// <summary>
        /// Image fime name
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

    }
}