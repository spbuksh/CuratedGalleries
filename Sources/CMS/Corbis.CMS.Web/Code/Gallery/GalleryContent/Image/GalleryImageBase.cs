using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Base class for any gallery image 
    /// </summary>
    [Serializable]
    public class GalleryImageBase
    {
        /// <summary>
        /// Unique gallery image identifier for usage in gallery output. 
        /// In general case 'ID' and 'ImageID' is the same. But image identifier in output can have strict name rules
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Unique gallery image identifier. It is internal identifier
        /// </summary>
        public string ImageID { get; set; }

        /// <summary>
        /// Image url. Gallery will reference to the image using this url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Image fime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image source
        /// </summary>
        public GalleryImageSource ImageSource { get; set; }
    }
}
