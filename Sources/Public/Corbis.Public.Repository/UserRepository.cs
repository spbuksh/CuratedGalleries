using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Web.Helpers;
using System.Net.Http;

using Corbis.Public.Entity;
using Corbis.Common;
using Corbis.Public.Repository.Interface;
using Corbis.Common.WebApi;
using System.Dynamic;

namespace Corbis.Public.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        /// <summary>
        /// Authenicates the user
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">user password</param>
        /// <param name="scope">application scope</param>
        /// <returns></returns>
        public virtual OperationResult<UserAuthResults, CorbisUser> Authenticate(string userName, string password, Scope scope = Scope.All)
        {
            #region OAuth autorization

            var scopePmtr = scope.GetItems().ToString("+").ToLower();

            var uri = string.Format("{0}/oauth/access_token?client_id={1}&client_secret={2}&grant_type=password&username={3}&password={4}&scope={5}"
                , WebApiClient.Instance.AuthUrl.TrimEnd(Path.AltDirectorySeparatorChar)
                , WebApiClient.Instance.ClientID.GetUrlEncoded()
                , WebApiClient.Instance.ClientSecret.GetUrlEncoded()
                , userName.GetUrlEncoded()
                , password.GetUrlEncoded()
                , scopePmtr.GetUrlEncoded());


            var token = CorbisWebApiHelper.ExecHttpRequest<dynamic>(uri);

            var output = new OperationResult<UserAuthResults, CorbisUser>();

            if (token == null)
            {
                output.Result = UserAuthResults.Failure;
                return output;
            }
            else if (!token.IsSuccess)
            {
                if (token.Error != null)
                {

                    switch (token.Error.ErrorType)
                    {
                        case "invalid_client":
                            output.Result = UserAuthResults.AuthFailure;
                            break;
                        default:
#if DEBUG
                            throw new NotImplementedException();
#else
                            output.Result = UserAuthResultType.Failed;
#endif
                    }
                }
                else
                {
#if DEBUG
                    //error must be required
                    throw new NotImplementedException();
#else
                    output.Result = UserAuthResultType.Failed;
#endif
                }

                return output;
            }

            output.Result = UserAuthResults.Success;
            output.Output = new CorbisUser();

            output.Output.Token = new UserToken() { Scope = scope };
            output.Output.Token.AccessToken = new Guid(token.Content["access_token"]);
            output.Output.Token.RefreshToken = new Guid(token.Content["refresh_token"]);
            output.Output.Token.TokenType = token.Content["token_type"];
            output.Output.Token.ExpirationDate = DateTime.UtcNow.AddSeconds(token.Content["expires_in"]);

            #endregion OAuth autorization

            #region Get User Info

            //TODO: API does not provide user name and password. For that reason I hardcoded them. Delete it after point clarifying
            output.Output.FirstName = "John";
            output.Output.LastName = "Doe";
            output.Output.FullName = string.Format("{0} {1}", output.Output.FirstName, output.Output.LastName);

            #endregion Get User Info

            return output;
        }

        /// <summary>
        /// Refreshes user token
        /// </summary>
        /// <param name="exparedToken">expired user token</param>
        /// <returns>New user token</returns>
        public virtual OperationResult<UserTokenRefreshResults, UserToken> RefreshToken(UserToken exparedToken)
        {
            var uri = string.Format("{0}/oauth/refresh_token?client_id={1}&client_secret={2} &grant_type=refresh&refresh_token={3}"
                , WebApiClient.Instance.AuthUrl.TrimEnd(Path.AltDirectorySeparatorChar)
                , WebApiClient.Instance.ClientID.GetUrlEncoded()
                , WebApiClient.Instance.ClientSecret.GetUrlEncoded()
                , exparedToken.RefreshToken);

            var rslt = CorbisWebApiHelper.ExecHttpRequest<dynamic>(uri);

            if (!rslt.IsSuccess)
            {
                switch(rslt.Error.ErrorType.ToLower())
                {
                    case "unauthorized_client":
                        return new OperationResult<UserTokenRefreshResults, UserToken>() { Result = UserTokenRefreshResults.InvalidRefreshToken, Output = null };
                    default:
                        throw new NotImplementedException();
                }
            }

            var token = Corbis.Common.Utils.Clone<UserToken>(exparedToken);
            token.AccessToken = new Guid(rslt.Content["access_token"]);
            token.RefreshToken = new Guid(rslt.Content["refresh_token"]);
            token.TokenType = rslt.Content["token_type"];
            token.ExpirationDate = DateTime.UtcNow.AddSeconds(rslt.Content["expires_in"]);

            return new OperationResult<UserTokenRefreshResults, UserToken>() { Result = UserTokenRefreshResults.Succeess, Output = token };
        }
    }
}
