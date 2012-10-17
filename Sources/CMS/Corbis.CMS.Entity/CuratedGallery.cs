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
        /// ctor
        /// </summary>
        public CuratedGallery()
        {
            this.Enabled = true;
        }
    }
}
