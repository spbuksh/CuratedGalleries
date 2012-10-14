using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Logging.Interface
{
    /// <summary>
    /// LogEntry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Log entry type
        /// </summary>
        public LogEntryType EntryType { get; set; }

        /// <summary>
        /// Exception object
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Entry Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Version of the application
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Application user identifier
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Date and time in UTC
        /// </summary>
        public DateTime DateTimeUTC { get; set; }

        /// <summary>
        /// Additional log entry properties
        /// </summary>
        public Dictionary<string, object> Properties 
        {
            get { return this.m_Properties; }
        }
        private readonly Dictionary<string, object> m_Properties = new Dictionary<string, object>();

    }

    public enum LogEntryType
    {
        FatalError,
        Error,
        Warning,
        Debug,
        Info
    }
}
