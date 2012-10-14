using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using System.Security.Principal;
using Corbis.Public.Entity;
using System.Globalization;
using Corbis.Public.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.Common;

namespace Corbis.Public.Web.Code
{
    /// <summary>
    /// Base controller for all controllers of the public site
    /// </summary>
    public abstract class PublicControllerBase : Corbis.Presentation.Common.BaseController
    {
        [Dependency]
        public IUserRepository UserRepository { get; set; }

        /// <summary>
        /// Current user culture
        /// </summary>
        protected virtual CultureInfo UserCulture
        {
            get 
            {
                if(this.m_Culture == null)
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
            Thread.CurrentThread.CurrentUICulture = this.UserCulture;

            base.ExecuteCore();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected override void OnAuthorization(AuthorizationContext context)
        {
            base.OnAuthorization(context);

            if (context.HttpContext.Request.IsAuthenticated)
            {
                bool isAuth = false;  //TODO: check auth

                if (!isAuth)
                {
                    //try to auth
                    isAuth = true;

                    //if auth is failed then go to login page
                    if (isAuth)
                    {
                        FormsAuthentication.SignOut();
                        context.Result = this.RedirectToLoginPage(); 
                    }
                }
                else
                {
                    //Auth cookie is expired when session is expired: in the Web.config in <authentication>.<forms> node pointed:
                    //  1. slidingExpiration = true
                    //  2. timeout = 20 minutes (session timeout)
                    //So if auth cookie exests but user data are not in the session then it is problem and we must exec auth action
                    FormsAuthentication.SignOut();
                    context.Result = this.RedirectToLoginPage();
                }
            }
        }

        /// <summary>
        /// Builds error description model for displaying
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Presentation.Common.ErrorDetailsModel BuildErrorDetailsModel(ExceptionContext context)
        {
            var model = base.BuildErrorDetailsModel(context);

            //TODO: customize error model creation

            return model;
        }
    }
}