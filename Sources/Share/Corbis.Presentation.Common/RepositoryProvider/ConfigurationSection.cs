using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Corbis.Common;
using Corbis.Common.Configuration;

namespace Corbis.Presentation.Common.RepositoryProvider
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultConfigSectionName("RepositoryProvider")]
    public class RepositoryProviderConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Loads configuration section
        /// </summary>
        /// <param name="sectionName">configuration section name</param>
        /// <returns></returns>
        public static RepositoryProviderConfigurationSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<RepositoryProviderConfigurationSection>(sectionName, bThrowException);
        }

        /// <summary>
        /// Elements which has provider settings
        /// </summary>
        [ConfigurationProperty("Settings")]
        public RepositoryProviderSettingsElement Settings
        {
            get { return this["Settings"] as RepositoryProviderSettingsElement; }
            set { this["Settings"] = value; }
        }
    }

    /// <summary>
    /// Element with repository provider settings
    /// </summary>
    public class RepositoryProviderSettingsElement : ConfigurationElement
    {
        /// <summary>
        /// Repository assembly file path. It defines assembly which has implementors of repository interfaces. The path is absolute or relevant path.
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string FilePath
        {
            get { return this["path"] as string; }
            set { this["path"] = value; }
        }

        /// <summary>
        /// Repository instance creation mode
        /// </summary>
        [ConfigurationProperty("creationMode", DefaultValue = InstanceCreationMode.PerCall, IsRequired = false)]
        public InstanceCreationMode CreationMode
        {
            get { return (InstanceCreationMode)this["creationMode"]; }
            set { this["creationMode"] = value; }
        }
    }
}
