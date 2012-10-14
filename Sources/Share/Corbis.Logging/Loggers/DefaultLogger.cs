using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;
using Corbis.Logging.Core;
using NLog;
using System.Web;
using System.Runtime.CompilerServices;
using System.IO;
using System.Diagnostics;

namespace Corbis.Logging.Loggers
{
    /// <summary>
    /// This default logger It is wrapper above NLog
    /// </summary>
    public class DefaultLogger : LoggerBase
    {
        /// <summary>
        /// Native logger
        /// </summary>
        protected virtual NLog.Logger NLogger { get; set; }

        /// <summary>
        /// Writes log entry
        /// </summary>
        /// <param name="entry"></param>
        public override void Write(LogEntry entry)
        {
            if(!this.IsInitialized)
                this.Initialize();

            var einfo = this.CreateLogEventInfo(entry);
            einfo.LoggerName = this.NLogger.Name;
            this.NLogger.Log(einfo);
        }

        protected bool IsInitialized { get; set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual void Initialize()
        {
            if (this.IsInitialized)
                return;

            if (this.NLogger == null)
            {
                try
                {
                    string configPath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "NLog.config");

                    if (configPath.StartsWith("file:"))
                        configPath = configPath.Substring("file:".Length).TrimStart(Path.DirectorySeparatorChar);

                    NLog.Config.LoggingConfiguration config = new NLog.Config.XmlLoggingConfiguration(configPath);

                    using (var context = new NLog.Config.InstallationContext())
                    {
                        config.Install(context);
                    }

                    this.NLogger = NLog.LogManager.GetCurrentClassLogger();
                    this.NLogger.Factory.Configuration = config;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                finally
                { }
            }

            this.IsInitialized = true;
        }

        protected LogEventInfo CreateLogEventInfo(LogEntry entry)
        {
            var einfo = new LogEventInfo();

            einfo.Exception = entry.Exception;
            einfo.Message = entry.Message;

            switch (entry.EntryType)
            {
                case LogEntryType.Debug:
                    einfo.Level = LogLevel.Debug;
                    break;
                case LogEntryType.Error:
                    einfo.Level = LogLevel.Error;
                    break;
                case LogEntryType.FatalError:
                    einfo.Level = LogLevel.Fatal;
                    break;
                case LogEntryType.Info:
                    einfo.Level = LogLevel.Info;
                    break;
                case LogEntryType.Warning:
                    einfo.Level = LogLevel.Warn;
                    break;
                default:
                    throw new Exception();
            }

            einfo.Properties["DateTimeUTC"] = entry.DateTimeUTC;
            einfo.Properties["UserID"] = entry.UserID;
            einfo.Properties["Browser"] = (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Browser != null) ? string.Format("{0} {1}", HttpContext.Current.Request.Browser.Browser, HttpContext.Current.Request.Browser.Version) : null;
            einfo.Properties["Application"] = entry.Application;
            einfo.Properties["ApplicationVersion"] = entry.ApplicationVersion;

            foreach (string key in entry.Properties.Keys)
                einfo.Properties[key] = entry.Properties[key];

            return einfo;
        }
    }
}
