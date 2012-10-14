using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.Public.Entity.Search
{
    [Serializable, DataContract]
    public class ImageSearchResultItem
    {
        /// <summary>
        /// Public image unique identifier
        /// </summary>
        [DataMember]
        public string CorbisID { get; set; }

        /// <summary>
        /// Internal unique image identifier
        /// </summary>
        [DataMember]
        public long ImageID { get; set; }

        /// <summary>
        /// Image title
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Search rate
        /// </summary>
        [DataMember]
        public decimal Score { get; set; }

        /// <summary>
        /// Category identifier (name is not included because categories will be staic and cached info)
        /// </summary>
        [DataMember]
        public int CategoryID { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }

        /// <summary>
        /// Image collection name
        /// </summary>
        [DataMember]
        public string CollectionName { get; set; }

        /// <summary>
        /// Image Collection identifier
        /// </summary>
        [DataMember]
        public int CollectionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool HasRelatedImages { get; set; }

        /// <summary>
        /// License type
        /// </summary>
        [DataMember]
        public Nullable<LicenseModels> LicenseModel { get; set; }

        /// <summary>
        /// License type
        /// </summary>
        [DataMember]
        public Nullable<int> LicenseType { get; set; }

        /// <summary>
        /// File size in KB
        /// </summary>
        [DataMember]
        public Nullable<long> FileSize { get; set; }

        /// <summary>
        /// Image resolution in ppi
        /// </summary>
        [DataMember]
        public Nullable<int> Resolution { get; set; }

        /// <summary>
        /// Image size in inches
        /// </summary>
        [DataMember]
        public Size<double> SizeInch { get; set; }

        /// <summary>
        /// Image size in pixels
        /// </summary>
        [DataMember]
        public Size<int> SizePx { get; set; }

        /// <summary>
        /// Date photographed
        /// </summary>
        [DataMember]
        public Nullable<DateTime> DatePhotographed { get; set; }

        /// <summary>
        /// Photographer name
        /// </summary>
        [DataMember]
        public string Photographer { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Url for cutted down image (small size)
        /// </summary>
        [DataMember]
        public string CutdownUrl128 { get; set; }

        /// <summary>
        /// Url for cutted down image (medium size)
        /// </summary>
        [DataMember]
        public string CutdownUrl170 { get; set; }

        /// <summary>
        /// Url for cutted down image (large size)
        /// </summary>
        [DataMember]
        public string CutdownUrl350 { get; set; }
    }
}
