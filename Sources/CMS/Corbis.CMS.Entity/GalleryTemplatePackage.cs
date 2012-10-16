using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Gallery template package
    /// </summary>
    [Serializable, DataContract]
    public class GalleryTemplatePackage
    {
        /// <summary>
        /// ZIP File name
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// ZIP file content
        /// </summary>
        public byte[] Content { get; set; }
    }
}
