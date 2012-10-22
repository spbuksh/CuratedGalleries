using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery content. It defines gallery view state.
    /// </summary>
    [Serializable]
    [KnownType(typeof(QnATextContent))]
    [KnownType(typeof(PullQuotedTextContent))]
    public class GalleryContent
    {
        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery font
        /// </summary>
        public GalleryFont Font { get; set; } 

        /// <summary>
        /// Gallery cover image
        /// </summary>
        public GalleryCoverImage CoverImage { get; set; }

        /// <summary>
        /// Gallery images
        /// </summary>
        [XmlArray("Images")]
        [XmlArrayItem("Image", typeof(GalleryContentImage))]
        public List<GalleryContentImage> Images
        {
            get { return this.m_Images; }
        }
        private readonly List<GalleryContentImage> m_Images = new List<GalleryContentImage>();
    }
}
