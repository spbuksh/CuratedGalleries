using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery cover image. It is always first image in the gallery
    /// </summary>
    [Serializable]
    public class GalleryCoverImage : GalleryImageBase
    {
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
