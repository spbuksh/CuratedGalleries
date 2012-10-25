using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    [Serializable, DataContract]
    public class GalleryTemplateInfo
    {
        /// <summary>
        /// ctor
        /// </summary>
        public GalleryTemplateInfo()
        {
            this.Enabled = true;
            this.IsDefault = false;
        }

        /// <summary>
        /// Is gallery active or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Is template default or not
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// It is zip archive of the template package
        /// </summary>
        public ZipArchivePackage Package { get; set; }
    }
}
