using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Corbis.Public.Entity.Search
{
    /// <summary>
    /// Image search result object
    /// </summary>
    [Serializable, DataContract]
    public class ImageSearchResult
    {
        /// <summary>
        /// Whole searched result count
        /// </summary>
        [DataMember]
        public long HitCount { get; set; }

        /// <summary>
        /// Requested portion of whole searched list items
        /// </summary>
        [DataMember]
        public List<ImageSearchResultItem> Items 
        {
            get { return this.m_Items; }
        }
        private readonly List<ImageSearchResultItem> m_Items = new List<ImageSearchResultItem>();
    }
}
