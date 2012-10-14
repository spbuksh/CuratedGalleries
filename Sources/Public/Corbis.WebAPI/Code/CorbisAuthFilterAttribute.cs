using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using Corbis.WebAPI.Controllers.API;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;


namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Corbis authentication attribute
    /// </summary>
    public class CorbisAuthFilterAttribute : CorbisActionFilterAttribute
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public CorbisAuthFilterAttribute() : base()
        { }

        #region Fields

        private const string AccessTokenKey = "access_token";

        #endregion

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="context">The action context.</param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext context)
        {
            base.OnActionExecuting(context);

            if (context.ControllerContext.ControllerDescriptor.GetCustomAttributes<CorbisAuthIgnoreFilterAttribute>().Any())
                return;

            if (context.ActionDescriptor.GetCustomAttributes<CorbisAuthIgnoreFilterAttribute>().Any())
                return;

            Nullable<Guid> token = null;

            try
            {
                if (context.Request.Headers.Contains(AccessTokenKey))
                {
                    token = new Guid(context.Request.Headers.GetValues(AccessTokenKey).First());
                }
                else if (context.Request.Properties.ContainsKey(AccessTokenKey))
                {
                    token = new Guid(context.Request.Properties[AccessTokenKey].ToString());
                }
            }
            catch (FormatException ex)
            {
                this.Logger.WriteError(ex, "Error during parsing access token into System.Guid type");

#if DEBUG
                throw;
#else
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
#endif
            }

            if (!token.HasValue)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            var principal = new CorbisClientPrincipal(new GenericIdentity(token.Value.ToString()), null, token);
            Thread.CurrentPrincipal = principal;            
        }
    }
}