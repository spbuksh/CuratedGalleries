using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Interface
{
    public interface IJob : IDisposable
    {
        /// <summary>
        /// Job unique name. It is used for job identification
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Initializes job with corresponding xml data 
        /// </summary>
        /// <param name="xmlJobConfigSection"></param>
        /// <param name="container">Container to inject outside logic</param>
        void Initialize(string xmlJobConfigSection = null, IUnityContainer container = null);

        /// <summary>
        /// Executes job tasks
        /// </summary>
        /// <param name="async"></param>
        /// <param name="state"></param>
        /// <returns>IAsyncResult if task has not executed otherwise it returns null</returns>
        IAsyncResult Execute(bool async = true, object state = null);

        /// <summary>
        /// Current job status
        /// </summary>
        JobStatus Status { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler StatusChanged;
    }
}
