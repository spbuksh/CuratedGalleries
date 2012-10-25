﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.CMS.Web.Models
{
    public class GalleryTemplateModel
    {
        /// <summary>
        /// Gallery identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// gallery image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Is this gallery default or not
        /// </summary>
        public bool IsDefault { get; set; }
    }
}