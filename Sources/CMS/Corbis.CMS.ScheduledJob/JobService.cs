using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Corbis.Logging.Interface;
using Corbis.Job;
using Corbis.Job.Core;
using Corbis.Job.Interface;

namespace Corbis.CMS.ScheduledJob
{
    public partial class JobService : ServiceBase
    {
        public JobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.StartService();
        }

        protected override void OnStop()
        {
        }

        /// <summary>
        /// Application logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Corbis.Logging.LogManagerProvider.Instance; }
        }

        protected IJobManager JobManager { get; set; }

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
#if DEBUG
            Thread.Sleep(10000);
#endif

            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs e)
                {
                    Exception ex = e.ExceptionObject as Exception;

                    if (this.Logger != null)
                        this.Logger.WriteError(ex);

                    EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                };

            ThreadStart startHandler = delegate()
            {
                try
                {
                    var section = JobConfigurationSection.GetSection();

                    if (!string.IsNullOrEmpty(section.JobManagerType))
                    {
                        Type type = Type.GetType(section.JobManagerType);

                        if (type == null)
                            throw new TypeLoadException(string.Format("Type '{0}' can not be loaded", section.JobManagerType));

                        if (!typeof(IJobManager).IsAssignableFrom(type))
                            throw new Exception(string.Format("Type '{0}' must implement '{1}'", section.JobManagerType, typeof(IJobManager).AssemblyQualifiedName));

                        this.JobManager = Activator.CreateInstance(type) as IJobManager;
                    }
                    else
                    {
                        this.JobManager = new JobManager();
                    }

                    this.JobManager.Initialize();
                    this.JobManager.Start(true, section.SchedulePeriod); 
                }
                catch (Exception ex)
                {
                    if (this.Logger != null)
                        this.Logger.WriteError(ex);

                    EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);

#if DEBUG
                    throw;
#endif
                }
            };

            // Start in separated thread not to block standard thread start logic (we have limit 30 sec)
            Thread thread = new Thread(startHandler) { IsBackground = true };
            thread.Start();
        }

    }
}
