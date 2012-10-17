using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.CMS.Repository.Interface.Communication;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {
        /// <summary>
        /// Gets absolute folder path to the gallery based on its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetGalleryFolderPath(int id)
        {
            return Path.Combine(GalleryRuntime.GalleryDirectory, id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static CuratedGallery CreateGallery(string name, Nullable<int> templateID = null)
        {
            //try to create gallery
            OperationResult<OperationResults, CuratedGallery> rslt = null;

            try
            {
                rslt = GalleryRuntime.GalleryRepository.CreateGallery(name, templateID);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex, "Gallery creation was failed");
                throw;
            }

            if (rslt.Result != OperationResults.Success)
                throw new Exception("Gallery creation was failed");

            var gallery = rslt.Output;

            //find template for the gallery
            var tdir = new DirectoryInfo(GetTemplateFolderPath(gallery.TemplateID));

            if (!tdir.Exists)
            {
                tdir.Create();

                GalleryRepository.GetTemplate(gallery.TemplateID, GalleryTemplateContent.All);
            }

            var gdir = tdir.CopyTo(gallery.GetFolderPath());


            return gallery;
        }
    }
}