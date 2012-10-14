using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Corbis.Common.Configuration
{
    /// <summary>
    /// Configuration element which describes application
    /// </summary>
    public class ApplicationDetailsElement : ConfigurationElement
    {
        /// <summary>
        /// Application name
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Application version. It is string like 1.0.0.0
        /// </summary>
        [ConfigurationProperty("version", IsRequired = true)]
        public string Version
        {
            get { return this["version"] as string; }
            set { this["version"] = value; }
        }

    }
}
