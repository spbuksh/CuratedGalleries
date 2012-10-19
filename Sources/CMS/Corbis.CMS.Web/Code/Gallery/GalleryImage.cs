using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Corbis.CMS.Web.Code.Gallery
{
    [Serializable, DataContract]
    public class GalleryImage //: INotifyPropertyChanged
    {
        [XmlElement]
        public string ID { get; set; }

        [XmlElement]
        public string FileName { get; set; }

        [XmlElement]
        public int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string Url { get; set; }
    }
}