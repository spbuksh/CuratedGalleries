using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using Corbis.Job.Core;
using Corbis.Job.Interface;
using Corbis.Job;
using Corbis.CMS.Web.GarbageCollector;
using System.Web.Http;

namespace Corbis.CMS.Web.Controllers.Api
{
    public class JobController : ApiControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Execute()
        {
            System.Threading.ThreadStart tstart = delegate()
            {
                IJobManager jobMngr = null;

                var section = JobConfigurationSection.GetSection();

                if (!string.IsNullOrEmpty(section.JobManagerType))
                {
                    Type type = Type.GetType(section.JobManagerType);

                    if (type == null)
                        throw new TypeLoadException(string.Format("Type '{0}' can not be loaded", section.JobManagerType));

                    if (!typeof(IJobManager).IsAssignableFrom(type))
                        throw new Exception(string.Format("Type '{0}' must implement '{1}'", section.JobManagerType, typeof(IJobManager).AssemblyQualifiedName));

                    jobMngr = Activator.CreateInstance(type) as IJobManager;
                }
                else
                {
                    jobMngr = new JobManager();
                }

                using (jobMngr)
                {
                    jobMngr.Initialize(null, MvcApplication.Container);
                    jobMngr.Start();
                    jobMngr.Stop(true);
                }
            };

            //run logic in another thread not to make client wait for answer
            System.Threading.Thread thread = new System.Threading.Thread(tstart) { IsBackground = true };
            thread.Start();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Executes file garbage collector to remove obsolete files and folders
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage ExecFGC()
        {
            CorbisGarbageCollector.Collect();
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Publish/unpublish galleries
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage ExecGalleryPublising()
        {
            throw new NotImplementedException();
        }

    }
}