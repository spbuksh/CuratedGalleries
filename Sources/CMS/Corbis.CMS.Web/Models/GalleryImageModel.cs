using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using Corbis.CMS.Web.Code;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class GalleryImageModelBase
    {
        public int GalleryID { get; set; }

        /// <summary>
        /// Image identifier
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Image order number
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Image url
        /// </summary>
        public ImageUrlSet Urls { get; set; }

        /// <summary>
        /// Image descriptor
        /// </summary>
        public string Text { get; set; }
       
    }

    public class GalleryContentImageModel : GalleryImageModelBase
    {

        public GalleryContentImageModel()
        {
            if(ContentImage == null)
            {
                ContentImage = new GalleryImageContentModel();
            }
        }
        public GalleryImageContentModel ContentImage { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GalleryCoverImageModel : GalleryImageModelBase
    {
        public GalleryCoverImageModel()
        {
            if(ContentImage == null)
            {
                ContentImage = new GalleryImageContentModel();
            }
        }

        public GalleryImageContentModel ContentImage { get; set; }

        public CoverTextContentPositions TextPosition { get; set; }

        [Required(ErrorMessage = "Headline copy must not be empty")]
        public string HeadlineCopyText { get; set; }

        public float HeadlineCopyFontSize { get; set; }

        [Required(ErrorMessage = "Standfirst must not be empty")]
        public string StandfirstText { get; set; }

        public float StandfirstFontSize { get; set; }

        [Required]
        public string Biography { get; set; }
    }

}