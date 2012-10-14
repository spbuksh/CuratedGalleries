using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Text;

using Corbis.Public.Repository.Interface;
using Corbis.WebAPI.Code;
using Corbis.WebAPI.Models;
using Corbis.Public.Entity;
using Corbis.Common;

namespace Corbis.WebAPI.Controllers.API
{
    /// <summary>
    /// Contains method to work with user
    /// </summary>
    public class UserController : ApiControllerBase
    {
        #region Repositories

        /// <summary>
        /// User repository. Main data provider to work with users
        /// </summary>
        [Dependency]
        public IUserRepository UserRepository { get; set; }

        #endregion Repositories

        /// <summary>
        /// Authintecates user
        /// </summary>
        [HttpPost]
        [CorbisAuthIgnoreFilter]
        public HttpResponseMessage Authenticate([FromBody]AuthModel model)
        {
            var rdata = new ApiResponse<UserTokenModel>();

            #region Validation

            if(!this.ModelState.IsValid)
            {
                rdata.Result = UserApiActionCodes.ValidationFailure;
                rdata.Errors = this.ModelState.GetErrorList();
                return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
            }

            #endregion Validation

            //check client identifier and secret
            // TODO: I skipped this step

            OperationResult<UserAuthResults, CorbisUser> authResult = null;

            try
            {
                authResult = this.UserRepository.Authenticate(model.UserName, model.Password, model.Scope.HasValue ? model.Scope.Value : Public.Entity.Scope.All);
            }
            catch (Exception ex)
            {
                string emsg = string.Format("Authentication failure for user '{0}'. Server error.", model.UserName);
                this.Logger.WriteError(ex, emsg);

                rdata.Result = UserApiActionCodes.Failure;
                rdata.Errors = new List<string>() { emsg };
                return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
            }

            switch(authResult.Result)
            {
                case UserAuthResults.Failure:
                    rdata.Result = UserApiActionCodes.Failure;
                    return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.InternalServerError, rdata);
                case UserAuthResults.NotFound:
                case UserAuthResults.InvalidPassword:
                case UserAuthResults.AuthFailure:
                    rdata.Result = UserApiActionCodes.AuthFailure;
                    return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
                case UserAuthResults.Success:
                    rdata.Result = UserApiActionCodes.Success;
                    rdata.Output = new UserTokenModel() { AccessToken = authResult.Output.Token.AccessToken, RefreshToken = authResult.Output.Token.RefreshToken, ExpirationDate = authResult.Output.Token.ExpirationDate.Ticks };
                    return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.OK, rdata);
                default:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// Refreshes already expired token
        /// </summary>
        /// <param name="model">obsoled and expired token</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RefreshToken([FromBody]UserTokenRequestModel model)
        {
            var rdata = new ApiResponse<UserTokenModel>();

            #region Validation

            if (!this.ModelState.IsValid)
            {
                rdata.Result = UserApiActionCodes.ValidationFailure;
                rdata.Errors = this.ModelState.GetErrorList();
                return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
            }

            #endregion Validation

            OperationResult<UserTokenRefreshResults, UserToken> rslt = null;

            try
            {
                rslt = this.UserRepository.RefreshToken(new UserToken() { AccessToken = model.AccessToken, RefreshToken = model.RefreshToken.Value, Scope = model.Scope.HasValue ? model.Scope.Value : Public.Entity.Scope.All });
            }
            catch (Exception ex)
            {
                string emsg = string.Format("Error during token refresh. RefreshToken: '{0}'", model.RefreshToken.Value);
                this.Logger.WriteError(ex, emsg);

                rdata.Result = UserApiActionCodes.Failure;
                rdata.Errors = new List<string>() { emsg };
                return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
            }

            switch(rslt.Result)
            {
                case UserTokenRefreshResults.InvalidRefreshToken:
                    rdata.Result = UserApiActionCodes.InvalidRefreshToken;
                    return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.BadRequest, rdata);
                case UserTokenRefreshResults.Succeess:
                    { 
                        rdata.Result = UserApiActionCodes.Success;
                        rdata.Output = new UserTokenModel() { AccessToken = rslt.Output.AccessToken, RefreshToken = rslt.Output.RefreshToken, ExpirationDate = rslt.Output.ExpirationDate.Ticks, Scope = rslt.Output.Scope };
                        return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.OK, rdata);
                    }
                case UserTokenRefreshResults.Failure:
                    {
                        rdata.Result = UserApiActionCodes.Failure;
                        return this.Request.CreateResponse<ApiResponse<UserTokenModel>>(HttpStatusCode.InternalServerError, rdata);
                    }
                default:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }
    }
}
