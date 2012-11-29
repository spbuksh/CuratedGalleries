using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using System.IO;
using System.Xml.Linq;
using Corbis.Common;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        /// <summary>
        /// Gets absolute gallery dev root folder path. All gallery data is located inside this folder
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetDevPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryDevPath(gallery.ID);
        }

        /// <summary>
        /// Gets absolute gallery output folder path. This folder contains output gallery files
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetOutputPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryOutputPath(gallery.ID);
        }
        /// <summary>
        /// Gets absolute gallery output folder path. This folder contains output gallery files
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetLiveOutputPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryLiveOutputPath(gallery.ID);
        }

        /// <summary>
        /// Gets absolute gallery root folder path. All gallery data is located inside this folder
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetContentPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryContentPath(gallery.ID);
        }

        /// <summary>
        /// Gets absolute gallery live root folder path.
        /// </summary>
        /// <param name="gallery"></param>
        /// <returns></returns>
        public static string GetLivePath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryLivePath(gallery.ID);
        }

        /// <summary>
        /// Gets url to the entry point html file
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetPreviewUrl(this CuratedGallery gallery, HttpContextBase context = null, bool vdirProcess = true)
        {
            var file = GetTemplateEntryPointFile(gallery.TemplateID);
            string htmlpath = Path.Combine(gallery.GetOutputPath(), file.Name.Substring(0, file.Name.Length - ".xlst".Length));
            return Corbis.Common.Utils.AbsoluteToVirtual(htmlpath, context, vdirProcess);
        }
        /// <summary>
        /// Gets url to the entry point html file
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetLiveUrl(this CuratedGallery gallery, HttpContextBase context = null, bool vdirProcess = true)
        {
            var file = GetTemplateEntryPointFile(gallery.TemplateID);
            string htmlpath = Path.Combine(gallery.GetLiveOutputPath(), file.Name.Substring(0, file.Name.Length - ".xlst".Length));
            return Corbis.Common.Utils.AbsoluteToVirtual(htmlpath, context, vdirProcess);
        }
        private static FileInfo GetTemplateEntryPointFile(int templateID)
        {
            var dir = new DirectoryInfo(GalleryRuntime.GetTemplatePath(templateID));

            //TODO: temporary I guess that template directory exists
            if (!dir.Exists)
                throw new Exception();

            var file = dir.GetFiles("*.xslt").Where(x => x.Name.ToLower().EndsWith(".html.xslt")).SingleOrDefault();

            if (file == null)
                throw new Exception("Template must have single entry point file with extension '.html.xslt'");

            return file;
        }

        public static GalleryContent LoadContent(this CuratedGallery gallery, bool bSync = true)
        {
            return GalleryRuntime.LoadGalleryContent(gallery.ID, bSync);
        }
        public static void SaveContent(this CuratedGallery gallery, GalleryContent content, bool bSync = true)
        {
            GalleryRuntime.SaveGalleryContent(gallery.ID, content, bSync);
        }
        public static object GetSyncRoot(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGallerySyncRoot(gallery.ID);
        }

        public static OperationResults Publish(this CuratedGallery gallery, int? userID, DateTime fromUTC, DateTime? toUTC)
        {
            return GalleryRuntime.PublishGallery(userID, gallery.ID, fromUTC, toUTC);
        }
        public static OperationResults GoLive(this CuratedGallery gallery)
        {
            return GalleryRuntime.GoGalleryLive(gallery.ID);
        }
        public static OperationResults UnPublish(this CuratedGallery gallery)
        {
            return GalleryRuntime.UnPublishGallery(gallery.ID);
        }

    }
}