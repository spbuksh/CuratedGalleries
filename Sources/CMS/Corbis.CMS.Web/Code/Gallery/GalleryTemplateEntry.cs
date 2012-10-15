using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using System.Runtime.Serialization;

namespace Corbis.CMS.Web.Code
{
    [Serializable, DataContract]
    public class GalleryTemplateEntry : GalleryTemplate
    {
        /// <summary>
        /// Absolute path to the directory with unziped template package
        /// </summary>
        public string Directory { get; set; }

    }
}