using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Gallery status. !!!DO NOT CHANGE ENUM VALUES DO TO THESE VALUES ARE BINDED TO DB!!!
    /// </summary>
    [Serializable, DataContract]
    public enum CuratedGalleryStatuses
    {
        /// <summary>
        /// Gallery published
        /// </summary>
        [EnumMember]
        Published = 1,
        /// <summary>
        /// Gallery is unpublished
        /// </summary>
        [EnumMember]
        UnPublished = 2
    }
}
