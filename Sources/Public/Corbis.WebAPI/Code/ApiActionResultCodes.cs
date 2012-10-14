using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Contains base api action result codes
    /// </summary>
    public class ApiActionResultCodes
    {
        /// <summary>
        /// Action is succeed
        /// </summary>
        public const int Success = 200;
        /// <summary>
        /// Action is failed due to internal server error
        /// </summary>
        public const int Failure = 500;
        /// <summary>
        /// Validation error
        /// </summary>
        public const int ValidationFailure = 400;
        /// <summary>
        /// User is unathorized. It means that auth data is missed or is incorrect
        /// </summary>
        public const int Unauthorized = 401;
        /// <summary>
        /// Requested data was not found
        /// </summary>
        public const int NotFound = 404;
    }
}