using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using Corbis.Job.Interface;
using Microsoft.Practices.Unity;


namespace Corbis.Job.Core
{
    /// <summary>
    /// This class incapsulate job configuration file structure
    /// </summary>
    public class JobConfigSectionProvider : IJobConfigSectionProvider, IDisposable
    {
        protected XDocument XDoc { get; set; }

        protected IUnityContainer UnityContainer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilePath">absolute or relative job configuration file path</param>
        public virtual void Initialize(string configFilePath, IUnityContainer container = null)
        {
            string filepath = configFilePath;
            this.UnityContainer = container;

            if (!Path.IsPathRooted(filepath))
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath.TrimStart(Path.DirectorySeparatorChar));

            if (!File.Exists(filepath))
                throw new FileNotFoundException(string.Format("Job configuration file '{0}' was not found", filepath));

            this.XDoc = XDocument.Load(filepath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobName">Unique job name</param>
        /// <returns></returns>
        public string GetConfigSection(string jobName)
        {
            if (this.XDoc == null)
                throw new Exception("Provider must be initialized first");

            var xelem = this.XDoc.XPathSelectElement(string.Format("//*/job[@name='{0}']", jobName));
            return xelem == null ? null : xelem.ToString();
        }

        public virtual void Dispose()
        {
            //By default do nothing
        }
    }
}
