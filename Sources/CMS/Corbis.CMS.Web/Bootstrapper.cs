using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.WebApi;

namespace Corbis.CMS.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            return container;
        }
    }
}