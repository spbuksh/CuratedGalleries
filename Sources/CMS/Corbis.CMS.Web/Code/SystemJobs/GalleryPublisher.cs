using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Web.Code;
using Corbis.Job.Core;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.CMS.Entity;
using System.Threading;

namespace Corbis.CMS.Web.Code.SystemJobs
{
    public class GalleryPublisher : JobBase
    {
        [Microsoft.Practices.Unity.Dependency]
        public ICuratedGalleryRepository GalleryRepository { get; set; }

        protected override void ExecuteCore()
        {
            var action = this.GalleryRepository.GetGalleries(new CuratedGalleryFilter() { Enabled = true, Status = CuratedGalleryStatuses.Published });

            var nowUTC = DateTime.UtcNow;

            WaitCallback goLive = delegate(object state)
            {
                var items = state as object[];

                var gallery = items[0] as CuratedGallery;

                lock (gallery.GetSyncRoot())
                {
                    try { gallery.GoLive(); }
                    catch { }
                }
            };

            WaitCallback stopLive = delegate(object state)
            {
                var gallery = state as CuratedGallery;

                lock (gallery.GetSyncRoot())
                {
                    try { gallery.UnPublish(); }
                    catch { }
                }
            };

            foreach (var item in action.Output)
            {
                if (item.PublicationPeriod == null)
                    continue;

                if (item.PublicationPeriod.Start <= nowUTC && (!item.PublicationPeriod.End.HasValue || item.PublicationPeriod.End.Value > nowUTC))
                    ThreadPool.QueueUserWorkItem(goLive, new object[] { item, item.PublicationPeriod.Start, item.PublicationPeriod.End });

                if (item.PublicationPeriod.End.HasValue && item.PublicationPeriod.End.Value <= nowUTC)
                    ThreadPool.QueueUserWorkItem(stopLive, item);
            }
        }
    }
}