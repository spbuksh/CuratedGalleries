using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Logging.Interface;
using System.Net;
using System.Configuration;
using System.Net.Http;
using Corbis.Common.ObjectMapping.Mappers;

namespace Corbis.Public.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryBase
    {
        /// <summary>
        /// Object mapper. It is helper to copy data from one object to another one
        /// </summary>
        protected virtual MapperBase ObjectMapper
        {
            get { return Corbis.Common.ObjectMapping.Mappers.ObjectMapper.Instance; }
        }

        protected static string MainConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(m_MainConnectionString))
                {
                    lock (typeof(RepositoryBase))
                    {
                        if (string.IsNullOrEmpty(m_MainConnectionString))
                            m_MainConnectionString = ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString;
                    }
                }
                return m_MainConnectionString;
            }
        }
        private static string m_MainConnectionString = null;

        /// <summary>
        /// This method must be used to work with Main DB for every action separately
        /// http://stackoverflow.com/questions/2641725/linq2sql-singleton-or-using-best-practices
        /// </summary>
        /// <returns></returns>
        protected virtual Corbis.DB.Linq.MainDataContext CreateMainContext()
        {
            return new Corbis.DB.Linq.MainDataContext(MainConnectionString);
        }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogManager Logger
        {
            get { return Logging.LogManagerProvider.Instance; }
        }
    }
}
