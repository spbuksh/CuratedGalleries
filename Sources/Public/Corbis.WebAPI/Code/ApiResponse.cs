using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Generic response for all action
    /// </summary>
    /// <typeparam name="Tenum">Type of enum which indicates action result(result code). it is possible to use numeric types</typeparam>
    /// <typeparam name="Tdata">Type of output data</typeparam>
    public class ApiResponse<Tenum, Tdata>
    {
        /// <summary>
        /// Action result
        /// </summary>
        public Tenum Result { get; set; }
        /// <summary>
        /// Operation result data
        /// </summary>
        public Tdata Output { get; set; }
        /// <summary>
        /// List of errors
        /// </summary>
        public List<string> Errors { get; set; }
    }

    /// <summary>
    /// Generic response for all action
    /// </summary>
    /// <typeparam name="Tdata"></typeparam>
    public class ApiResponse<Tdata> : ApiResponse<int, Tdata>
    { }

}