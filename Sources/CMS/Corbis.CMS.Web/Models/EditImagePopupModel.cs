using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class EditImagePopupModel
    {
        [Required]
        public string PopupID { get; set; }

        [Required]
        public Nullable<int> GalleryID { get; set; }

        [Required]
        public string ImageID { get; set; }

    }
}