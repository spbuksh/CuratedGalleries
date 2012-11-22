using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Corbis.Common;
using Corbis.Common.Configuration;

namespace Corbis.Job.Core
{
    [DefaultConfigSectionName("JobSection")]
    public class JobConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Loads configuration section
        /// </summary>
        /// <param name="sectionName">configuration section name</param>
        /// <returns></returns>
        public static JobConfigurationSection GetSection(string sectionName = null, bool bThrowException = true)
        {
            return ConfigurationUtils.GetSection<JobConfigurationSection>(sectionName, bThrowException);
        }

        /// <summary>
        /// Relative filepath to job config file
        /// </summary>
        [ConfigurationProperty("jobManagerType")]
        public string JobManagerType
        {
            get { return this["jobManagerType"] as string; }
            set { this["jobManagerType"] = value; }
        }

        /// <summary>
        /// Relative filepath to job config file
        /// </summary>
        [ConfigurationProperty("schedulePeriod", IsRequired = true)]
        public int SchedulePeriod
        {
            get { return (int)this["schedulePeriod"]; }
            set { this["schedulePeriod"] = value; }
        }

        /// <summary>
        /// Relative filepath to job config file
        /// </summary>
        [ConfigurationProperty("jobConfigFilePath", IsRequired = true)]
        public string ConfigFilePath
        {
            get { return this["jobConfigFilePath"] as string; }
            set { this["jobConfigFilePath"] = value; }
        }

        [ConfigurationProperty("storage")]
        public JobStorageElement Storage
        {
            get { return this["storage"] as JobStorageElement; }
            set { this["storage"] = value; }
        }

        /// <summary>
        /// Collection of loggers for logging
        /// </summary>
        [ConfigurationProperty("jobs")]
        [ConfigurationCollection(typeof(JobConfigElement))]
        public JobConfigElementCollection Jobs
        {
            get { return this["jobs"] as JobConfigElementCollection; }
        }
    }

    /// <summary>
    /// Job config element
    /// </summary>
    public class JobConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Job unique name
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Indicates wehther job enabled or not
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Assembly-qualified type name
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// Job pending period in miliseconds
        /// </summary>
        [ConfigurationProperty("period", IsRequired = true)]
        public int Period
        {
            get { return (int)this["period"]; }
            set { this["period"] = value; }
        }
    }

    public class JobConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobConfigElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as JobConfigElement).Name;
        }
    }

    public enum JobStorageModes
    {
        XmlFile,
        InProc,
        DB,
        Custom
    }
    public class JobStorageElement : ConfigurationElement
    {
        /// <summary>
        /// Job pending period in miliseconds
        /// </summary>
        [ConfigurationProperty("mode", IsRequired = true)]
        public JobStorageModes Mode
        {
            get { return (JobStorageModes)this["mode"]; }
            set { this["mode"] = value; }
        }

        /// <summary>
        /// File for job storage
        /// </summary>
        [ConfigurationProperty("filepath")]
        public string FilePath
        {
            get { return (string)this["filepath"]; }
            set { this["filepath"] = value; }
        }

        /// <summary>
        /// Connection string name in connection string sections
        /// </summary>
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        /// <summary>
        /// Assembly-qualified type name
        /// </summary>
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }
    }
}
