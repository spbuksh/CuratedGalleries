using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Interface;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Core
{
    public class JobDescriptor : IJobDescriptor
    {
        /// <summary>
        /// Job name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// XML job settings
        /// </summary>
        public string Settings { get; set; }

        /// <summary>
        /// Job assembly-qualified type name
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Shedule period
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IJob CreateInstance(IUnityContainer container = null)
        {
            var type = System.Type.GetType(this.Type);

            if (type == null)
                throw new TypeLoadException(string.Format("Type '{0}' can not be loaded", this.Type));

            if (!typeof(IJob).IsAssignableFrom(type))
                throw new Exception(string.Format("Type '{0}' must imaplment interface '{1}'", this.Type, typeof(IJob).AssemblyQualifiedName));

            var output = Activator.CreateInstance(type) as IJob;
            output.Name = this.Name;
            output.Initialize(this.Settings, container);

            return output;
        }
    }
}
