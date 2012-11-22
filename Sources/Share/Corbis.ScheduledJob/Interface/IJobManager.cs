using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Interface
{
    /// <summary>
    /// Job manager
    /// </summary>
    public interface IJobManager : IDisposable
    {
        /// <summary>
        /// Initializes manager
        /// </summary>
        /// <param name="configSection">Configuration section name with settings for initialization</param>
        /// <param name="container">Container provider for object build up</param>
        void Initialize(string configSection = null, IUnityContainer container = null);

        /// <summary>
        /// Starts manager activities
        /// </summary>
        void Start(bool bScheduled = false, Nullable<int> period = null, object data = null);

        /// <summary>
        /// Stops all manager activities
        /// </summary>
        /// <param name="wait">Wait until all jobs stop</param>
        void Stop(bool wait = true);

        /// <summary>
        /// Current job manager status
        /// </summary>
        JobManagerStatus Status { get; }

        /// <summary>
        /// Occurs when manager status was changed
        /// </summary>
        event EventHandler StatusChanged;
    }

    public enum JobManagerStatus
    {
        Undefined,
        Initializing,
        Stopped,
        Stopping,
        Working
    }
}
