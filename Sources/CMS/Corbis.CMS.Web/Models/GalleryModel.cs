using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class GalleryModel
    {
        /// <summary>
        /// Gallery identifier
        /// </summary>
        public Nullable<int> ID { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        [Display(Name = "Gallery Name")]
        [Required(ErrorMessage = "Gallery name can not be empty")]
        public string Name { get; set; }

        /// <summary>
        /// Gallery cover
        /// </summary>
        public GalleryCoverImageModel CoverImage { get; set; }

        /// <summary>
        /// Gallery content images
        /// </summary>
        public List<GalleryContentImageModel> ContentImages { get; set; }

        /// <summary>
        /// Font family
        /// </summary>
        [Display(Name = "Font Family")]
        [Required(ErrorMessage = "Font family can not be empty")]
        public string FontFamily { get; set; }
    }
}