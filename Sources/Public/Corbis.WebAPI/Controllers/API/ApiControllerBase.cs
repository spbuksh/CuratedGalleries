using System;
using System.Security.Principal;
using System.Web.Http;
using Corbis.Logging.Interface;
using System.Threading;
using Corbis.WebAPI.Code;
using System.Globalization;
using System.Web;

namespace Corbis.WebAPI.Controllers.API
{
    /// <summary>
    /// Base controller for all API controllers
    /// </summary>
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        /// Application event logger
        /// </summary>
        protected ILogManager Logger
        {
            get
            {
                return Logging.LogManagerProvider.Instance;
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        protected Guid? AccessToken
        {
            get
            {
                var corbisClientPrincipal = User as CorbisClientPrincipal;
                return corbisClientPrincipal != null ? corbisClientPrincipal.AccessToken : null;
            }
        }
        
        /// <summary>
        /// Http context of the user request
        /// </summary>
        protected HttpContext HttpContext
        {
            get 
            {
                if (this.m_HttpContext == null)
                {
                    if (this.Request.Properties.ContainsKey("MS_HttpContext"))
                        this.m_HttpContext = this.Request.Properties["MS_HttpContext"] as HttpContext;
                }
                return this.m_HttpContext;
            }
        }
        private HttpContext m_HttpContext = null;
        

        /// <summary>
        /// Current using culture
        /// </summary>
        protected virtual CultureInfo Culture
        {
            get { return CultureInfo.InvariantCulture; }
        }

        /// <summary>
        /// Customization of the init process
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Thread.CurrentThread.CurrentCulture = this.Culture;
        }

    }
}