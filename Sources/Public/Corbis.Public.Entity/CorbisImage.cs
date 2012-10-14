using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Public.Entity
{
    /// <summary>
    /// Corbis image
    /// </summary>
    public class CorbisImage
    {
        /// <summary>
        /// Public image unique identifier
        /// </summary>
        public string CorbisID { get; set; }

        /// <summary>
        /// Internal unique image identifier
        /// </summary>
        public long ImageID { get; set; }

        /// <summary>
        /// Date added
        /// </summary>
        public Nullable<DateTime> DateAdded { get; set; }

        /// <summary>
        /// Image title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Marketting collection
        /// </summary>
        public Nullable<int> MarkettingCollectionID { get; set; }

        /// <summary>
        /// Marketting collection name
        /// </summary>
        public string MarkettingCollectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> ArchiveSizeCode { get; set; }

        /// <summary>
        /// TODO which types?
        /// </summary>
        public Nullable<MediaTypes> MediaType { get; set; }


        public string PricingLevelCode { get; set; }

        public Nullable<Guid> PricingLevelID { get; set; }
        public Nullable<int> PricingTier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> MediaCollections { get; set; }
        public Nullable<int> MediaRating { get; set; }
        public Nullable<int> MediaState { get; set; }

        /// <summary>
        /// Image color format
        /// </summary>
        public Nullable<ColorFormats> ColorFormat { get; set; }

        /// <summary>
        /// Image orientation
        /// </summary>
        public Nullable<ImageOrientations> Orientation { get; set; }

        /// <summary>
        /// Image credit line
        /// </summary>
        public string CreditLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<ImageSeachAccesses> SearchAccess { get; set; }

        public Nullable<ImageReleasedItems> ReleasedItems { get; set; }

        /// <summary>
        /// Image keywords
        /// </summary>
        public List<string> Keywords 
        {
            get { return this.m_Keywords; }
        }
        private readonly List<string> m_Keywords = new List<string>();

        /// <summary>
        /// Category identifier (name is not included because categories will be staic and cached info)
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> HasRelatedImages { get; set; }

        public List<string> ReferenceImageIDs { get; set; }

        /// <summary>
        /// License model
        /// </summary>
        public Nullable<LicenseModels> LicenseModel { get; set; }

        /// <summary>
        /// License type
        /// </summary>
        public Nullable<int> LicenseType { get; set; }

        /// <summary>
        /// File size in KB
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Image resolution in ppi
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// Image size in inches
        /// </summary>
        public Size<double> SizeInch { get; set; }

        /// <summary>
        /// Image size in pixels
        /// </summary>
        public Size<int> SizePx { get; set; }

        /// <summary>
        /// Date photographed
        /// </summary>
        public Nullable<DateTime> DatePhotographed { get; set; }

        /// <summary>
        /// Photographer name
        /// </summary>
        public string Photographer { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Url for cutted down image (small size)
        /// </summary>
        public string CutdownUrl128 { get; set; }

        /// <summary>
        /// Url for cutted down image (medium size)
        /// </summary>
        public string CutdownUrl170 { get; set; }

        /// <summary>
        /// Url for cutted down image (large size)
        /// </summary>
        public string CutdownUrl350 { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, Flags]
    public enum ImageReleasedItems
    {
        Model = 1,
        Property = 2,
        All = ImageReleasedItems.Model | ImageReleasedItems.Property
    }

    [Serializable, Flags]
    public enum ImageSeachAccesses
    {
        External = 1,
        Internal = 2,
        All = ImageSeachAccesses.External | ImageSeachAccesses.Internal
    }

    [Serializable, Flags]
    public enum MediaTypes
    { }

    #region Composition

    /// <summary>
    /// Composition style predefined values
    /// </summary>
    [Serializable, Flags]
    public enum CompositionStyle
    {
        Blur = 1,
        Indoors = 2,
        Outdoors = 4,
        Silhouette = 8,
        StudioShot = 16,
        All = CompositionStyle.Blur | CompositionStyle.Indoors | CompositionStyle.Outdoors | CompositionStyle.Silhouette | CompositionStyle.StudioShot
    }

    /// <summary>
    /// Composition style predefined values
    /// </summary>
    [Serializable, Flags]
    public enum CompositionLayout
    {
        ColorBackground = 1,
        CopySpace = 2,
        Cropped = 4,
        CutOut = 8,
        FullLength = 16,
        HeadAndShoulders = 32,
        WhiteBackground = 64,
        All = CompositionLayout.ColorBackground | CompositionLayout.CopySpace | CompositionLayout.Cropped |
            CompositionLayout.CutOut | CompositionLayout.FullLength | CompositionLayout.HeadAndShoulders | CompositionLayout.WhiteBackground
    }

    /// <summary>
    /// Composition style predefined values
    /// </summary>
    [Serializable, Flags]
    public enum CompositionViewpoint
    {
        Aerial = 1,
        CloseUp = 2,
        FromAbove = 4,
        FromBehind = 8,
        FromBelow = 16,
        LookingAtCamera = 32,
        LookingAwayFromCamera = 64,
        All = CompositionViewpoint.Aerial | CompositionViewpoint.CloseUp | CompositionViewpoint.FromAbove | CompositionViewpoint.FromBehind |
            CompositionViewpoint.FromBelow | CompositionViewpoint.LookingAtCamera | CompositionViewpoint.LookingAwayFromCamera
    }

    #endregion Composition

    #region Image Attributes

    [Serializable, Flags]
    public enum LicenseModels
    {
        /// <summary>
        /// Rights managed
        /// </summary>
        RM = 1,
        /// <summary>
        /// Royalty free
        /// </summary>
        RF = 2,
        /// <summary>
        /// Any color. Union of all possible color format values.
        /// </summary>
        All = LicenseModels.RM | LicenseModels.RF
    }

    [Serializable, Flags]
    public enum ColorFormats
    {
        /// <summary>
        /// Full color
        /// </summary>
        Color = 1,
        /// <summary>
        /// Black and white
        /// </summary>
        BW = 2,
        /// <summary>
        /// Any color. Union of all possible color format values.
        /// </summary>
        All = ColorFormats.Color | ColorFormats.BW
    }

    [Serializable, Flags]
    public enum ImageOrientations
    {
        Horizontal = 1,
        Panorama = 2,
        Square = 4,
        Vertical = 8,
        All = ImageOrientations.Horizontal | ImageOrientations.Panorama | ImageOrientations.Square | ImageOrientations.Vertical
    }

    [Serializable, Flags]
    public enum ImageTypes
    {
        Illustration = 1,
        Photography = 2,
        All = ImageTypes.Illustration | ImageTypes.Photography
    }

    [Serializable, Flags]
    public enum ImageSizes
    {
        Web = 1,
        Small = 2,
        Medium = 4,
        Lagest = 8,
        All = ImageSizes.Lagest | ImageSizes.Medium | ImageSizes.Small | ImageSizes.Web
    }

    #endregion Image Attributes

    #region Pepople

    /// <summary>
    /// Number of people predefined values
    /// </summary>
    [Serializable, Flags]
    public enum NumbersOfPeople
    {
        /// <summary>
        /// No people on the picture
        /// </summary>
        NoPeople = 1,
        /// <summary>
        /// Single person on the picture
        /// </summary>
        Person1 = 2,
        /// <summary>
        /// 2 persons on the picture
        /// </summary>
        Person2 = 4,
        /// <summary>
        /// 3 persons on the picture
        /// </summary>
        Person3 = 8,
        /// <summary>
        /// 4 persons on the picture
        /// </summary>
        Person4 = 16,
        /// <summary>
        /// 5 persons on the picture
        /// </summary>
        Person5 = 32,
        /// <summary>
        /// 6+ persons on the picture
        /// </summary>
        Group = 64,
        All = NumbersOfPeople.NoPeople | NumbersOfPeople.Person1 | NumbersOfPeople.Person2 | NumbersOfPeople.Person3 |
            NumbersOfPeople.Person4 | NumbersOfPeople.Person5 | NumbersOfPeople.Group
    }

    /// <summary>
    /// Gender of people predefined values
    /// </summary>
    [Serializable, Flags]
    public enum GendersOfPeople
    {
        /// <summary>
        /// Male only
        /// </summary>
        MaleOnly = 1,
        /// <summary>
        /// Female only
        /// </summary>
        FemaleOnly = 2,
        /// <summary>
        /// Both male and female
        /// </summary>
        Mix = 4,
        /// <summary>
        /// All possible values
        /// </summary>
        All = GendersOfPeople.FemaleOnly | GendersOfPeople.MaleOnly | GendersOfPeople.Mix
    }

    /// <summary>
    /// Age predefined values
    /// </summary>
    [Serializable, Flags]
    public enum AgesOfPeople
    {
        /// <summary>
        /// 0-23 month
        /// </summary>
        Baby = 1,
        /// <summary>
        /// 2-9 years
        /// </summary>
        Child = 2,
        /// <summary>
        /// 10-12 years
        /// </summary>
        PreTeen = 4,
        /// <summary>
        /// 13-17 years
        /// </summary>
        Teenager = 8,
        /// <summary>
        /// 18-29 years
        /// </summary>
        YoungAdult = 16,
        /// <summary>
        /// 30-39 years
        /// </summary>
        MidAdult = 32,
        /// <summary>
        /// 40-59 years
        /// </summary>
        MiddleAged = 64,
        /// <summary>
        /// 60+ years
        /// </summary>
        Senior = 128,
        /// <summary>
        /// All possible values
        /// </summary>
        All = AgesOfPeople.Baby | AgesOfPeople.Child | AgesOfPeople.MidAdult | AgesOfPeople.MiddleAged | AgesOfPeople.PreTeen |
            AgesOfPeople.Senior | AgesOfPeople.Teenager | AgesOfPeople.YoungAdult
    }

    /// <summary>
    /// Ethnicity predefined values
    /// </summary>
    [Serializable, Flags]
    public enum Ethnicities
    {
        African = 1,
        Asian = 2,
        Caucasian = 4,
        Hispanic = 8,
        MiddleEastern = 16,
        Multiethnic = 32,
        All = Ethnicities.African | Ethnicities.Asian | Ethnicities.Caucasian | Ethnicities.Hispanic | Ethnicities.MiddleEastern | Ethnicities.Multiethnic
    }

    #endregion People

}
