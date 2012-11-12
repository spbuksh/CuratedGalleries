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
        /// Each template can have its own font family set.
        /// </summary>
        [XmlArray()]
        [XmlArrayItem("FontFamily")]
        public List<FontFamilyItem> FontFamilies { get; set; }


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

        /// <summary>
        /// Font families
        /// </summary>
        FontFamily[] ITemplateGallerySettings.FontFamilies
        {
            get 
            {
                if(this.m_FontFamilies == null)
                {
                    var allfonts = System.Drawing.FontFamily.Families.ToList();
                    allfonts.AddRange(GalleryRuntime.ApplicationFonts.Where(x => allfonts.Where(f => f.Name == x.Name).Count() == 0));

                    if (this.FontFamilies == null || this.FontFamilies.Count == 0)
                    {
                        this.m_FontFamilies = allfonts.ToArray();
                    }
                    else
                    {
                        var items = this.FontFamilies.Select(x => x.Name.ToLower()).ToList();
                        this.m_FontFamilies = allfonts.Where(x => items.Contains(x.Name.ToLower())).ToArray();
                    }
                }
                return this.m_FontFamilies;
            }
        }
        private FontFamily[] m_FontFamilies = null;

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        FontFamily ITemplateGallerySettings.DefaultFontFamily
        {
            get
            {
                if (m_DefaultFontFamily == null)
                {
                    if (this.FontFamilies == null || this.FontFamilies.Count == 0)
                        return null;

                    FontFamilyItem font = this.FontFamilies == null ? null : this.FontFamilies.Where(x => x.IsDefault).SingleOrDefault();
                    this.m_DefaultFontFamily = font == null ? this.FontFamilies[0].FontFamily : font.FontFamily;
                }
                return this.m_DefaultFontFamily;
            }
        }
        private FontFamily m_DefaultFontFamily = null;
    }

    /// <summary>
    /// Font family
    /// </summary>
    [Serializable]
    public class FontFamilyItem
    {
        [XmlAttribute("name")]
        public string Name 
        {
            get { return this.m_Name; }
            set
            {
                if (this.m_Name == value) return;
                this.m_Name = value;
                this.m_FontFamily = null;
            }
        }
        private string m_Name = null;

        [XmlAttribute("isDefault")]
        public bool IsDefault { get; set; }

        [XmlIgnore]
        public FontFamily FontFamily
        {
            get 
            {
                if (this.m_FontFamily == null)
                {
                    var font = System.Drawing.FontFamily.Families.Where(x => string.Equals(x.Name, this.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                    if (font == null)
                        throw new Exception(string.Format("Font family '{0}' is not registered in the system", this.Name));

                    this.m_FontFamily = font;
                }
                return this.m_FontFamily;
            }
        }
        private FontFamily m_FontFamily = null;
    }
}