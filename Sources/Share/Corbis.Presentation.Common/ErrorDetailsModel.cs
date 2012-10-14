using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Presentation.Common
{
    /// <summary>
    /// Error details. It is model which is used by ErrorController
    /// </summary>
    [Serializable]
    public class ErrorDetailsModel
    {
        /// <summary>
        /// Error message to display
        /// </summary>
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Error message. Contains error description
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Occured exception
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Current loggedin user identifier
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Is fatal error or not
        /// </summary>
        public bool IsFatal { get; set; }

        /// <summary>
        /// Is error added in log storage or not
        /// </summary>
        public bool IsLogged { get; set; }

        /// <summary>
        /// Contains additional attributes and properties for error processing displaying
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get { return this.Properties; }
        }
        private readonly Dictionary<string, object> m_Properties = new Dictionary<string, object>();
    }
}
