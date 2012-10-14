using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Corbis.Logging.Core.Configuration
{
    public class LoggerConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Unique logger name
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Is logger enabled for logging or not
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Assembly-qualified type name
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }


    }
}
