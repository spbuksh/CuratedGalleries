using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Text content position
    /// </summary>
    [Serializable]
    public enum CoverTextContentPositions
    {
        /// <summary>
        /// Text stands at left top angle of the cover image
        /// </summary>
        TopLeft,
        /// <summary>
        /// Text stands at right top angle of the cover image
        /// </summary>
        TopRight,
        /// <summary>
        /// Text stands at center of the cover image
        /// </summary>
        Center,
        /// <summary>
        /// Text stands at left bottom angle of the cover image
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Text stands at right bottom angle of the cover image
        /// </summary>
        BottomRight
    }

    /// <summary>
    /// Gallery cover image. It is always first image in the gallery
    /// </summary>
    [Serializable]
    public class GalleryCoverImage : GalleryImageBase
    {
        public CoverTextContentPositions TextPosition { get; set; }

        public CoverTextItem Headline { get; set; }

        public CoverTextItem Standfirst { get; set; }

        public string Biography { get; set; }
    }

    public class CoverTextItem
    { 
        public string Text { get; set; }

        public float FontSize { get; set; }
    }
}
