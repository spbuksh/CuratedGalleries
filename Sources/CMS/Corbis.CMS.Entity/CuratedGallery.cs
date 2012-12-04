using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
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
        /// Gallery template identifier
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// Is gallery active or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Creation date. Has UTC format
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Last modified date. Has UTC format
        /// </summary>
        public Nullable<DateTime> DateModified { get; set; }

        /// <summary>
        /// Zip archive with current gallery state.
        /// </summary>
        public ZipArchivePackage Package { get; set; }

        /// <summary>
        /// Admin person who is editing this gallery. We must prevent gallery editing wy several persons
        /// </summary>
        public AdminUserInfo Editor { get; set; }

        /// <summary>
        /// Current gallery status
        /// </summary>
        public CuratedGalleryStatuses Status { get; set; }

        /// <summary>
        /// Gallery publication period
        /// </summary>
        public GalleryPublicationPeriod PublicationPeriod { get; set; }

        /// <summary>
        /// Gallery publisher
        /// </summary>
        public AdminUserInfo Publisher { get; set; }

        /// <summary>
        /// True - this gallery is in edit mode now.
        /// </summary>
        public bool IsInEditMode
        {
            get { return this.Editor != null; }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CuratedGallery()
        {
            this.Enabled = true;
            this.Status = CuratedGalleryStatuses.UnPublished;
        }
    }
}
