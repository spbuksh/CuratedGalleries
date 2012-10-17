using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using System.IO;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        #region Decorator for CuratedGallery

        /// <summary>
        /// Gets absolute folder path to the gallery
        /// </summary>
        /// <param name="gallery"></param>
        /// <returns></returns>
        public static string GetFolderPath(this CuratedGallery gallery)
        {
            return GalleryRuntime.GetGalleryFolderPath(gallery.ID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gallery"></param>
        /// <returns></returns>
        public static string GetUrl(this CuratedGallery gallery)
        {
            throw new NotImplementedException();
        }

        #endregion Decorator for CuratedGallery

        #region Decorator for GalleryTemplate

        /// <summary>
        /// Gets absolute folder path to the template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string GetFolderPath(this GalleryTemplate template)
        {
            return GalleryRuntime.GetTemplateFolderPath(template.ID);
        }

        #endregion Decorator for GalleryTemplate

    }
}