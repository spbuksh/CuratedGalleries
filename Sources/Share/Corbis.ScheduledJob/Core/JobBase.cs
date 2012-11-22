using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Interface;
using Corbis.Logging.Interface;
using System.Threading;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Unity;

namespace Corbis.Job.Core
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JobBase : IJob
    {
        /// <summary>
        /// Job unique name. It is used for job identification
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates wether job is initialized or not
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// Application event logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Logging.LogManagerProvider.Instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IAsyncResult TaskAsyncResult { get; set; }

        /// <summary>
        /// Unity container to pass and inject outside logic
        /// </summary>
        protected IUnityContainer UnityContainer { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        protected JobBase()
        {
            this.IsInitialized = false;
            this.Status = JobStatus.Undefined;
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~JobBase()
        {
            this.DisposeCore();
        }
        
        /// <summary>
        /// Initializes job with job configuration data
        /// </summary>
        /// <param name="xmlJobConfigSection"></param>
        public void Initialize(string xmlJobConfigSection = null, IUnityContainer container = null)
        {
            if (this.IsInitialized)
                return;

            lock (this)
            {
                if (this.IsInitialized)
                    return;

                this.InitializeCore(xmlJobConfigSection, container);

                this.IsInitialized = true;
                this.Status = JobStatus.Pending;
            }
        }

        /// <summary>
        /// Initializes job with job configuration data
        /// </summary>
        /// <param name="xmlJobConfig"></param>
        protected virtual void InitializeCore(string xmlJobConfig = null, IUnityContainer container = null)
        {
            this.UnityContainer = container;

            if (this.UnityContainer != null)
                this.UnityContainer.BuildUp(this.GetType(), this);
        }

        /// <summary>
        /// Executes job tasks
        /// </summary>
        /// <param name="async"></param>
        /// <param name="state">Input data for task executing</param>
        public IAsyncResult Execute(bool async = true, object state = null)
        {
            if (!this.IsInitialized)
                throw new Exception("Job must be initialized before task executing");

            if (this.Status == JobStatus.Working)
                return this.TaskAsyncResult;

            Action action = delegate()
            {
                lock (this)
                {
                    if (this.Status == JobStatus.Working)
                        return;

                    this.Status = JobStatus.Working;
                    this.ExecuteCore();
                    this.Status = JobStatus.Pending;
                }
            };

            IAsyncResult output = null;

            if (async)
            {
                lock (this)
                {
                    AsyncCallback callback = delegate(IAsyncResult result) 
                    {
                        lock (this)
                        {
                            this.TaskAsyncResult = null;
                        }
                    };
                    this.TaskAsyncResult = action.BeginInvoke(callback, null);
                    output = this.TaskAsyncResult;
                }
            }
            else
            {
                action();
            }

            return output;
        }

        /// <summary>
        /// contains main logic for job task executing
        /// </summary>
        protected abstract void ExecuteCore();

        /// <summary>
        /// Current job status. Must be synchronized
        /// </summary>
        public JobStatus Status
        {
            get 
            {
                return this.m_Status;
            }
            protected set
            {
                if (this.m_Status == value) return;
                this.m_Status = value;
                this.OnStatusChanged();
            }
        }
        private JobStatus m_Status = JobStatus.Undefined;

        /// <summary>
        /// Occurs when status changed
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// Raises 'StatusChanged' event
        /// </summary>
        protected virtual void OnStatusChanged()
        {
            if (this.StatusChanged != null)
                this.StatusChanged(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            this.DisposeCore();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeCore()
        {
            //By default do nothing
        }
    }
}
