using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;
using Corbis.Logging.Core;

namespace Corbis.Logging
{
    public class LogManagerProvider
    {
        /// <summary>
        /// Initializes log manager via configuration section
        /// </summary>
        /// <param name="sectionName">Log configuration section name</param>
        public static void Initialize(string sectionName = null)
        {
            LoggingSectionName = sectionName;
        }
        /// <summary>
        /// Logging section name
        /// </summary>
        protected static string LoggingSectionName { get; set; }

        /// <summary>
        /// Log manager instance
        /// </summary>
        public static ILogManager Instance
        {
            get 
            {
                if (object.ReferenceEquals(m_Instance, null))
                {
                    lock (typeof(LogManagerProvider))
                    {
                        if (object.ReferenceEquals(m_Instance, null))
                        {
                            var logmngr  = new LogManager();
                            logmngr.Initialize(LoggingSectionName);
                            m_Instance = logmngr;
                        }
                    }
                }
                return m_Instance;
            }
        }
        private static ILogManager m_Instance = null;
    }
}
