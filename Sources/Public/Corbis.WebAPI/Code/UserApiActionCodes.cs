using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Contains action result codes for user web api
    /// </summary>
    public class UserApiActionCodes : ApiActionResultCodes
    {
        /// <summary>
        /// Not valid password
        /// </summary>
        public const int InvalidPassword = 1000;

        /// <summary>
        /// Authentication is failed. It means some user data is incorect
        /// </summary>
        public const int AuthFailure = 1001;

        /// <summary>
        /// Invalid refresh token
        /// </summary>
        public const int InvalidRefreshToken = 1002;

    }
}