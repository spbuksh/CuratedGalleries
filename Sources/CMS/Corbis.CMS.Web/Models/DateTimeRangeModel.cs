using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class FormattedDateTimeRangeModel
    {
        /// <summary>
        /// Formatted string for period start datetime
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public virtual string From { get; set; }

        /// <summary>
        /// Formatted string for period end datetime
        /// </summary>
        public virtual string To { get; set; }

        /// <summary>
        /// Date time format
        /// </summary>
        public string DateTimeFormat { get; set; }
    }
}