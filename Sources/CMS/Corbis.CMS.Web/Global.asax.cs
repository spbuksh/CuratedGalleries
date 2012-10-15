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
using Microsoft.Practices.Unity;
using Corbis.CMS.Web.Code;
using Corbis.Common;
using Corbis.CMS.Repository.Interface;

namespace Corbis.CMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Root unity container for the entire application
        /// </summary>
        public static IUnityContainer Container { get; set; }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();


            //*************************************************************
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs e)
                {
                    Logging.LogManagerProvider.Instance.WriteError(e.ExceptionObject as Exception);
                };

            //logging initialization
            Logging.LogManagerProvider.Initialize("LoggingSection");

            //Repository and controller initialization
            MvcApplication.Container = new UnityContainer();

            //Web Configuration
            IoCControllerFactory.Container = MvcApplication.Container;
            RepositoryProvider.Register(IoCControllerFactory.Container, "RepositoryProvider");
            ControllerBuilder.Current.SetControllerFactory(typeof(IoCControllerFactory));

            //WebApi Configuration
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(MvcApplication.Container);


            //initialize curated gallery environment
            var environment = new CuratedGalleryEnvironment();
            IoCControllerFactory.Container.BuildUp(environment);
            environment.Initialize();
            SingletonProvider<CuratedGalleryEnvironment>.Initialize(environment);
            //*************************************************************
        }
    }
}