using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Corbis.Public.Entity.Search
{
    /// <summary>
    /// TODO: MUST BE DELETED!!!!
    /// </summary>
    [Serializable, XmlRoot, DataContract]
    public class SearchResultValue
    {
        [XmlElement]
        [DataMember]
        public string score { get; set; }
        [XmlElement]
        [DataMember]
        public string AssetId { get; set; }
    }
}
