using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Logging.Interface
{
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Writes log entry
        /// </summary>
        /// <param name="entry"></param>
        void Write(LogEntry entry);
    }


}
