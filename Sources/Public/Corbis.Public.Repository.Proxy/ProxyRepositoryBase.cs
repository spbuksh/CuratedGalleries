using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;

namespace Corbis.Public.Repository.Proxy
{
    /// <summary>
    /// Base repository proxy for all repositories
    /// </summary>
    public class RepositoryProxyBase
    {
        protected ILogManager Logger
        {
            get { return Logging.LogManagerProvider.Instance; }
        }

        public T GetData<T>(string url)
        {
            return Utils.GetData<T>(url);
        }

    }
}
