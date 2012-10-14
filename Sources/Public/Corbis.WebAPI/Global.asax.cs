using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Corbis.Presentation.Common.RepositoryProvider;
using Corbis.Presentation.Common;
using Corbis.WebAPI.Code;
using Microsoft.Practices.Unity;

namespace Corbis.WebAPI
{
    /// <summary>
    /// Note: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Entry Point
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //*************************************************************
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs e)
                {
                    Logging.LogManagerProvider.Instance.WriteError(e.ExceptionObject as Exception);
                };

            //logging initialization
            Logging.LogManagerProvider.Initialize("LoggingSection");

            //Repository and controller initialization
            UnityContainer container = new UnityContainer();
            RepositoryProvider.Register(container, "RepositoryProvider");
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            //*************************************************************
        }
    }
}