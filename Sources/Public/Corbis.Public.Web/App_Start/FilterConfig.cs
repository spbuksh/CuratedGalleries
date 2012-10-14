using System.Web;
using System.Web.Mvc;

namespace Corbis.Pubic.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //!!! DO NOT DELETE AND UNCOMMENT!!! We use custom error processing!!!
            //filters.Add(new HandleErrorAttribute());
        }
    }
}