using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Job.Core;
using Corbis.CMS.Web.GarbageCollector;

namespace Corbis.CMS.Web.Code.SystemJobs
{
    public class FCG : JobBase
    {
        protected override void ExecuteCore()
        {
            CorbisGarbageCollector.Collect();
        }
    }
}