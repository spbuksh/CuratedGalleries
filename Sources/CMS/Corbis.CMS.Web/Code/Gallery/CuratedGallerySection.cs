using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Corbis.Common.Configuration;
using System.Drawing;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Configuration section for curated gallery environment
    /// </summary>
    [DefaultConfigSectionName("CuratedGalleryEnvironment")]
    public class CuratedGalleryEnvironmentSection : ConfigurationSection
    {
        /// <summary>
        /// Gets configuration section from app configuration file by section name
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static CuratedGalleryEnvironmentSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<CuratedGalleryEnvironmentSection>(sectionName, bThrowException);
        }

        /// <summary>
        /// Settings of the curated gallery environment
        /// </summary>
        [ConfigurationProperty("Settings", IsRequired = true)]
        public CuratedGalleryEnvironmentSettings Settings
        {
            get { return this["Settings"] as CuratedGalleryEnvironmentSettings; }
            set { this["Settings"] = value; }
        }
    }

    /// <summary>
    /// Settings of the curated gallery environment
    /// </summary>
    public class CuratedGalleryEnvironmentSettings : ConfigurationElement
    {
        /// <summary>
        /// Absolute or relative directory path to folder with templates
        /// </summary>
        [ConfigurationProperty("templateDir", IsRequired = true)]
        public string TemplateDirectory
        {
            get { return this["templateDir"] as string; }
            set { this["templateDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with deployed (released) curated galleries
        /// </summary>
        [ConfigurationProperty("galleryDevDir", IsRequired = true)]
        public string GalleryDevelopmentDirectory
        {
            get { return this["galleryDevDir"] as string; }
            set { this["galleryDevDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with deployed (released) curated galleries
        /// </summary>
        [ConfigurationProperty("galleryLiveDir", IsRequired = true)]
        public string GalleryLiveDirectory
        {
            get { return this["galleryLiveDir"] as string; }
            set { this["galleryLiveDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with shared images
        /// </summary>
        [ConfigurationProperty("sharedDir", IsRequired = true)]
        public string SharedDirectory
        {
            get { return this["sharedDir"] as string; }
            set { this["sharedDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with templates
        /// </summary>
        [ConfigurationProperty("tempDir", IsRequired = true)]
        public string TemporaryDirectory
        {
            get { return this["tempDir"] as string; }
            set { this["tempDir"] = value; }
        }

        /// <summary>
        /// Url to the default template image. If template image is absent then this image is used for gallery presentation
        /// </summary>
        [ConfigurationProperty("defaultTemplateImageUrl", IsRequired = true)]
        public string DefaultTemplateImageUrl
        {
            get { return this["defaultTemplateImageUrl"] as string; }
            set { this["defaultTemplateImageUrl"] = value; }
        }

        /// <summary>
        /// Min image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        [ConfigurationProperty("minImageSize", IsRequired = false)]
        public Nullable<long> MinImageSize
        {
            get { return (Nullable<long>)this["minImageSize"]; }
            set { this["minImageSize"] = value; }
        }

        /// <summary>
        /// Max image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        [ConfigurationProperty("maxImageSize", IsRequired = false)]
        public Nullable<long> MaxImageSize
        {
            get { return (Nullable<long>)this["maxImageSize"]; }
            set { this["maxImageSize"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("imageEditMode", IsRequired = true)]
        public CuratedGalleryImageEditModeSettings ImageEditMode
        {
            get { return (CuratedGalleryImageEditModeSettings)this["imageEditMode"]; }
            set { this["imageEditMode"] = value; }
        }
    }

    public class CuratedGalleryImageEditModeSettings : ConfigurationElement
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("largeImageSizePx", IsRequired = true)]
        public CuratedGalleryImageSize LargeImageSizePx
        {
            get { return (CuratedGalleryImageSize)this["largeImageSizePx"]; }
            set { this["largeImageSizePx"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("smallImageSizePx", IsRequired = true)]
        public CuratedGalleryImageSize SmallImageSizePx
        {
            get { return (CuratedGalleryImageSize)this["smallImageSizePx"]; }
            set { this["smallImageSizePx"] = value; }
        }

    }
    public class CuratedGalleryImageSize : ConfigurationElement 
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("width", IsRequired = true)]
        public int Width
        {
            get { return (int)this["width"]; }
            set { this["width"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("height", IsRequired = true)]
        public int Height
        {
            get { return (int)this["height"]; }
            set { this["height"] = value; }
        }
    }
}