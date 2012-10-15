using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Presentation.Common;
using Microsoft.Practices.Unity;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Repository data provider
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Creates or gets repository instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return MvcApplication.Container.Resolve<T>();
        }
    }
}