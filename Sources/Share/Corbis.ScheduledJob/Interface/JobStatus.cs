using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Job.Interface
{
    public enum JobStatus
    {
        Undefined,
        /// <summary>
        /// 
        /// </summary>
        Pending,
        /// <summary>
        /// Job is executing its tasks
        /// </summary>
        Working,
    }
}
