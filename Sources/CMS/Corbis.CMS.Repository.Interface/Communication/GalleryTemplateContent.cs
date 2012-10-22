using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Repository.Interface.Communication
{
    [Serializable, DataContract]
    [Flags]
    public enum GalleryTemplateContent
    {
        /// <summary>
        /// Base gallery attributes
        /// </summary>
        Base = 1,
        /// <summary>
        /// Gallery as zip archive
        /// </summary>
        Archive = 2,
        /// <summary>
        /// 
        /// </summary>
        All = GalleryTemplateContent.Base | GalleryTemplateContent.Archive
    }

}
