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

    }
}