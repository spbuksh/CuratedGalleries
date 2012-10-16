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
        public string GalleryDirectory { get; protected set; }

        /// <summary>
        /// Absolute directory path to folder with shared images
        /// </summary>
        public string SharedDirectory { get; protected set; }

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
            this.GalleryDirectory = this.GetAbsolutePath(section.Settings.GalleryDirectory);

            if (!Directory.Exists(this.GalleryDirectory))
                Directory.CreateDirectory(this.GalleryDirectory);

            //
            this.MaxImageSize = section.Settings.MaxImageSize;
            this.MinImageSize = section.Settings.MinImageSize;

            //
            this.SharedDirectory = this.GetAbsolutePath(section.Settings.SharedDirectory);

            if (!Directory.Exists(this.SharedDirectory))
                Directory.CreateDirectory(this.SharedDirectory);

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


        public OperationResult<OperationResults, GalleryTemplateEntry> AddTemplate(string filename, byte[] package)
        {
            //
            using (var stream = new MemoryStream(package))
            {
                using (var zip = ZipFile.Read(stream))
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                    zip.ExtractAll(tempDir);

                    //read template descriptor into 
                    var template = new GalleryTemplateInfo() { Package = new GalleryTemplatePackage() { Filename = filename, Content = package } };


                    //
                    var ares = this.GalleryRepository.AddTemplate(template);


                    string tpath = Path.Combine(this.TemplateDirectory, ares.Output.ID.ToString());

                    //move files from path to tpath

                }
            }


            throw new NotImplementedException();
        }
    }
}