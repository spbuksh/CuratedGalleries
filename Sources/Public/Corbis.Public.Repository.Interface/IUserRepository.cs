using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Public.Entity;
using Corbis.Common;

namespace Corbis.Public.Repository.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Authenticates
        /// </summary>
        /// <param name="userName">user name (login)</param>
        /// <param name="password">user password</param>
        OperationResult<UserAuthResults, CorbisUser> Authenticate(string userName, string password, Scope scope = Scope.All);

        /// <summary>
        /// Refreshes already expired token
        /// </summary>
        /// <param name="expiredToken">obsoled and expired token</param>
        /// <returns></returns>
        OperationResult<UserTokenRefreshResults, UserToken> RefreshToken(UserToken expiredToken);
    }
}
