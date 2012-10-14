using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Public.Entity;

namespace Corbis.Presentation.Common.Models
{
    public class CorbisImageModel
    {
        /// <summary>
        /// Image title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Collection name
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Image license mode(s)
        /// </summary>
        public string LicenseModel { get; set; }

        /// <summary>
        /// Date photographed
        /// </summary>
        public string DatePhotographed { get; set; }

        /// <summary>
        /// Unique public image identifier
        /// </summary>
        public string CorbisID { get; set; }

        /// <summary>
        /// Image resolution in ppi
        /// </summary>
        public Nullable<int> Resolution { get; set; }

        /// <summary>
        /// File size in KB
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Photographer { get; set; }


        /// <summary>
        /// Image size in pixels
        /// </summary>
        public Size<int> SizePx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CutdownUrl128 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CutdownUrl170 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CutdownUrl350 { get; set; }
    }
}
