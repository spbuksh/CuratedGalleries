using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.Common;
using Ionic.Zip;
using Corbis.Logging;
using Corbis.Logging.Interface;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// It is stateless and threadsafe
    /// </summary>
    public partial class GalleryRuntime
    {
        /// <summary>
        /// Gallery template repository
        /// </summary>
        protected static ICuratedGalleryRepository GalleryRepository 
        {
            get
            {
                if (m_GalleryRepository == null)
                {
                    lock (typeof(GalleryRuntime))
                    {
                        if (m_GalleryRepository == null)
                            m_GalleryRepository = Repository.GetInstance<ICuratedGalleryRepository>();
                    }
                }
                return m_GalleryRepository;
            }
        }
        private static ICuratedGalleryRepository m_GalleryRepository = null;

        /// <summary>
        /// System logger
        /// </summary>
        protected static ILogManager Logger
        {
            get { return LogManagerProvider.Instance; }
        }

        #region Environment settings

        /// <summary>
        /// Absolute directory path to folder with templates
        /// </summary>
        public static string TemplateDirectory { get; private set; }

        /// <summary>
        /// Absolute directory path to folder with (released) curated galleries
        /// </summary>
        public static string GalleryDirectory { get; private set; }

        /// <summary>
        /// Absolute directory path to folder with shared images
        /// </summary>
        public static string SharedDirectory { get; private set; }

        /// <summary>
        /// Min image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        public static Nullable<long> MinImageSize { get; private set; }

        /// <summary>
        /// Max image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        public static Nullable<long> MaxImageSize { get; private set; }

        /// <summary>
        /// Default template image url
        /// </summary>
        public static ImageUrlSet DefaultTemplateImageUrl { get; private set; }

        #endregion Environment settings

        /// <summary>
        /// Initializes environment with data from configuration section
        /// </summary>
        /// <param name="configSection"></param>
        public static void Initialize(string configSection = null)
        {
            var section = CuratedGalleryEnvironmentSection.GetSection(configSection);

            //
            GalleryDirectory = GetAbsolutePath(section.Settings.GalleryDirectory);

            if (!Directory.Exists(GalleryDirectory))
                Directory.CreateDirectory(GalleryDirectory);

            //
            MaxImageSize = section.Settings.MaxImageSize;
            MinImageSize = section.Settings.MinImageSize;

            //
            SharedDirectory = GetAbsolutePath(section.Settings.SharedDirectory);

            if (!Directory.Exists(SharedDirectory))
                Directory.CreateDirectory(SharedDirectory);

            //
            TemplateDirectory = GetAbsolutePath(section.Settings.TemplateDirectory);

            if (!Directory.Exists(TemplateDirectory))
                Directory.CreateDirectory(TemplateDirectory);

            //
            DefaultTemplateImageUrl = new ImageUrlSet() { Large = section.Settings.DefaultTemplateImageUrl };

            if (!Directory.Exists(TemplateDirectory))
                Directory.CreateDirectory(TemplateDirectory);
        }

        /// <summary>
        /// Gets absolute path based on relative path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetAbsolutePath(string path)
        {
            return Path.IsPathRooted(path) ? path :
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.TrimStart(Path.DirectorySeparatorChar));
        }

    }
}