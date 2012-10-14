using System.Web;
using System.Web.Mvc;
using Corbis.WebAPI.Code;

namespace Corbis.WebAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CorbisAuthFilterAttribute());
        }
    }
}