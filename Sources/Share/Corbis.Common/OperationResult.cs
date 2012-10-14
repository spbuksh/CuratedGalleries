using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common
{
    /// <summary>
    /// Contains base cases of operation results
    /// </summary>
    public enum OperationResults
    {
        /// <summary>
        /// Operation is succeed
        /// </summary>
        Success,
        /// <summary>
        /// Access token is not set or it is invalid
        /// </summary>
        Unathorized,
        /// <summary>
        /// Operation is failed
        /// </summary>
        Failure
    }

    /// <summary>
    /// Operation or action result
    /// </summary>
    /// <typeparam name="Tenum">Operation result type or code. It means that based on codes we specify nornal or not normal behavior which we must process</typeparam>
    /// <typeparam name="Tdata"></typeparam>
    public class OperationResult<Tenum, Tdata> where Tenum : struct
    {
        /// <summary>
        /// Operation result
        /// </summary>
        public Tenum Result { get; set; }

        /// <summary>
        /// Operation output
        /// </summary>
        public Tdata Output { get; set; }
    }
}
