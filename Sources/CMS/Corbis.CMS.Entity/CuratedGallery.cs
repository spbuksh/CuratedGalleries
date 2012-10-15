using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Possible statuses of any curated gallery
    /// </summary>
    public enum CuratedGalleryStatuses
    {
        /// <summary>
        /// Curated gallery has been created and released
        /// </summary>
        Deployed,
        /// <summary>
        /// Curated gallery is in development process
        /// </summary>
        IsInDevelopment
    }

    /// <summary>
    /// Curated gallery. It is gallery created based on gallery template.
    /// </summary>
    [Serializable, DataContract]
    public class CuratedGallery
    {
        /// <summary>
        /// Gallery unique identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Set of gallery image urls
        /// </summary>
        public ImageUrlSet UrlSet { get; set; }

        /// <summary>
        /// Gallery template identifier
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// Is gallery active or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gallery root directory path.
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// Url to the gallery for preview
        /// </summary>
        public string GalleryUrl { get; set; }

        /// <summary>
        /// Current gallery status
        /// </summary>
        public CuratedGalleryStatuses Status { get; set; }
    }
}
