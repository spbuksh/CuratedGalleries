using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Corbis.Common.Configuration
{
    /// <summary>
    /// Describes running application details
    /// </summary>
    [DefaultConfigSectionName("ApplicationDescriptor")]
    public class ApplicationDescriptorConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Details")]
        public ApplicationDetailsElement ApplicationDetails 
        { 
            get { return this["Details"] as ApplicationDetailsElement; }
            set { this["Details"] = value; }
        }

        /// <summary>
        /// Loads configuration section
        /// </summary>
        /// <param name="sectionName">configuration section name</param>
        /// <returns></returns>
        public static ApplicationDescriptorConfigSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<ApplicationDescriptorConfigSection>(sectionName, bThrowException);
        }

    }

}
