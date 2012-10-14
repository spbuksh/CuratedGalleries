using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Corbis.Common.Configuration;

namespace Corbis.Common.WebApi
{
    /// <summary>
    /// Corbis client configuration settings section. This section must be encrypted in the future
    /// </summary>
    [DefaultConfigSectionName("WebApiClientSection")]
    public class WebApiClientConfigSection : ConfigurationSection
    {
        /// <summary>
        /// Loads configuration section
        /// </summary>
        /// <param name="sectionName">configuration section name</param>
        /// <returns></returns>
        public static WebApiClientConfigSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<WebApiClientConfigSection>(sectionName, bThrowException);
        }

        [ConfigurationProperty("Settings")]
        public WebApiClientSettingsElement Settings
        {
            get { return this["Settings"] as WebApiClientSettingsElement; }
            set { this["Settings"] = value; }
        }
    }

    public class WebApiClientSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("clientID")]
        public string ClientID
        {
            get { return this["clientID"] as string; }
            set { this["clientID"] = value; }
        }

        [ConfigurationProperty("secret")]
        public string Secret
        {
            get { return this["secret"] as string; }
            set { this["secret"] = value; }
        }

        /// <summary>
        /// Base url for authentication
        /// </summary>
        [ConfigurationProperty("authUrl")]
        public string AuthUrl
        {
            get { return this["authUrl"] as string; }
            set { this["authUrl"] = value; }
        }

        /// <summary>
        /// Base url for provided web api
        /// </summary>
        [ConfigurationProperty("apiUrl")]
        public string ApiUrl
        {
            get { return this["apiUrl"] as string; }
            set { this["apiUrl"] = value; }
        }
    }
}
