using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Corbis.WebAPI.Models
{
    /// <summary>
    /// Search autocomplete suggestions request model
    /// </summary>
    [DataContract]
    public class SearchAutocompleteModel
    {
        /// <summary>
        /// This is the search term for which suggestions are needed
        /// </summary>
        [Required(ErrorMessage = "Text for autocomplete is required")]
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// This is the number of results that are returned
        /// </summary>
        [Required(ErrorMessage = "Number of max result count is required")]
        [DataMember(IsRequired = true, Name = "ResultCount")]
        public Nullable<int> MaxResultCount { get; set; }

        /// <summary>
        /// Culture name of the text
        /// </summary>
        [Required(ErrorMessage = "Culture is required")]
        [DataMember(IsRequired = true)]
        public string Culture { get; set; }
    }
}