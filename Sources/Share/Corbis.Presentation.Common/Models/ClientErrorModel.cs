using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Corbis.Presentation.Common.Models
{
    public class ClientLogEntryModel
    {
        /// <summary>
        /// Log entry type
        /// </summary>
        [Required]
        public string EntryType { get; set; }

        /// <summary>
        /// Log entry message
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Error page url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Error page url
        /// </summary>
        public Nullable<int> Line { get; set; }

    }
}
