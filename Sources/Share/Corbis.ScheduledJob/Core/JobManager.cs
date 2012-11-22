using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Core;
using System.IO;
using Corbis.Job.Interface;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Unity;

namespace Corbis.Job
{
    public class JobManager : IJobManager
    {
        /// <summary>
        /// Job configuration settings provider
        /// </summary>
        protected virtual IJobConfigSectionProvider JobConfigSectionProvider
        {
            get 
            {
                if (this.m_JobConfigSectionProvider == null)
                    this.m_JobConfigSectionProvider = new JobConfigSectionProvider();

                return this.m_JobConfigSectionProvider;
            }
        }
        private IJobConfigSectionProvider m_JobConfigSectionProvider = null;

        /// <summary>
        /// Unity IoC container to provide and inject logic outside
        /// </summary>
        protected IUnityContainer UnityContainer { get; set; }

        /// <summary>
        /// Job storage
        /// </summary>
        protected virtual IJobStorage JobStorage  { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected System.Threading.Timer Timer { get; set; }

        /// <summary>
        /// Indicates wether manager initialized or not
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public JobManager()
        { }
        ~JobManager()
        {
            this.DisposeCore();
        }

        /// <summary>
        /// Initializes manager
        /// </summary>
        /// <param name="configSection">Configuration section name with settings for initialization</param>
        public virtual void Initialize(string configSection = null, IUnityContainer container = null)
        {
            this.Initialize(JobConfigurationSection.GetSection(configSection), container);
        }

        /// <summary>
        /// Initializes manager
        /// </summary>
        /// <param name="configSection">Configuration section name with settings for initialization</param>
        public virtual void Initialize(JobConfigurationSection section, IUnityContainer container = null)
        {
            if (this.IsInitialized)
                return;

            lock (this)
            {
                //use double blocking multiple initializing
                if (this.IsInitialized)
                    return;

                this.Status = JobManagerStatus.Initializing;

                this.UnityContainer = container;

                //initialize job config provider
                if (!string.IsNullOrEmpty(section.ConfigFilePath))
                    this.JobConfigSectionProvider.Initialize(section.ConfigFilePath);

                //initialize data storage
                this.JobStorage = JobStorageFactory.CreateInstance(section.Storage);

                this.Jobs.Clear();

                foreach (JobConfigElement item in section.Jobs)
                {
                    if (!item.Enabled)
                        continue;

                    var descriptor = this.CreateDescriptor(item);
                    this.Jobs[descriptor.Name] = new JobWrapper(descriptor) { LastExecution = this.JobStorage.GetLastExecutionTime(descriptor.Name) };
                } 

                this.IsInitialized = true;
                this.Status = JobManagerStatus.Stopped;
            }
        }

        /// <summary>
        /// it is used for job thread synchronization
        /// </summary>
        protected List<IAsyncResult> AsyncResults
        {
            get { return this.m_AsyncResults; }
        }
        private readonly List<IAsyncResult> m_AsyncResults = new List<IAsyncResult>();

        /// <summary>
        /// Registered jobs
        /// </summary>
        protected Dictionary<string, JobWrapper> Jobs
        {
            get { return this.m_Jobs; }
        }
        private readonly Dictionary<string, JobWrapper> m_Jobs = new Dictionary<string, JobWrapper>();

        /// <summary>
        /// Creates descriptor based on config element. Configuration section can be extended => it is posiible to change this logic
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        protected virtual IJobDescriptor CreateDescriptor(JobConfigElement elem)
        {
            return new JobDescriptor() { Name = elem.Name, Period = elem.Period, Settings = this.JobConfigSectionProvider.GetConfigSection(elem.Name), Type = elem.Type };
        }

        /// <summary>
        /// Current job manager status
        /// </summary>
        public JobManagerStatus Status 
        {
            get
            {
                return this.m_Status;
            }
            protected set
            {
                if (this.m_Status == value) return;
                this.m_Status = value;
                this.RaiseStatusChanged();
            }
        }
        private JobManagerStatus m_Status = JobManagerStatus.Undefined;

        /// <summary>
        /// Occurs when status changed
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// Raises 'StatusChanged' event
        /// </summary>
        protected virtual void RaiseStatusChanged()
        {
            if (this.StatusChanged != null)
                this.StatusChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Job status changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnJobStatusChanged(object sender, EventArgs args)
        {
            var job = sender as IJob;

            switch (job.Status)
            {
                case JobStatus.Undefined:
                    throw new Exception();
                case JobStatus.Working:
                    break;
                case JobStatus.Pending:
                    {
                        DateTime utc = DateTime.UtcNow;

                        var wrapper = this.Jobs[job.Name];

                        lock (wrapper.SyncRoot)
                        {
                            wrapper.LastExecution = utc;
                        }

                        this.JobStorage.SetLastExecutionTime(job.Name, utc);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (this.AsyncResults.Count != 0)
            {
                lock (this.AsyncResults)
                {
                    //double blocking
                    if (this.AsyncResults.Count != 0)
                    {
                        foreach (var item in this.AsyncResults.Where(x => x.IsCompleted).ToArray())
                            this.AsyncResults.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Starts manager activities
        /// </summary>
        /// <param name="bScheduled"></param>
        /// <param name="period">Scheduled period in miliseconds for job task executing attempt</param>
        /// <param name="data">This data will be passed into jobs. It can be some data provider or simple data</param>
        public virtual void Start(bool bScheduled = false, Nullable<int> period = null, object data = null)
        {
            if (!this.IsInitialized)
                throw new Exception("Manager must be initialized first");

            if (this.Status != JobManagerStatus.Stopped)
                return;

            lock (this)
            {
                if (this.Status != JobManagerStatus.Stopped)
                    return;
            }

            this.Status = JobManagerStatus.Working;

            if (bScheduled)
            {
                if (!period.HasValue && period.Value <= 0)
                    throw new ArgumentException(string.Format("Period is required and must be more than 0"));

                System.Threading.TimerCallback callback = delegate(object state)
                {
                    this.StartCore();
                };

                this.Timer = new System.Threading.Timer(callback, data, 0, period.Value);
            }
            else
            {
                this.StartCore();

                //we started job tasks 1 time => wait for job work finish and change status
                this.Stop();
            }
        }

        protected virtual void StartCore()
        {
            EventHandler statusHandler = null;

            foreach (var item in this.Jobs.Values)
            {
                lock (item.SyncRoot)
                {
                    if (item.Instance == null)
                    {
                        item.Instance = item.Descriptor.CreateInstance(this.UnityContainer);

                        if (statusHandler == null)
                            statusHandler = new EventHandler(this.OnJobStatusChanged);

                        item.Instance.StatusChanged += statusHandler;
                    }
                    else
                    {
                        if (item.LastExecution.HasValue && item.LastExecution.Value.AddMilliseconds(item.Descriptor.Period) > DateTime.UtcNow)
                            continue;
                    }
                }

                var ares = item.Instance.Execute(true);

                if (ares != null)
                {
                    lock (this.AsyncResults)
                    {
                        if (!ares.IsCompleted)
                            this.AsyncResults.Add(ares);
                    }
                }
            }
        }

        /// <summary>
        /// Stops any manager activities
        /// </summary>
        public virtual void Stop(bool wait = true)
        {
            if (this.Status == JobManagerStatus.Stopped)
                return;

            lock (this)
            {
                if (this.Status == JobManagerStatus.Stopped)
                    return;

                this.Status = JobManagerStatus.Stopping;

                if (this.Timer != null)
                {
                    this.Timer.Dispose();
                    this.Timer = null;
                }

                System.Threading.WaitHandle[] handles = null;

                if (this.AsyncResults.Count != 0)
                {
                    lock (this.AsyncResults)
                    {
                        if (this.AsyncResults.Count != 0)
                        {
                            foreach (var item in this.AsyncResults.Where(x => x.IsCompleted).ToArray())
                                this.AsyncResults.Remove(item);

                            handles = this.AsyncResults.Select(x => x.AsyncWaitHandle).ToArray();
                        }
                    }
                }

                if (handles != null && handles.Length != 0)
                {
                    if (wait)
                        System.Threading.WaitHandle.WaitAll(this.AsyncResults.Select(x => x.AsyncWaitHandle).ToArray());
                }

                this.Status = JobManagerStatus.Stopped;
            }
        }

        public void Dispose()
        {
            this.DisposeCore();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeCore()
        {
            lock (this)
            {
                this.Stop();

                foreach (var item in this.Jobs)
                {
                    if (item.Value.Instance == null)
                        continue;

                    item.Value.Instance.Dispose();
                }

                this.Jobs.Clear();
            }
        }
    }

}
