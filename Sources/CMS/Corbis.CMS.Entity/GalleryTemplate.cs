using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Gallery template object
    /// </summary>
    [Serializable, DataContract]
    public class GalleryTemplate : GalleryTemplateInfo
    {
        /// <summary>
        /// Gallery template identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long PackageID { get; set; }
        
        /// <summary>
        /// Creation date. Has UTC format
        /// </summary>
        public Nullable<DateTime> DateCreated { get; set; }
    }
}
