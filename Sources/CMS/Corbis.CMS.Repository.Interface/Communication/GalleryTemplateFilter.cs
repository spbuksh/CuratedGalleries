using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Repository.Interface.Communication
{
    [Serializable, DataContract]
    public class GalleryTemplateFilter
    {
        //TODO: Sorting filter (flagged enum)

        /// <summary>
        /// Name pattern. It is substring of the name
        /// </summary>
        public string NamePattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> Enabled { get; set; }

        /// <summary>
        /// Index of the first
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gallery count to get
        /// </summary>
        public Nullable<int> Count { get; set; }

        /// <summary>
        /// Indicates include zip packages or not
        /// </summary>
        public bool IncludePackage { get; set; }
    }
}
