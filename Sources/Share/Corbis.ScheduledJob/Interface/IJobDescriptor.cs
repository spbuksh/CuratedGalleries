using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Interface
{
    public interface IJobDescriptor
    {
        /// <summary>
        /// Unique job name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Task executing schedule
        /// </summary>
        int Period { get; }

        /// <summary>
        /// Creates job inastance
        /// </summary>
        /// <returns></returns>
        IJob CreateInstance(IUnityContainer container = null);
    }
}
