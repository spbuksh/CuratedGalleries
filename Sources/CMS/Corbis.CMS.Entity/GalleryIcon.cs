using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Gallery Icon object.
    /// </summary>
    public class GalleryIcon
    {
        /// <summary>
        /// Represents Gallery Icon source Type.
        /// </summary>
        public GalleryIconType GalleryIconType { get; set; }

        /// <summary>
        /// Value specified for provided Type.
        /// </summary>
        public string Value { get; set; }
    }
}
