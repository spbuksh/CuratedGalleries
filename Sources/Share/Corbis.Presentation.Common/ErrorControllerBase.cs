using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics;

namespace Corbis.Presentation.Common
{
    public abstract class ErrorControllerBase : BaseController
    {
        /// <summary>
        /// Last error session key. Error details must be stored in the session
        /// </summary>
        public static string ErrorSessionKey
        {
            get { return "Error_{CAC27966-AD6F-4FD1-ACCF-9D1081387FCA}"; }
        }

        /// <summary>
        /// Default static error page
        /// </summary>
        protected virtual string DefaultErrorPageUrl
        {
            get { return "/Errors/default.html"; }
        }

        /// <summary>
        /// Contains logic of last error displaying
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ErrorDetails()
        {
            //In the session we store last user error and do not delete it
            ErrorDetailsModel model = this.Session[ErrorSessionKey] as ErrorDetailsModel;

            if(model == null)
            {
                model = new ErrorDetailsModel();
                model.DisplayMessage = "Last error details were not found";
            }
            else
            {
                if (!model.IsLogged)
                {
                    this.Logger.WriteError(model.Exception, model.Message, false, model.UserID);
                    model.IsLogged = true;
                }
            }

            return this.View("ErrorDetails", model);
        }

       /// <summary>
       /// Logging errors from web site client code
       /// </summary>
       /// <param name="browser">browser info</param>
       /// <param name="page">file name</param>
       /// <param name="message">error message</param>
       /// <param name="user">user identifier</param>
       /// <returns></returns>
        public virtual ActionResult LogClientError(string browser, string page, string message, string user)
        {
            //In the session we store last user error and do not delete it
            this.Logger.WriteError(new Exception(string.Format("user: {0} ;filename: {1}; browser: {2}", user, page, browser)), message);

            return this.Json("Logged");
        }


        protected override void OnException(System.Web.Mvc.ExceptionContext context)
        {
            try
            {
                this.Logger.WriteError(context.Exception, "Logger error");
                context.ExceptionHandled = true;
            }
            catch (Exception ex)
            {                
                Debug.WriteLine(ex);

                //if we cannot process error
                if (string.IsNullOrEmpty(this.DefaultErrorPageUrl))
                    throw;

                this.Redirect(this.DefaultErrorPageUrl);
            }

            base.OnException(context);
        }
    }
}
