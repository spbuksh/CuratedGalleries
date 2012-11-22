using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Job.Core;
using Corbis.Logging.Interface;

namespace Corbis.CMS.Jobs
{
    public abstract class CorbisJobBase : JobBase
    {
        protected T GetRepository<T>()
        {
            return Globals.GetRepository<T>();
        }
    }
}
