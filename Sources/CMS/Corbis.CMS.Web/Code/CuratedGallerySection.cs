using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Corbis.Common.Configuration;

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
        [ConfigurationProperty("deploymentDir", IsRequired = true)]
        public string DeploymentDirectory
        {
            get { return this["deploymentDir"] as string; }
            set { this["deploymentDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with curated galleries which are being developed
        /// </summary>
        [ConfigurationProperty("developmentDir", IsRequired = true)]
        public string DevelopmentDirectory
        {
            get { return this["developmentDir"] as string; }
            set { this["developmentDir"] = value; }
        }

        /// <summary>
        /// Absolute or relative directory path to folder with shared images
        /// </summary>
        [ConfigurationProperty("sharedImageDir", IsRequired = true)]
        public string SharedImageDirectory
        {
            get { return this["sharedImageDir"] as string; }
            set { this["sharedImageDir"] = value; }
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
    }
}