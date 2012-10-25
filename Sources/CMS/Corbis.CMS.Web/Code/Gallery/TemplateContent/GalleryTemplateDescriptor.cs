using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Xml.Serialization;
using Corbis.CMS.Entity;

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
        [XmlIgnore]
        public GalleryImageSizes RequiredImageSizes
        {
            get 
            {
                if (!this.m_RequiredImageSizes.HasValue)
                {
                    if (this.ImageSizes != null && this.ImageSizes.Count != 0)
                    {
                        foreach (var item in this.ImageSizes)
                            this.m_RequiredImageSizes = this.m_RequiredImageSizes.HasValue ? (this.m_RequiredImageSizes | item.Type) : item.Type;
                    }
                }
                return this.m_RequiredImageSizes.Value;
            }
        }
        private Nullable<GalleryImageSizes> m_RequiredImageSizes = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="szType"></param>
        /// <returns></returns>
        public Nullable<Size> GetImageSize(GalleryImageSizes szType)
        {
            if ((this.RequiredImageSizes & szType) != szType)
                throw new Exception();

            return this.ImageSizes.Where(x => x.Type == szType).Select(x => new Size(x.Width, x.Height)).Single();
        }
    }

    [Serializable]
    public class GalleryImageSize
    {
        /// <summary>
        /// Image size type
        /// </summary>
        [XmlAttribute("sztype")]
        public GalleryImageSizes Type { get; set; }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Image height in pixels
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }
    }

}