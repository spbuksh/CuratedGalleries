using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Action or controller marked with this attribute are not required for authentication
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CorbisAuthIgnoreFilterAttribute : Attribute
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public CorbisAuthIgnoreFilterAttribute() : base()
        { }
    }
}