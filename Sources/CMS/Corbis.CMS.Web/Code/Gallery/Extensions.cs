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

        public static string GetDirectory(this CuratedGallery gallery)
        {
            return Path.Combine(GalleryRuntime.GalleryDirectory, gallery.ID.ToString());
        }
        public static string GetUrl(this CuratedGallery gallery)
        {
            throw new NotImplementedException();
        }

        #endregion Decorator for CuratedGallery

        #region Decorator for GalleryTemplate

        public static string GetDirectory(this GalleryTemplate template)
        {
            throw new NotImplementedException();
        }

        #endregion Decorator for GalleryTemplate

    }
}