using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    [Serializable, DataContract]
    public class ZipArchivePackage
    {
        /// <summary>
        /// ZIP File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// ZIP file content
        /// </summary>
        public byte[] FileContent { get; set; }

    }
}
