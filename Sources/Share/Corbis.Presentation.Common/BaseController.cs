using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Corbis.Logging.Interface;
using Corbis.Public.Entity;
using Corbis.Common;
using System.Threading;
using Corbis.Common.ObjectMapping.Mappers;

namespace Corbis.Presentation.Common
{
    /// <summary>
    /// Application base controller
    /// </summary>
    //[HandleError]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Logging.LogManagerProvider.Instance; }
        }

        /// <summary>
        /// Object mapper. It helps to transfer data from one object type to another
        /// </summary>
        protected virtual MapperBase ObjectMapper
        {
            get { return Corbis.Common.ObjectMapping.Mappers.ObjectMapper.Instance; }
        }

        /// <summary>
        /// Redirects to the Home page.
        /// </summary>
        /// <returns>The Home page view.</returns>
        protected virtual ActionResult RedirectToHomePage()
        {
            return this.RedirectToPage("Index", "Home");
        }

        /// <summary>
        /// Redirects to the Login page.
        /// </summary>
        /// <returns>The Login page view.</returns>
        protected virtual ActionResult RedirectToLoginPage()
        {
            return this.RedirectToPage("Login", "Account");
        }

        /// <summary>
        /// Redirects to the Home page.
        /// </summary>
        /// <returns>The Home page view.</returns>
        protected virtual ActionResult RedirectToErrorPage(ErrorDetailsModel model = null)
        {
            if (model != null)
            {
                if (!model.IsLogged)
                {
                    this.Logger.WriteError(model.Exception, model.Message, model.IsFatal, model.UserID);
                    model.IsLogged = true;
                }

                this.Session[ErrorControllerBase.ErrorSessionKey] = model;
            }

            return this.RedirectToPage("ErrorDetails", "Error");
        }

        protected override void OnException(ExceptionContext context)
        {            
            if (!context.ExceptionHandled)
            {
                var model = this.BuildErrorDetailsModel(context);
                context.Result = this.RedirectToErrorPage(model);
                context.ExceptionHandled = true;
            }

            base.OnException(context);
        }

        /// <summary>
        /// Builds error details model for error processing and displaying
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual ErrorDetailsModel BuildErrorDetailsModel(ExceptionContext context)
        {
            return new ErrorDetailsModel()
            {
                DisplayMessage = "Unexpected server error",
                Exception = context.Exception,
                Message = "Unexpected server error",
                IsFatal = true,
                UserID = null,
                IsLogged = false
            };
        }

        /// <summary>
        /// Redirects to the page despite the type of request (Ajax or not).
        /// </summary>
        /// <param name="action">The action method name.</param>
        /// <param name="controller">The controller name.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The action result: either RedirectToActionResult or JavaScriptResult in case of the Ajax request.</returns>
        protected virtual ActionResult RedirectToPage(string action, string controller, object routeValues)
        {
            return !this.Request.IsAjaxRequest()
                ? (ActionResult)this.RedirectToAction(action, controller, routeValues)
                : (ActionResult)this.JavaScript(string.Format("window.location = '{0}';", this.Url.Action(action, controller, routeValues)));
        }

        /// <summary>
        /// Redirects to the page despite the type of the request (Ajax or not).
        /// </summary>
        /// <param name="action">The action method name.</param>
        /// <param name="controller">The controller name.</param>
        /// <returns>The action result: either RedirectToActionResult or JavaScriptResult in case of the Ajax request.</returns>
        protected virtual ActionResult RedirectToPage(string action, string controller)
        {
            return this.RedirectToPage(action, controller, null);
        }

    }
}
