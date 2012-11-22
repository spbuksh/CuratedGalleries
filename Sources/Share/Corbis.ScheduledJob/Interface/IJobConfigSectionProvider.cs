using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Interface
{
    public interface IJobConfigSectionProvider
    {
        /// <summary>
        /// Initializes provider
        /// </summary>
        /// <param name="configFilePath">Absolute or relative job configuration file path</param>
        void Initialize(string configFilePath, IUnityContainer container = null);

        /// <summary>
        /// Gets job configuration settings if they exists or null if the are not pointed
        /// </summary>
        /// <param name="jobName">Unique job name</param>
        /// <returns>XML text with job configuration settings if they are pointed or null if the are not set</returns>
        string GetConfigSection(string jobName);
    }
}
