using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery font
    /// </summary>
    [Serializable]
    public class GalleryFont
    {
        /// <summary>
        /// Font family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Font size
        /// </summary>
        public float Size { get; set; }
    }
}
