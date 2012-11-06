using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Xml.Serialization;
using Corbis.CMS.Entity;
using System.Collections.ObjectModel;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GalleryTemplateDescriptor
    {
        /// <summary>
        /// Gallery Template Author
        /// </summary>
        [XmlElement]
        public string Author { get; set; }

        /// <summary>
        /// Gallery Template Company
        /// </summary>
        [XmlElement]
        public string Company { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Gallery template image url. It is "face" of the gallery
        /// </summary>
        [XmlElement]
        public GalleryImageSource Icon { get; set; }

        /// <summary>
        /// Gallery template description
        /// </summary>
        [XmlElement]
        public string Description { get; set; }

        /// <summary>
        /// Rules applied to all galleries created based on this templete
        /// </summary>
        [XmlElement]
        public TemplateGallerySettings GallerySettings { get; set; }
    }

    [Serializable]
    public class TemplateGallerySettings : ITemplateGallerySettings
    {
        /// <summary>
        /// Image upload path
        /// </summary>
        [XmlElement]
        public string ImageUploadPath { get; set; }

        /// <summary>
        /// Max image size in Bytes
        /// </summary>
        [XmlElement(ElementName = "MaxImageSize", IsNullable = true)]
        public Nullable<Size> MaxImageSize { get; set; }

        /// <summary>
        /// Min image size in Bytes
        /// </summary>
        [XmlElement(ElementName = "MinImageSize", IsNullable = true)]
        public Nullable<Size> MinImageSize { get; set; }

        /// <summary>
        /// Min number of images which can have any gallery which are created based on this template
        /// </summary>
        [XmlElement(ElementName = "MinGalleryImageCount", IsNullable = true)]
        public Nullable<int> MinGalleryImageCount { get; set; }

        /// <summary>
        /// Max number of images which can have any gallery which are created based on this template
        /// </summary>
        [XmlElement(ElementName = "MaxGalleryImageCount", IsNullable = true)]
        public Nullable<int> MaxGalleryImageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray()]
        [XmlArrayItem("ImageSize")]
        public List<GalleryImageSize> ImageSizes
        {
            get { return this.m_ImageSizes; }
        }
        private readonly List<GalleryImageSize> m_ImageSizes = new List<GalleryImageSize>();

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string DefaultFontFamilyName 
        { 
            get
            {
                if(string.IsNullOrEmpty(this.m_DefaultFontFamilyName))
                    this.m_DefaultFontFamilyName = this.FontFamilies[0].Name;

                return this.m_DefaultFontFamilyName;
            }
            set
            {
                this.m_DefaultFontFamilyName = value;
            }
        }
        private string m_DefaultFontFamilyName = null;

        /// <summary>
        /// Each template can have its own font family set.
        /// </summary>
        public FontFamily[] FontFamilies
        {
            get
            {   
                if(m_FontFamilies == null)
                    m_FontFamilies = System.Drawing.FontFamily.Families;

                return this.m_FontFamilies;
            }        
        }
        private FontFamily[] m_FontFamilies = null;


        /// <summary>
        /// Contains gallery image sizes
        /// </summary>
        ReadOnlyCollection<IGalleryImageSize> ITemplateGallerySettings.ImageSizes
        {
            get 
            {
                if (this.m_ReadonlyImageSizes == null)
                    this.m_ReadonlyImageSizes = new ReadOnlyCollection<IGalleryImageSize>(this.ImageSizes.Cast<IGalleryImageSize>().ToList());

                return this.m_ReadonlyImageSizes;
            }
        }
        private ReadOnlyCollection<IGalleryImageSize> m_ReadonlyImageSizes = null;
    }


}