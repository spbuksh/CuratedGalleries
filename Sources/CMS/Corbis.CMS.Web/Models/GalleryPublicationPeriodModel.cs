using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class GalleryPublicationPeriodModel : FormattedDateTimeRangeModel
    {
        [Required(ErrorMessage = "Gallery identifier is required")]
        public int GalleryID { get; set; }
    }
}