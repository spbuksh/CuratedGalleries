using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Corbis.CMS.Repository.Interface;
using Corbis.Public.Repository.Interface;

namespace Corbis.CMS.Repository
{
    public class EntryPoint : Corbis.Public.Repository.EntryPoint
    {
        protected override void RegisterTypes(IUnityContainer container)
        {         
            container.RegisterType<IAdminUserRepository, AdminUserRepository>();
            container.RegisterType<ICuratedGalleryRepository, CuratedGalleryRepository>();

        }

        protected override void RegisterInstances(IUnityContainer container)
        {
            var admin = new AdminUserRepository();
            container.BuildUp(admin.GetType(), admin);
            container.RegisterInstance<IAdminUserRepository>(admin);

            var tmpl = new CuratedGalleryRepository();
            container.BuildUp(tmpl.GetType(), tmpl);
            container.RegisterInstance<ICuratedGalleryRepository>(tmpl);
        }

    }
}
