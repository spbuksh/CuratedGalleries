using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Corbis.Logging.Interface;

namespace Corbis.CMS.Web.Controllers.Api
{
    /// <summary>
    /// Base controller for web API
    /// </summary>
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Logging.LogManagerProvider.Instance; }
        }

        /// <summary>
        /// Http context of the user request
        /// </summary>
        protected HttpContextWrapper HttpContext
        {
            get
            {
                if (this.m_HttpContext == null)
                {
                    if (this.Request.Properties.ContainsKey("MS_HttpContext"))
                        this.m_HttpContext = this.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
                }
                return this.m_HttpContext;
            }
        }
        private HttpContextWrapper m_HttpContext = null;

    }
}