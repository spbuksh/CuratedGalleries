using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Corbis.Public.Entity.Search
{
    [Serializable]
    public enum SearchModes
    {
        /// <summary>
        /// Search by public image id (stock id)
        /// </summary>
        Identifier = 1,
        /// <summary>
        /// Free text search with other filters
        /// </summary>
        Regular = 2,
        /// <summary>
        /// 
        /// </summary>
        MoreOrLikeThis = 3
    }

    [Serializable]
    public class ImageSearchContext
    {
        /// <summary>
        /// Search mode. If id does not set then SearchMode.Regular is used
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<SearchModes> SearchMode { get; set; }

        /// <summary>
        /// Depends on mode. It is either public image identifier (stock photo id) of free text
        /// </summary>
        public virtual string SearchText { get; set; }

        #region Composition Section

        /// <summary>
        /// Composition style
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<CompositionStyle> Style { get; set; }

        /// <summary>
        /// Composition layout
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<CompositionLayout> Layout { get; set; }

        /// <summary>
        /// Composition view-point
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<CompositionViewpoint> Viewpoint { get; set; }

        #endregion Composition Section

        #region Image Attributes Section

        /// <summary>
        /// Categories for search
        /// </summary>
        public virtual List<int> Categories { get; set; }

        /// <summary>
        /// List of marketing collection identifiers
        /// </summary>
        public virtual List<int> MarketingCollections { get; set; }

        /// <summary>
        /// Entire or partial photograper name
        /// </summary>
        public virtual string Photographer { get; set; }

        /// <summary>
        /// Image type (not image extension)
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<ImageTypes> Type { get; set; }

        /// <summary>
        /// Image orientation
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<ImageOrientations> Orientation { get; set; }

        /// <summary>
        /// Image color format
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<ColorFormats> ColorFormat { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<bool> IsModelReleased { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<bool> IsPropertyReleased { get; set; }

        /// <summary>
        /// Image releases
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<ImageSizes> Size { get; set; }

        /// <summary>
        /// Date photographed
        /// </summary>
        //[XmlElement(IsNullable = true)]
        public virtual Range<Nullable<DateTime>> DateCreated { get; set; }

        #endregion Image Attributes Section

        #region People Section

        [XmlElement(IsNullable = true)]
        public virtual Nullable<NumbersOfPeople> NumberOfPeople { get; set; }

        [XmlElement(IsNullable = true)]
        public virtual Nullable<GendersOfPeople> Gender { get; set; }

        [XmlElement(IsNullable = true)]
        public virtual Nullable<AgesOfPeople> Age { get; set; }

        [XmlElement(IsNullable = true)]
        public virtual Nullable<Ethnicities> Ethnicity { get; set; }

        #endregion People Section

        #region Misc

        /// <summary>
        /// Composition location
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        /// Number last days where image was added
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<int> LastDaysAdded { get; set; }

        /// <summary>
        /// Image date added
        /// </summary>
        public virtual Range<Nullable<DateTime>> DateAdded { get; set; }

        /// <summary>
        /// Image date added
        /// </summary>
        public virtual string DatePhotographed { get; set; }

        #endregion

        #region Page Filter

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(IsNullable = true)]
        public virtual Nullable<int> PageNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public virtual Nullable<int> PageLength { get; set; }

        #endregion Page Filter

        #region Sorting Filter

        //TODO: In the CorbisImages API it does not pointed!!! Clarify and add it!

        #endregion Sorting
    }
}
