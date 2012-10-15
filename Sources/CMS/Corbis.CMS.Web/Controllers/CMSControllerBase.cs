using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using Microsoft.Practices.Unity;
using System.Web.Security;
using System.Threading;
using System.Globalization;

using Corbis.Common;
using Corbis.Presentation.Common;
using Corbis.Public.Repository.Interface;
using Corbis.Public.Entity;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Entity;
using Corbis.CMS.Web.Code;
using System.Security.Principal;

namespace Corbis.CMS.Web.Controllers
{
    /// <summary>
    /// Base controller for all Web CMS controllers. All web controller must be inherited from it
    /// </summary>
    public class CMSControllerBase : BaseController
    {
        /// <summary>
        /// Current logged on admin user
        /// </summary>
        protected AdminUserInfo CurrentUser
        {
            get 
            {
                if(this.m_CurrentUser == null)
                    this.m_CurrentUser = (AdminUserInfo)this.Session["CurrentUser_{50E9BEBB-3352-4259-9487-4B96F9D3E549}"];

                return this.m_CurrentUser;
            }
            set 
            {
                this.m_CurrentUser = value;
                this.Session["CurrentUser_{50E9BEBB-3352-4259-9487-4B96F9D3E549}"] = this.m_CurrentUser; 
            }
        }
        private AdminUserInfo m_CurrentUser = null;

        /// <summary>
        /// Current user culture
        /// </summary>
        protected virtual CultureInfo UserCulture
        {
            get
            {
                if (this.m_Culture == null)
                {
                    var cookie = this.HttpContext.Request.Cookies["Culture"];

                    if (cookie != null)
                    {
                        int cultureID;

                        if (int.TryParse(cookie.Value, out cultureID))
                        {
                            this.m_Culture = CultureInfo.GetCultureInfo(cultureID);
                        }
                        else
                        {
                            //set default culture
                            this.m_Culture = CultureInfo.InvariantCulture;
                            this.HttpContext.Response.Cookies["Culture"].Value = this.m_Culture.LCID.ToString();
                        }
                    }
                    else
                    {
                        //set default culture
                        this.m_Culture = CultureInfo.InvariantCulture;
                        this.HttpContext.Response.Cookies["Culture"].Value = this.m_Culture.LCID.ToString();
                    }
                }

                return this.m_Culture;
            }
        }
        private CultureInfo m_Culture = null;

        /// <summary>
        /// 
        /// </summary>
        protected override void ExecuteCore()
        {
            //If we have multiple culture
            Thread.CurrentThread.CurrentCulture = this.UserCulture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.ExecuteCore();
        }

        /// <summary>
        /// Authorization and authentication logic
        /// </summary>
        /// <param name="context"></param>
        protected override void OnAuthorization(AuthorizationContext context)
        {
            base.OnAuthorization(context);

            if (context.HttpContext.Request.IsAuthenticated)
            {
                //'Remember Me' case
                if (this.CurrentUser == null)
                {
                    //***** !!! DO NOT REMOVE this commented code block !!! *****
                    //Auth cookie is expired when session is expired: in the Web.config in <authentication>.<forms> node pointed:
                    //  1. slidingExpiration = true
                    //  2. timeout = 20 minutes (session timeout)
                    //So if auth cookie exests but user data are not in the session then it is problem and we must exec auth action
                    //FormsAuthentication.SignOut();
                    //context.Result = this.RedirectToLoginPage();
                    //***********************************************************

                    //try to restore user from cookie
                    AdminUserInfo restored = null;

                    try
                    {
                        restored = this.RestoreUserFromCookie();
                    }
                    catch(Exception ex)
                    {
                        this.Logger.WriteWarning(string.Format("Error during user data restoring from cookies: {0}{1}", Environment.NewLine, ex));
                    }

                    if (restored == null)
                    { 
                        FormsAuthentication.SignOut();
                        context.Result = this.RedirectToLoginPage();
                        return;
                    }

                    //User data was restored successfully. We must get actual data and compare with restored
                    //TODO: send request to get actual user info. Currently i quess that actual is equal to restored
                    AdminUserInfo actual = restored; 
                    
                    //if user will be deleted/deactivated or has updated roles then exec login action
                    if (actual == null || actual.Roles != restored.Roles)
                    {
                        FormsAuthentication.SignOut();
                        context.Result = this.RedirectToLoginPage();
                        return;
                    }

                    //update auth info in cookies
                    this.CurrentUser = restored;
                    this.SetAuthData(restored);
                }
                else
                {
                    //TODO: update user info by schedule (for example every 5 minutes)
                }
            }
            else
            {
                //case when user deletes cookies but user data in session are actual
                if (this.CurrentUser != null)
                    this.SetAuthData(this.CurrentUser);
            }
        }

        /// <summary>
        /// Set auth data based on user data
        /// </summary>
        /// <param name="user"></param>
        protected virtual void SetAuthData(AdminUserInfo user)
        {
            var roles = user.Roles.HasValue ? user.Roles.Value.GetRoleNames() : new string[] { };

            IPrincipal principal = new AdminUserPrincipal(new GenericIdentity(user.ID.ToString()), user, roles);

            this.HttpContext.User = principal;
            System.Threading.Thread.CurrentPrincipal = principal;

            //in the configuration file slidingExpiration is set as TRUE and timeout is set as sessions timeout => auth ticket is valid only for session period
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.ID.ToString(),
                DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), false, System.Web.Helpers.Json.Encode(user));

            this.HttpContext.Response.Cookies.Set(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }
        /// <summary>
        /// Restore user from the auth cookie data
        /// </summary>
        /// <returns></returns>
        protected virtual AdminUserInfo RestoreUserFromCookie()
        {
            return System.Web.Helpers.Json.Decode<AdminUserInfo>((this.HttpContext.User.Identity as FormsIdentity).Ticket.UserData);
        }

        /// <summary>
        /// Builds error description model for displaying
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Presentation.Common.ErrorDetailsModel BuildErrorDetailsModel(ExceptionContext context)
        {
            var model = base.BuildErrorDetailsModel(context);

            if (this.CurrentUser != null)
                model.UserID = this.CurrentUser.ID.ToString();

            return model;
        }
    }
}
