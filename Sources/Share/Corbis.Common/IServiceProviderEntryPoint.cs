using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Corbis.Common
{
    public enum InstanceCreationMode
    {
        PerCall,
        Singleton
    }

    public interface IRepositoryEntryPoint
    {
        /// <summary>
        /// Registers services inside the provided container.
        /// </summary>
        /// <param name="container">External container for type registration. It is external because container can be used in different scenarious (can be used as main or child and so on)</param>
        void Register(IUnityContainer container, InstanceCreationMode mode = InstanceCreationMode.PerCall);
    }
}
