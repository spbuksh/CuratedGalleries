using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Logging.Interface;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Base class for all filter attributes
    /// </summary>
    public abstract class CorbisActionFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public CorbisActionFilterAttribute() : base()
        { }

        /// <summary>
        /// Corbis System logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Corbis.Logging.LogManagerProvider.Instance; }
        }
    }
}