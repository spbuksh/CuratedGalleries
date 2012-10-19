using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.CMS.Repository.Interface.Communication;
using System.Web.Routing;
using System.Xml.Xsl;
using System.Xml;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {
        #region Gallery file system structure 

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
        /// This folder contains result gallery for preview. This folder has strict name.
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public static string GetGalleryOutputPath(int id)
        {
            return Path.Combine(GetGalleryPath(id), "Output");
        }

        /// <summary>
        /// This folder contains gallery content (images, xml view state file and ect). This folder has strict name.
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public static string GetGalleryContentPath(int id)
        {
            return Path.Combine(GetGalleryPath(id), "Content");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetGallerySourcePath(int id)
        {
            var file = Directory.GetFiles(GetGalleryContentPath(id), "*.xml").SingleOrDefault();
            return string.IsNullOrEmpty(file) ? null : file;
        }

        #endregion Gallery file system structure

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

            Directory.CreateDirectory(GetGalleryPath(gallery.ID));
            Directory.CreateDirectory(GetGalleryOutputPath(gallery.ID));
            Directory.CreateDirectory(GetGalleryContentPath(gallery.ID));


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

            //var gdir = tdir.CopyTo(gallery.GetFolderPath());


            return gallery;
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


        public static CuratedGallery BuildGalleryOutput(int id)
        {
            CuratedGallery gallery = GetGallery(id);

            var outputDir = new DirectoryInfo(GalleryRuntime.GetGalleryOutputPath(id));

            if (outputDir.Exists)
            {
                outputDir.Clear();
            }
            else
            {
                outputDir.Create();
            }

            BuildOutput(new DirectoryInfo(GalleryRuntime.GetTemplatePath(gallery.TemplateID)), outputDir, GetGallerySourcePath(id));

            return gallery;
        }
        public static void BuildOutput(DirectoryInfo source, DirectoryInfo target, string xfilepath)
        {
            if (!target.Exists)
                target.Create();

            foreach (var file in source.GetFiles())
            {
                //if (file.Name.ToLower().EndsWith(".xslt"))
                //{
                //    var outfilepath = file.Name.Substring(0, file.Name.Length - ".xslt".Length);
                //    Transform(xfilepath, file.FullName, Path.Combine(source.FullName, outfilepath));
                //}
                //else
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);
                }
            }

            foreach (var subdir in source.GetDirectories())
                BuildOutput(subdir, new DirectoryInfo(Path.Combine(target.FullName, subdir.Name)), xfilepath);
        }

        public static void Transform(string xmlfilepath, string xsltfilepath, string outfilepath)
        {
            XslCompiledTransform transform = new XslCompiledTransform();

            using (XmlReader reader = XmlReader.Create(xsltfilepath))
            {
                transform.Load(reader);
            }

            transform.Transform(xmlfilepath, outfilepath);
        }

    }
}