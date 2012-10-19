using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.CMS.Repository.Interface.Communication;
using System.Web.Routing;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {
        /// <summary>
        /// Gets absolute folder path to the gallery based on its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetGalleryPath(int id)
        {
            return Path.Combine(GalleryRuntime.GalleryDirectory, id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="htmlfile"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetGalleryPreviewUrl(int id, string htmlfile, HttpContextBase context = null)
        {
            if(context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            return GetGalleryPreviewUrl(id, htmlfile, new RequestContext(context, new RouteData()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <param name="htmlfile">Gallery entry point html file</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetGalleryPreviewUrl(int id, string htmlfile, RequestContext context)
        {
            var helper = new System.Web.Mvc.UrlHelper(context, RouteTable.Routes);

            //NOTE: RouteConfig.RegisterRoutes has "GalleryPreview" root registration. So if you change this logic it 
            return helper.RouteUrl("GalleryPreview", new { id = id, filename = htmlfile });
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
            var tdir = new DirectoryInfo(GetTemplatePath(gallery.TemplateID));

            if (!tdir.Exists)
            {
                tdir.Create();

                GalleryRepository.GetTemplate(gallery.TemplateID, GalleryTemplateContent.All);

                //unpack files into template directory
                //...
            }

            //create source xml file

            var gdir = tdir.CopyTo(gallery.GetFolderPath());


            return gallery;
        }

        /// <summary>
        /// Absolute path to the gallery content xml file. This file contains gallery data state.
        /// </summary>
        /// <param name="id">gallery identifier</param>
        /// <returns></returns>
        public static string GetGallerySourcePath(int id)
        {
            return Path.Combine(GetGalleryPath(id), "GalleryContent.xml");
        }

        /// <summary>
        /// Gets gallery template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CuratedGallery GetGallery(int id)
        {
            return new CuratedGallery() { ID = id, TemplateID = 1 };

            //throw new NotImplementedException();
        }

    }
}