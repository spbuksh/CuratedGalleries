using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Corbis.Public.Repository.Proxy
{
    public class Utils
    {
        /// <summary>
        /// Gets strongly typed data from REST service using json data exchange format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T GetData<T>(string url)
        {
            WebClient client = new WebClient();
            string datastring = client.DownloadString(url);
            return System.Web.Helpers.Json.Decode<T>(datastring);
        }
        
    }
}
