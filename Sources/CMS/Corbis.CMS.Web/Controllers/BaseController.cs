using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Entity;
using Corbis.Public.Entity;
using Microsoft.Practices.Unity;
using Corbis.Public.Repository.Interface;
using Corbis.Presentation.Common;
using System.Web.Security;
using Corbis.Common;
using System.Threading;
using System.Globalization;

namespace Corbis.CMS.Web.Controllers
{
    public class CMSControllerBase : BaseController
    {
        public AdminUserInfo CurrentUser
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected override void OnAuthorization(AuthorizationContext context)
        {
            base.OnAuthorization(context);

            if (context.HttpContext.Request.IsAuthenticated)
            {
                //TODO
                bool isAuth = false;

                if (isAuth)
                {
                    throw new Exception();
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

            //TODO: temporary we identify user based on his fullname due to currently we do not have user identifier

            return model;
        }
    }
}
