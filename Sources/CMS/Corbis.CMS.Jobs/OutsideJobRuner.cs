using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Core;
using System.Net;
using System.Configuration;

namespace Corbis.CMS.Jobs
{
    public class OutsideJobRuner : JobBase
    {
        protected const string UrlKey = "jobExecuteUrl";

        protected override void ExecuteCore()
        {
            try
            {
                string url = ConfigurationManager.AppSettings[UrlKey];

                if (string.IsNullOrEmpty(url))
                    throw new Exception(string.Format("Application setting with key '{0}' is not set", UrlKey));

                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode != HttpStatusCode.OK)
                    this.Logger.WriteWarning(string.Format("Response for GET request for url '{0}' has status code {1}({2})", url, (int)response.StatusCode, response.StatusCode));
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
            }
        }
    }
}
