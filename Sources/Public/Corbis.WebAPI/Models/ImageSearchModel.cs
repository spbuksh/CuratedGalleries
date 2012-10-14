using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Corbis.Public.Entity;
using Corbis.Public.Entity.Search;

namespace Corbis.WebAPI.Models
{
    /// <summary>
    /// Image search request data
    /// </summary>
    [DataContract]
    public class ImageSearchModel
    {
        /// <summary>
        /// Search mode. If id does not set then SearchMode.Regular is used
        /// </summary>
        [DataMember]
        public Nullable<SearchModes> SearchMode { get; set; }

        /// <summary>
        /// Depends on mode. It is either public image identifier (stock photo id) of free text
        /// </summary>
        [DataMember]
        public string SearchText { get; set; }

        #region Composition Section

        /// <summary>
        /// Composition style
        /// </summary>
        [DataMember]
        public Nullable<CompositionStyle> Style { get; set; }

        /// <summary>
        /// Composition layout
        /// </summary>
        [DataMember]
        public Nullable<CompositionLayout> Layout { get; set; }

        /// <summary>
        /// Composition view-point
        /// </summary>
        [DataMember]
        public Nullable<CompositionViewpoint> Viewpoint { get; set; }

        #endregion Composition Section

        #region Image Attributes Section

        /// <summary>
        /// Categories for search
        /// </summary>
        [DataMember]
        public List<int> Categories { get; set; }

        /// <summary>
        /// List of marketing collection identifiers
        /// </summary>
        [DataMember]
        public List<int> MarketingCollections { get; set; }

        /// <summary>
        /// Entire or partial photograper name
        /// </summary>
        [DataMember]
        public string Photographer { get; set; }

        /// <summary>
        /// Image type (not image extension)
        /// </summary>
        [DataMember]
        public Nullable<ImageTypes> Type { get; set; }

        /// <summary>
        /// Image orientation
        /// </summary>
        [DataMember]
        public Nullable<ImageOrientations> Orientation { get; set; }

        /// <summary>
        /// Image color format
        /// </summary>
        [DataMember]
        public Nullable<ColorFormats> ColorFormat { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [DataMember]
        public Nullable<bool> IsModelReleased { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [DataMember]
        public Nullable<bool> IsPropertyReleased { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [DataMember]
        public Nullable<ImageSizes> Size { get; set; }

        /// <summary>
        /// Date photographed
        /// </summary>
        [DataMember]
        public Range<Nullable<DateTime>> DateCreated { get; set; }

        #endregion Image Attributes Section

        #region People Section

        /// <summary>
        /// Number of people
        /// </summary>
        [DataMember]
        public Nullable<NumbersOfPeople> NumberOfPeople { get; set; }

        /// <summary>
        /// Gender of people
        /// </summary>
        [DataMember]
        public Nullable<GendersOfPeople> Gender { get; set; }

        /// <summary>
        /// Age of people
        /// </summary>
        [DataMember]
        public Nullable<AgesOfPeople> Age { get; set; }

        /// <summary>
        /// Ethnicities of people
        /// </summary>
        [DataMember]
        public Nullable<Ethnicities> Ethnicity { get; set; }

        #endregion People Section

        #region Misc

        /// <summary>
        /// Composition location
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Number last days where image was added
        /// </summary>
        [DataMember]
        public Nullable<int> LastDaysAdded { get; set; }

        /// <summary>
        /// Image date added
        /// </summary>
        [DataMember]
        public Range<Nullable<DateTime>> DateAdded { get; set; }

        /// <summary>
        /// Image date added
        /// </summary>
        [DataMember]
        public string DatePhotographed { get; set; }

        #endregion

        #region Page Filter

        /// <summary>
        /// Page number of items
        /// </summary>
        [DataMember]
        public Nullable<int> PageNumber { get; set; }

        /// <summary>
        /// Page item count
        /// </summary>
        [DataMember]
        public Nullable<int> PageLength { get; set; }

        #endregion Page Filter

        #region Sorting Filter

        //TODO: In the CorbisImages API it does not pointed!!! Clarify and add it!

        #endregion Sorting

    }
}