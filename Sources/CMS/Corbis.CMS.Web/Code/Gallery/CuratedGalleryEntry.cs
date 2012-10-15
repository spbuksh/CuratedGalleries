using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    [Serializable, DataContract]
    public class CuratedGalleryEntry : CuratedGallery
    {
        /// <summary>
        /// Gallery root directory path.
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// Url to the gallery for preview
        /// </summary>
        public string Url { get; set; }

    }
}