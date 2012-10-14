using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace Corbis.Presentation.Common
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        public static IUnityContainer Container { get; set; }

        public IoCControllerFactory()
        { }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var controller = base.CreateController(requestContext, controllerName);
            IoCControllerFactory.Container.BuildUp(controller.GetType(), controller);
            return controller;
        }

    }
}
