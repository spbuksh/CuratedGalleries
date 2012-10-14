using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Corbis.Common.Configuration;

namespace Corbis.Logging.Core.Configuration
{   
    [DefaultConfigSectionName("LoggingSection")]
    public class LoggingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("async", DefaultValue = false, IsRequired = false)]
        public bool IsAsync
        {
            get { return (bool)this["async"]; }
            set { this["async"] = value; }
        }

        /// <summary>
        /// Application descriptor section name
        /// </summary>
        [ConfigurationProperty("appSectionName", IsRequired = false)]
        public string ApplicationDescriptorSectionName
        {
            get { return this["appSectionName"] as string; }
            set { this["appSectionName"] = value; }
        }

        /// <summary>
        /// Collection of loggers for logging
        /// </summary>
        [ConfigurationProperty("Loggers")]
        public LoggerConfigElementCollection Loggers
        {
            get { return this["Loggers"] as LoggerConfigElementCollection; }
        }

        /// <summary>
        /// Loads configuration section
        /// </summary>
        /// <param name="sectionName">configuration section name</param>
        /// <returns></returns>
        public static LoggingConfigurationSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<LoggingConfigurationSection>(sectionName, bThrowException);
        }
    }

}
