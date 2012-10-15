using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.Common;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// It is stateless and threadsafe
    /// </summary>
    public class CuratedGalleryEnvironment
    {
        /// <summary>
        /// Gallery template repository
        /// </summary>
        [Dependency]
        public ICuratedGalleryRepository GalleryRepository { get; set; }

        #region Environment settings

        /// <summary>
        /// Absolute directory path to folder with templates
        /// </summary>
        public string TemplateDirectory { get; protected set; }

        /// <summary>
        /// Absolute directory path to folder with deployed (released) curated galleries
        /// </summary>
        public string DeploymentDirectory { get; protected set; }

        /// <summary>
        /// Absolute directory path to folder with curated galleries which are being developed
        /// </summary>
        public string DevelopmentDirectory { get; protected set; }

        /// <summary>
        /// Absolute directory path to folder with shared images
        /// </summary>
        public string SharedImageDirectory { get; protected set; }

        /// <summary>
        /// Min image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        public Nullable<long> MinImageSize { get; protected set; }

        /// <summary>
        /// Max image size in bytes. This rule will be applied to all curated galleries
        /// </summary>
        public Nullable<long> MaxImageSize { get; protected set; }

        /// <summary>
        /// Default template image url
        /// </summary>
        public ImageUrlSet DefaultTemplateImageUrl { get; protected set; }

        #endregion Environment settings

        /// <summary>
        /// Initializes environment with data from configuration section
        /// </summary>
        /// <param name="configSection"></param>
        public void Initialize(string configSection = null)
        {
            var section = CuratedGalleryEnvironmentSection.GetSection(configSection);

            //
            this.DeploymentDirectory = this.GetAbsolutePath(section.Settings.DeploymentDirectory);

            if (!Directory.Exists(this.DeploymentDirectory))
                Directory.CreateDirectory(this.DeploymentDirectory);

            //
            this.DevelopmentDirectory = this.GetAbsolutePath(section.Settings.DevelopmentDirectory);

            if (!Directory.Exists(this.DevelopmentDirectory))
                Directory.CreateDirectory(this.DevelopmentDirectory);

            this.MaxImageSize = section.Settings.MaxImageSize;
            this.MinImageSize = section.Settings.MinImageSize;

            //
            this.SharedImageDirectory = this.GetAbsolutePath(section.Settings.SharedImageDirectory);

            if (!Directory.Exists(this.SharedImageDirectory))
                Directory.CreateDirectory(this.SharedImageDirectory);

            //
            this.TemplateDirectory = this.GetAbsolutePath(section.Settings.TemplateDirectory);

            if (!Directory.Exists(this.TemplateDirectory))
                Directory.CreateDirectory(this.TemplateDirectory);

            //
            this.DefaultTemplateImageUrl = new ImageUrlSet() { Large = section.Settings.DefaultTemplateImageUrl };

            if (!Directory.Exists(this.TemplateDirectory))
                Directory.CreateDirectory(this.TemplateDirectory);
        }

        /// <summary>
        /// Gets absolute path based on relative path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetAbsolutePath(string path)
        {
            return Path.IsPathRooted(path) ? path :
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.TrimStart(Path.DirectorySeparatorChar));
        }

        ///// <summary>
        ///// Deploys gallery.
        ///// </summary>
        //public void DeployGallery();


        //public OperationResult<OperationResults, GalleryTemplateEntry> AddTemplate(byte[] archive)
        //{ }
    }
}