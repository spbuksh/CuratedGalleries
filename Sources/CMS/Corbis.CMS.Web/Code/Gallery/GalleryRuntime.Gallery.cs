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
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {
        #region Gallery file system structure 

        /// <summary>
        /// Gallery output name
        /// </summary>
        protected static string GalleryOutputDirName
        {
            get { return "Output"; }
        }
        /// <summary>
        /// Gallery content folder name. This folder contains gallery data and gallery state
        /// </summary>
        protected static string GalleryContentDirName
        {
            get { return "Content"; }
        }

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
            return Path.Combine(GetGalleryPath(id), GalleryOutputDirName);
        }

        /// <summary>
        /// This folder contains gallery content (images, xml view state file and ect). This folder has strict name.
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <returns></returns>
        public static string GetGalleryContentPath(int id)
        {
            return Path.Combine(GetGalleryPath(id), GalleryContentDirName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetGallerySourcePath(int id)
        {
            return Path.Combine(GetGalleryContentPath(id), "GalleryContent.xml");
        }

        #endregion Gallery file system structure

        /// <summary>
        /// Loads gallery content data by gallery identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GalleryContent LoadGalleryContent(int id)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GalleryContent));

            using (FileStream fstream = new FileStream(GetGallerySourcePath(id), FileMode.Open, FileAccess.Read))
            {
                return serializer.Deserialize(fstream) as GalleryContent;
            }
        }
        /// <summary>
        /// Loads gallery content data by gallery identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void SaveGalleryContent(int id, GalleryContent content)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GalleryContent));

            using (FileStream fstream = new FileStream(GetGallerySourcePath(id), FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fstream, content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static CuratedGallery CreateGallery(string name, Nullable<int> templateID = null)
        {
            //try to create gallery in the storage
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

            var content = new GalleryContent() { Name = name };

            gallery.SaveContent(content);



            return gallery;
        }

        /// <summary>
        /// Deletes gallery
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        public static bool DeleteGallery(int id)
        {
            OperationResult<OperationResults, Nullable<bool>> rslt = null;

            try
            {
                rslt = GalleryRepository.DeleteGallery(id);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }

            switch (rslt.Result)
            {
                case OperationResults.Success:
                case OperationResults.NotFound:
                    {
                        var dir = new DirectoryInfo(GetGalleryPath(id));
                        if (dir.Exists)
                        {
                            dir.Clear();
                            dir.Delete(true);
                        }
                        return true;
                    }
                case OperationResults.Failure:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Gets gallery by gallery identifier
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        /// <param name="includePackage"></param>
        /// <returns>Return null if gallery was not found. Otrherwize returns gallery object</returns>
        public static CuratedGallery GetGallery(int id, bool includePackage = false)
        {
            OperationResult<OperationResults, CuratedGallery> rslt = null;

            try
            {
                rslt = GalleryRepository.GetGallery(id, includePackage);
            }
            catch(Exception ex)
            {
                Logger.WriteError(ex);
            }

            switch(rslt.Result)
            {
                case OperationResults.Success:
                    break;
                case OperationResults.NotFound:
                    return null;
                case OperationResults.Failure:
                    throw new Exception(string.Format("Internal server error during gallery(id={0}) retrival from the storage", id));
                default:
                    throw new NotImplementedException();
            }

            return rslt.Output;
        }


        public static CuratedGallery BuildGalleryOutput(int id)
        {
            CuratedGallery gallery = GetGallery(id);

            if (gallery == null)
                throw new Exception(string.Format("Gallery with id='{0}' was not found", id));

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

        /// <summary>
        /// Builds gallery output
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="xfilepath"></param>
        protected static void BuildOutput(DirectoryInfo source, DirectoryInfo target, string xfilepath)
        {
            if (!target.Exists)
                target.Create();

            foreach (var file in source.GetFiles())
            {
                if (file.Name.ToLower().EndsWith(".xslt"))
                {
                    var outfilepath = file.Name.Substring(0, file.Name.Length - ".xslt".Length);
                    Transform(xfilepath, file.FullName, Path.Combine(target.FullName, outfilepath));
                }
                else
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);
                }
            }

            foreach (var subdir in source.GetDirectories())
                BuildOutput(subdir, new DirectoryInfo(Path.Combine(target.FullName, subdir.Name)), xfilepath);
        }

        /// <summary>
        /// Executes xslt transformation
        /// </summary>
        /// <param name="xmlfilepath"></param>
        /// <param name="xsltfilepath"></param>
        /// <param name="outfilepath"></param>
        protected static void Transform(string xmlfilepath, string xsltfilepath, string outfilepath)
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