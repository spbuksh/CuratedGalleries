using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Job.Interface
{
    /// <summary>
    /// Local job storage
    /// </summary>
    public interface IJobStorage
    {
        /// <summary>
        /// Gets last job execution time in utc format
        /// </summary>
        /// <param name="jobName">Unique job name</param>
        /// <returns></returns>
        Nullable<DateTime> GetLastExecutionTime(string jobName);

        /// <summary>
        /// Saves last job execution time in utc format
        /// </summary>
        /// <param name="jobName">Unique job name</param>
        /// <param name="utc">Execution date time in utc format</param>
        void SetLastExecutionTime(string jobName, DateTime utc);
    }
}
