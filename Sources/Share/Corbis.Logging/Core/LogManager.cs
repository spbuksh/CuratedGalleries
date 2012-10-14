using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;
using Corbis.Logging.Core.Configuration;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;
using Corbis.Common.Configuration;
using Corbis.Common;

namespace Corbis.Logging.Core
{
    /// <summary>
    /// Base class for log managers. It is thread safe class and containd default implementation
    /// </summary>
    public class LogManager : ILogManager
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public LogManager()
        {
            this.IsInitialized = false;
            this.ApplicationDescriptor = new ApplicationDescriptor();
        }
        /// <summary>
        /// Class desctructor
        /// </summary>
        ~LogManager()
        {
            this.DisposeInstance();
        }

        /// <summary>
        /// Indicates if logging is asynchronic or not
        /// </summary>
        protected virtual bool IsAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ApplicationDescriptor ApplicationDescriptor { get; set; }

        /// <summary>
        /// Indicates is instance initialized or not
        /// </summary>
        public bool IsInitialized { get; protected set; }

        protected virtual void CheckInit()
        {
            if (!this.IsInitialized)
                throw new Exception("Instance is not initialized");
        }

        /// <summary>
        /// Initializes log manager instance
        /// </summary>
        /// <param name="configSection">Configuration section name</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Initialize(string configSection = null)
        {
            if (this.IsInitialized)
                return;

            LoggingConfigurationSection logSection = LoggingConfigurationSection.GetSection(configSection);

            if (object.ReferenceEquals(logSection, null))
                throw new Exception("Configuration section can not be loaded");

            this.IsAsync = logSection.IsAsync;

            foreach (LoggerConfigElement item in logSection.Loggers)
            { 
                if(!item.Enabled)
                    continue;

                var type = Type.GetType(item.TypeName);

                if (!typeof(ILogger).IsAssignableFrom(type))
                    throw new Exception(string.Format("Type '{0}' does not implement interface '{1}'", type.AssemblyQualifiedName, typeof(ILogger).AssemblyQualifiedName));

                var logger = Activator.CreateInstance(type) as ILogger;

                if (this.Loggers.ContainsKey(item.Name))
                    throw new Exception("Configuration section for logging has duplicate names");

                this.Loggers.Add(item.Name, logger);
            }

            var appSection = ApplicationDescriptorConfigSection.GetSection(logSection.ApplicationDescriptorSectionName, false);

            if (appSection != null)
            {
                this.ApplicationDescriptor.Name = appSection.ApplicationDetails.Name;
                this.ApplicationDescriptor.Version = appSection.ApplicationDetails.Version;
            }

            this.IsInitialized = true;
        }

        /// <summary>
        /// Enabled registered loggers
        /// </summary>
        protected Dictionary<string, ILogger> Loggers 
        {
            get { return this.m_Loggers; } 
        }
        private readonly Dictionary<string, ILogger> m_Loggers = new Dictionary<string, ILogger>();


        /// <summary>
        /// Get logger by logger name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ILogger GetLogger(string name)
        {
            this.CheckInit();
            if (string.IsNullOrEmpty(name)) throw new ArgumentException();
            return this.Loggers.ContainsKey(name) ? this.Loggers[name] : null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            this.DisposeInstance();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose loggers and unmanaged resources
        /// </summary>
        protected virtual void DisposeInstance()
        {
            if (!this.IsInitialized)
                return;

            foreach (var key in this.Loggers.Keys)
            {
                try
                {
                    this.Loggers[key].Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            this.Loggers.Clear();
            this.IsInitialized = false;
        }

        /// <summary>
        /// Writes entry
        /// </summary>
        /// <param name="entry"></param>
        public virtual void Write(LogEntry entry)
        {
            if (object.ReferenceEquals(entry, null))
                throw new NotImplementedException();

            this.CheckInit();

            if (this.IsAsync)
            {
                WaitCallback callback = 
                    delegate(object state) 
                    {
                        try
                        {
                            this.WriteEntry(state as LogEntry);
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    };
                ThreadPool.QueueUserWorkItem(callback, entry);
            }
            else
            {
                this.WriteEntry(entry);
            }
        }

        protected virtual void WriteEntry(LogEntry entry)
        {
            this.CheckInit();

            foreach (var logger in this.Loggers.Values)
            {
                try
                {
                    logger.Write(entry);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }


        public virtual void WriteError(Exception ex, string message = null, bool isFatal = false, string userID = null)
        {
            LogEntry entry = new LogEntry();
            entry.EntryType = isFatal ? LogEntryType.FatalError : LogEntryType.Error;
            entry.Exception = ex;
            entry.Message = message;
            entry.UserID = userID;
            entry.DateTimeUTC = DateTime.UtcNow;
            entry.Application = this.ApplicationDescriptor.Name;
            entry.ApplicationVersion = this.ApplicationDescriptor.Version;
            this.WriteEntry(entry);
        }
        public virtual void WriteError(string message, bool isFatal = false, string userID = null)
        {
            LogEntry entry = new LogEntry();
            entry.EntryType = isFatal ? LogEntryType.FatalError : LogEntryType.Error;
            entry.Message = message;
            entry.UserID = userID;
            entry.DateTimeUTC = DateTime.UtcNow;
            entry.Application = this.ApplicationDescriptor.Name;
            entry.ApplicationVersion = this.ApplicationDescriptor.Version;
            this.WriteEntry(entry);
        }
        public virtual void WriteWarning(string warn, string userID = null)
        {
            LogEntry entry = new LogEntry();
            entry.EntryType = LogEntryType.Warning;
            entry.Message = warn;
            entry.UserID = userID;
            entry.DateTimeUTC = DateTime.UtcNow;
            entry.Application = this.ApplicationDescriptor.Name;
            entry.ApplicationVersion = this.ApplicationDescriptor.Version;
            this.WriteEntry(entry);
        }
        public virtual void WriteDebug(string info, string userID = null)
        {
            LogEntry entry = new LogEntry();
            entry.EntryType = LogEntryType.Debug;
            entry.Message = info;
            entry.UserID = userID;
            entry.DateTimeUTC = DateTime.UtcNow;
            entry.Application = this.ApplicationDescriptor.Name;
            entry.ApplicationVersion = this.ApplicationDescriptor.Version;
            this.WriteEntry(entry);
        }
        public virtual void WriteInfo(string info, string userID = null)
        {
            LogEntry entry = new LogEntry();
            entry.EntryType = LogEntryType.Info;
            entry.Message = info;
            entry.UserID = userID;
            entry.DateTimeUTC = DateTime.UtcNow;
            entry.Application = this.ApplicationDescriptor.Name;
            entry.ApplicationVersion = this.ApplicationDescriptor.Version;
            this.WriteEntry(entry);
        }

    }
}
