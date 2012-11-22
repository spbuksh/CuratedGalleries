using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Interface;

namespace Corbis.Job.Core
{
    public class JobWrapper
    {
        protected JobWrapper()
        { }
        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="descriptor"></param>
        public JobWrapper(IJobDescriptor descriptor)
            : this()
        {
            this.Descriptor = descriptor;
        }

        /// <summary>
        /// Object for data synchronization
        /// </summary>
        public object SyncRoot 
        {
            get { return this.m_SyncRoot; }
        }
        private readonly object m_SyncRoot = new object();

        /// <summary>
        /// Job descriptor
        /// </summary>
        public IJobDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Last execution datetime in UTC format
        /// </summary>
        public Nullable<DateTime> LastExecution { get; set; }

        /// <summary>
        /// Job instance
        /// </summary>
        public IJob Instance { get; set; }

        /// <summary>
        /// Dictionary of dynamic tags and properties
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get { return this.m_Properties; }
        }
        private readonly Dictionary<string, object> m_Properties = new Dictionary<string, object>();
    }
}
