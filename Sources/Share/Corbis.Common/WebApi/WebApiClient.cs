using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common.WebApi
{
    /// <summary>
    /// This class provides web api client application settings
    /// </summary>
    public class WebApiClient
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        protected WebApiClient()
        { }

        /// <summary>
        /// Settings provider - singleton
        /// </summary>
        public static WebApiClient Instance
        {
            get
            {
                //use double block
                if (m_Instance == null)
                {
                    lock (typeof(WebApiClient))
                    {
                        if (m_Instance == null)
                        {
                            var section = WebApiClientConfigSection.GetSection();

                            if (object.ReferenceEquals(section.Settings, null))
                                throw new Exception("Corbis client config section does not have settings element");

                            m_Instance = new WebApiClient()
                            {
                                ClientID = section.Settings.ClientID,
                                ClientSecret = section.Settings.Secret,
                                AuthUrl = section.Settings.AuthUrl,
                                ApiUrl = section.Settings.ApiUrl
                            };
                        }
                    }
                }
                return m_Instance;
            }
        }
        private static WebApiClient m_Instance = null;

        /// <summary>
        /// Client Application identifier
        /// </summary>
        public string ClientID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Client Application Secret
        /// </summary>
        public string ClientSecret
        {
            get;
            protected set;
        }

        /// <summary>
        /// Base authenticate url. It is string like "https://secure.corbis.com"
        /// </summary>
        public string AuthUrl
        {
            get;
            protected set;
        }

        /// <summary>
        /// Base API url. It is string like "https://api.corbisimages.com"
        /// </summary>
        public string ApiUrl
        {
            get;
            protected set;
        }
    }
}
