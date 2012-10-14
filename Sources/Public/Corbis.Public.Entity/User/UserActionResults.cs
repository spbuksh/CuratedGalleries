using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Public.Entity
{
    public enum UserAuthResults
    {
        /// <summary>
        /// User was not found or user password is not valid
        /// </summary>
        AuthFailure,
        /// <summary>
        /// User was not found
        /// </summary>
        NotFound,
        /// <summary>
        /// Not valid password
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// User was authenticated successfully
        /// </summary>
        Success,
        /// <summary>
        /// Server error
        /// </summary>
        Failure
    }

    public enum UserTokenRefreshResults
    {
        /// <summary>
        /// User was authenticated successfully
        /// </summary>
        Succeess,
        /// <summary>
        /// Invalid refresh token
        /// </summary>
        InvalidRefreshToken,
        /// <summary>
        /// Server error
        /// </summary>
        Failure

    }
}
