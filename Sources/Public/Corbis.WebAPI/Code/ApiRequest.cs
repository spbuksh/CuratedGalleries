using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Ttoken">Type of user token</typeparam>
    /// <typeparam name="Tinput">Input data type</typeparam>
    public class ApiRequest<Ttoken, Tinput>
    {
        /// <summary>
        /// Access token
        /// </summary>
        public Ttoken Token { get; set; }

        /// <summary>
        /// Input user data for request processing
        /// </summary>
        public Tinput Input { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Tinput">Input data type</typeparam>
    public class ApiRequest<Tinput> : ApiRequest<Guid, Tinput>
    { }

}