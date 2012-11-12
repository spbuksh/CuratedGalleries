using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using System.IO;
using System.Xml.Linq;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        /// <summary>
        /// Gets absolute gallery root folder path. All gallery data is located inside this folder
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetRootPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryPath(gallery.ID);
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
        /// Gets absolute gallery root folder path. All gallery data is located inside this folder
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetContentPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryContentPath(gallery.ID);
        }


        /// <summary>
        /// Gets url to the entry point html file
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetPreviewUrl(this CuratedGallery gallery, HttpContextBase context = null, bool vdirProcess = true)
        {
            var dir = new DirectoryInfo(GalleryRuntime.GetTemplatePath(gallery.TemplateID));

            //TODO: temporary I guess that template directory exists
            if(!dir.Exists)
                throw new Exception();

            var file = dir.GetFiles("*.xslt").Where(x => x.Name.ToLower().EndsWith(".html.xslt")).SingleOrDefault();

            if(file == null)
                throw new Exception("Template must have single entry point file with extension '.html.xslt'");

            string htmlpath = Path.Combine(gallery.GetOutputPath(), file.Name.Substring(0, file.Name.Length - ".xlst".Length));
            return Corbis.Common.Utils.AbsoluteToVirtual(htmlpath, context, vdirProcess);
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

    }
}