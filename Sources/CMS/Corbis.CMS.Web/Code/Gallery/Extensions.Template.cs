using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        /// <summary>
        /// Gets absolute folder path to the template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string GetFolderPath(this GalleryTemplate template)
        {
            return GalleryRuntime.GetTemplatePath(template.ID);
        }

        /// <summary>
        /// Gets absolute folder path to the template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string GetRootPath(this IGalleryTemplate template)
        {
            return GalleryRuntime.GetTemplatePath(template.ID);
        }

    }
}