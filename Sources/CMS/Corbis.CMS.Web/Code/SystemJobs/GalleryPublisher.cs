﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Job.Core;
using Corbis.CMS.Repository.Interface;

namespace Corbis.CMS.Web.Code.SystemJobs
{
    public class GalleryPublisher : JobBase
    {
        [Microsoft.Practices.Unity.Dependency]
        public ICuratedGalleryRepository GalleryRepository { get; set; }

        protected override void ExecuteCore()
        {
            //throw new NotImplementedException();
        }
    }
}