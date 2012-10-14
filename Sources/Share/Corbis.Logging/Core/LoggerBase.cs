using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;

namespace Corbis.Logging.Core
{
    public abstract class LoggerBase : ILogger
    {
        public abstract void Write(LogEntry entry);

        public virtual void Dispose()
        {
            //By default do nothing
        }
    }
}
