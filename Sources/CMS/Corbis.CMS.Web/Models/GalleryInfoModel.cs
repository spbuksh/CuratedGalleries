using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.CMS.Web.Models
{
    public class GalleryInfoModel
    {
        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        public string Template { get; set; }

        public string Status { get; set; }

        public string CreationTime { get; set; }

        public string PublicationPeriod { get; set; }

        public string LiveURL { get; set; }

        public string LockedBy { get; set; }

        public int ImageCount { get; set; }
    }
}