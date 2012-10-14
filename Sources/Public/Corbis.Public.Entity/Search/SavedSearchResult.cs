using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Corbis.Public.Entity.Search
{
    [Serializable, XmlRoot, DataContract]
    public class SavedSearchResult
    {
        [XmlElement]
        [DataMember]
        public string CorbisUserId { get; set; }
        [XmlElement]
        [DataMember]
        public string SearchValue { get; set; }
        [XmlElement]
        [DataMember]
        public string Filter { get; set; }
    }
}
