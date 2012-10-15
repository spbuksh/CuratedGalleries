using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Repository.Interface.Communication
{
    [Serializable, DataContract]
    public class CuratedGalleryFilter
    {
        //TODO: Sort filter(flagged enum)

        /// <summary>
        /// Gallery name pattern
        /// </summary>
        public string NamePattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> Enabled { get; set; }

        /// <summary>
        /// Start gallery index
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Defines portion of retrived data. If it if null then rest data (from startIndex) will be returned or all entities
        /// </summary>
        public Nullable<int> Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<CuratedGalleryContent> GalleryContent { get; set; }
    }
}
