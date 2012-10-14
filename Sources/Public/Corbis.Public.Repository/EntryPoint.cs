using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Common;
using Microsoft.Practices.Unity;
using Corbis.Public.Repository.Interface;

namespace Corbis.Public.Repository
{
    public class EntryPoint : IRepositoryEntryPoint
    {
        /// <summary>
        /// Target container
        /// </summary>
        /// <param name="container">External container for type registration</param>
        public virtual void Register(IUnityContainer container, InstanceCreationMode mode = InstanceCreationMode.PerCall)
        {
            switch(mode)
            {
                case InstanceCreationMode.PerCall:
                    this.RegisterTypes(container);
                    break;
                case InstanceCreationMode.Singleton:
                    this.RegisterInstances(container);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual void RegisterTypes(IUnityContainer container)
        {
            //some necessary type or instance registration
            //....
            container.RegisterType(typeof(IUserRepository), typeof(UserRepository));
            container.RegisterType(typeof(ISearchRepository), typeof(SearchRepository));
        }
        protected virtual void RegisterInstances(IUnityContainer container)
        {
            //some necessary type or instance registration
            //....

            //take into account that if the repository is depended on any other type or repository then they must be registered before
            //in section above
            var user = new UserRepository();
            container.BuildUp(user.GetType(), user);
            container.RegisterInstance<IUserRepository>(user);

            var search = new SearchRepository();
            container.BuildUp(search.GetType(), search);
            container.RegisterInstance<ISearchRepository>(search);
        }

    }
}
