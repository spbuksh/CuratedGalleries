using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    [Serializable, DataContract]
    public class GalleryTemplate
    {
        /// <summary>
        /// Gallery template identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery template image url. It is "face" of the gallery
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gallery template description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Is gallery active or not
        /// </summary>
        public bool IsActive { get; set; }
    }
}
