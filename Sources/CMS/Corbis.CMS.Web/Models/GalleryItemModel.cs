﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using Corbis.Public.Entity;

namespace Corbis.CMS.Web.Models
{
    /// <summary>
    /// Gallery list item model
    /// </summary>
    public class GalleryItemModel
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
        /// Template name
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Is gallery enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gallery created date
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gallery created date
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Editor { get; set; }

        /// <summary>
        /// Gallery status
        /// </summary>
        public CuratedGalleryStatuses Status { get; set; }

        /// <summary>
        /// Publication period 
        /// </summary>
        public Range<string> PublicationPeriod { get; set; }

        /// <summary>
        /// Publication url 
        /// </summary>
        public string PublicationUrl { get; set; }

    }
}