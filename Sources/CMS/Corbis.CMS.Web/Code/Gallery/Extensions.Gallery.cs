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
        /// 
        /// </summary>
        /// <param name="gallery">Curated gallery</param>
        /// <returns></returns>
        public static string GetPreviewUrl(this CuratedGallery gallery, HttpContextBase context = null)
        {
            var dir = new DirectoryInfo(GalleryRuntime.GetTemplatePath(gallery.TemplateID));

            //TODO: temporary I guess that template directory exists
            if(!dir.Exists)
                throw new Exception();

            var file = dir.GetFiles("*.xslt").Where(x => x.Name.ToLower().EndsWith(".html.xslt")).SingleOrDefault();

            if(file == null)
                throw new Exception("Template must have single entry point file with extension '.html.xslt'");

            string htmlpath = Path.Combine(gallery.GetOutputPath(), file.Name.Substring(0, file.Name.Length - ".xlst".Length));
            return Corbis.Common.Utils.AbsoluteToVirtual(htmlpath, context);
        }

        //public static GalleryContent Load(this CuratedGallery gallery)
        //{
        //    string xmlpath = GalleryRuntime.GetGallerySourcePath(gallery.ID);


        //}
        //public static void Update(this CuratedGallery gallery, GalleryContent content)
        //{
        //    string path = Path.Combine(gallery.GetFolderPath(), "GalleryContent.xml");

        //    using (FileStream fstream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        //    {
 
        //        fstream.Write(
        //    }

        //    XDocument xdoc = XDocument.Load(path);
        //    xdoc.Save(path);

        //}

    }
}