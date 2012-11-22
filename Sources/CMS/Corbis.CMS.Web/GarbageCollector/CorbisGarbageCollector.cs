using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Corbis.CMS.Web.Code;
using Corbis.Common;
using Corbis.Logging;
using Corbis.Logging.Interface;
using System.Configuration;

namespace Corbis.CMS.Web.GarbageCollector
{
    /// <summary>
    /// Class to find and remove unnecesary files in the system.
    /// </summary>
    public static class CorbisGarbageCollector
    {
        /// <summary>
        /// System logger
        /// </summary>
        private static ILogManager Logger
        {
            get { return LogManagerProvider.Instance; }
        }

        /// <summary>
        /// Removes all not actual Templates.
        /// </summary>
        private static void CollectTemplates()
        {
            try
            {
                var root = new DirectoryInfo(GalleryRuntime.TemplateDirectory);
                var actualTemplates = GalleryRuntime.GetTemplates().Select(_ => _.ID.ToString()).ToList();
                var dirictories = root.GetDirectories().Select(_ => _.Name).ToList();
                var dirictoriesToRemove = dirictories.Except(actualTemplates).ToList();

                foreach (var directoryInfo in root.GetDirectories().Where(_ => dirictoriesToRemove.Contains(_.Name)))
                {
                    try     { directoryInfo.Remove(); }
                    catch   { }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        /// Removes all not actual Galleries.
        /// </summary>
        private static void CollectGalleries()
        {
            try
            {
                var root = new DirectoryInfo(GalleryRuntime.GalleryDirectory);
                var actualGalleries = GalleryRuntime.GetGalleries().Select(_ => _.ID.ToString()).ToList();
                var dirictories = root.GetDirectories().Select(_ => _.Name).ToList();
                var dirictoriesToRemove = dirictories.Except(actualGalleries).ToList();

                foreach (var directoryInfo in root.GetDirectories().Where(_ => dirictoriesToRemove.Contains(_.Name)))
                {
                    try     { directoryInfo.Remove(); }
                    catch   { }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        /// Start to collect garbage.
        /// </summary>
        public static void Collect()
        {
            try
            {
                //If we have N actions then (N-1) are started async. Last one is working in the same thread
                Action action = delegate() { CollectTemplates(); };
                var asyncRslt = action.BeginInvoke(null, null);
                CollectGalleries();
                System.Threading.WaitHandle.WaitAll(new System.Threading.WaitHandle[] { asyncRslt.AsyncWaitHandle });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
#if DEBUG
                throw;
#endif
            }
        }
    }
}