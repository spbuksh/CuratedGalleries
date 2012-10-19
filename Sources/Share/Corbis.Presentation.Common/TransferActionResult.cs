using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Corbis.Common;
using System.Web.Routing;
using System.Web;

namespace Corbis.Presentation.Common
{
    public class TransferActionResult : ActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        protected ActionHandler<ControllerContext, string> UrlBuilder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool PreserveForm { get; private set; }

        /// <summary>
        /// base hidden ctor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="preserveForm"></param>
        protected TransferActionResult(bool preserveForm = true)
        {
            this.PreserveForm = preserveForm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Target url string</param>
        /// <param name="preserveForm"></param>
        public TransferActionResult(string url, bool preserveForm = false)
            : this(preserveForm)
        {
            this.UrlBuilder = delegate(ControllerContext context) { return url; };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Target url string</param>
        /// <param name="preserveForm"></param>
        public TransferActionResult(Uri url, bool preserveForm = true)
            : this(url.ToString(), preserveForm)
        { }

        /// <summary>
        /// Sample: new TransferActionResult(new {controller = "home", action = "something" });
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="preserveForm"></param>
        public TransferActionResult(object routeValues, bool preserveForm = true)
            : this(null, routeValues, preserveForm)
        { }
        public TransferActionResult(string routeName, object routeValues, bool preserveForm = true)
            : this(preserveForm)
        {
            this.UrlBuilder =
                delegate(ControllerContext context)
                {
                    var helper = new UrlHelper(context.RequestContext, System.Web.Routing.RouteTable.Routes);
                    return string.IsNullOrEmpty(routeName) ? helper.RouteUrl(routeValues) : helper.RouteUrl(routeName, routeValues);
                };
        }


        public TransferActionResult(RouteValueDictionary routeValues, bool preserveForm = true)
            : this(null, routeValues, preserveForm)
        { }
        public TransferActionResult(string routeName, RouteValueDictionary routeValues, bool preserveForm = true)
            : this(preserveForm)
        {
            this.UrlBuilder =
                delegate(ControllerContext context)
                {
                    var helper = new UrlHelper(context.RequestContext, System.Web.Routing.RouteTable.Routes);
                    return string.IsNullOrEmpty(routeName) ? helper.RouteUrl(routeValues) : helper.RouteUrl(routeName, routeValues);
                };
        }


        public override void ExecuteResult(ControllerContext context)
        {
            string url = this.UrlBuilder(context);

            //
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                context.HttpContext.RewritePath(url, this.PreserveForm);

                //TransferRequest is used for IIS7+
                context.HttpContext.Server.TransferRequest(url, this.PreserveForm);
            }
            else
            {
                //IT DOES NOT WORK
                var cntx = HttpContext.Current;

                cntx.RewritePath(url, this.PreserveForm);
                cntx.Server.Transfer(url);
            }
        }
    }
}
