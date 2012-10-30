using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Gallery template setting provider
    /// </summary>
    public interface IGalleryTemplate
    {
        /// <summary>
        /// Unique template identifier
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Template name. This name presents the gallery in UI
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gallery template description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gallery template image url. It is "face" of the gallery
        /// </summary>
        IImageSource Icon { get; }

        /// <summary>
        /// Gallery Template Author
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gallery Template Company
        /// </summary>
        string Company { get; }

        /// <summary>
        /// Is template default or not
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Is gallery template enabled for gallery creation
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Flag that indicates that record can be delted or not.
        /// </summary>
        bool CanModify { get; }

        /// <summary>
        /// Template date registration
        /// </summary>
        Nullable<DateTime> DateCreated { get; }

        /// <summary>
        /// Global tempate image settings. These settings are applied to all images of the gallery
        /// </summary>
        ITemplateGallerySettings GallerySettings { get; } 
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum GalleryImageSizes
    {
        Small = 1,
        Middle = 2,
        Large = 4,
        All = GalleryImageSizes.Small | GalleryImageSizes.Middle | GalleryImageSizes.Large
    }

    /// <summary>
    /// Settings provider for all galleries created or creating based on this template
    /// </summary>
    public interface ITemplateGallerySettings
    {
        /// <summary>
        /// Image upload path
        /// </summary>
        string ImageUploadPath { get; set; }

        /// <summary>
        /// Max image size in Bytes
        /// </summary>
        Nullable<Size> MaxImageSize { get; set; }

        /// <summary>
        /// Min image size in Bytes
        /// </summary>
        Nullable<Size> MinImageSize { get; set; }

        /// <summary>
        /// Min number of images which can have any gallery which are created based on this template
        /// </summary>
        Nullable<int> MinGalleryImageCount { get; set; }

        /// <summary>
        /// Max number of images which can have any gallery which are created based on this template
        /// </summary>
        Nullable<int> MaxGalleryImageCount { get; set; }

        /// <summary>
        /// Required image sizes for the gallery
        /// </summary>
        GalleryImageSizes RequiredImageSizes { get; }

        /// <summary>
        /// Strict image sizes in pixels
        /// </summary>
        /// <param name="szType"></param>
        /// <returns>Size in pixels or null if this size does not supported</returns>
        Nullable<Size> GetImageSize(GalleryImageSizes szType);
    }

}