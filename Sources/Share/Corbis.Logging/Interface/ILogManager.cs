using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Logging.Interface
{
    public interface ILogManager : ILogger
    {
        void WriteError(Exception ex, string message = null, bool isFatal = false, string userID = null);
        void WriteError(string errorMessage, bool isFatal = false, string userID = null);
        void WriteWarning(string warn, string userID = null);
        void WriteDebug(string info, string userID = null);
        void WriteInfo(string info, string userID = null);
    }
}
