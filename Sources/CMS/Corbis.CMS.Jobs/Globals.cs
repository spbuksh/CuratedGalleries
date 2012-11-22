using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Corbis.Presentation.Common.RepositoryProvider;

namespace Corbis.CMS.Jobs
{
    public class Globals
    {
        public static T GetRepository<T>()
        {
            return RepositoryContainer.Resolve<T>();
        }
        public static IUnityContainer RepositoryContainer 
        {
            get
            {
                if (m_RepositoryContainer == null)
                {
                    lock (typeof(Globals))
                    {
                        if (m_RepositoryContainer == null)
                        {
                            m_RepositoryContainer = new UnityContainer();
                            RepositoryProvider.Register(m_RepositoryContainer); 
                        }
                    }
                }
                return m_RepositoryContainer;
            }
        }
        private static IUnityContainer m_RepositoryContainer = null;
    }
}
