using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery content image
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(QnATextContent))]
    [XmlInclude(typeof(PullQuotedTextContent))]
    [XmlInclude(typeof(CustomImageTextContent))]
    [XmlInclude(typeof(BodyCopyTextContent))]
    public class GalleryContentImage : GalleryImageBase
    {
       
    }
}
