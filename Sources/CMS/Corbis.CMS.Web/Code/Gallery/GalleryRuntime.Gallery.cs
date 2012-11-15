using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Web.Code.Gallery.GalleryContent.ImageContentGenerator;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.CMS.Repository.Interface.Communication;
using System.Web.Routing;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Serialization;
using Corbis.Common.Utilities.Image;
using System.Configuration;

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

        public static string GetDefaultGalleryCoverPath(HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            string rpath = ConfigurationManager.AppSettings["defaultGalleryCoverPath"];

            return Path.Combine(Utils.VirtualToAbsolute("~/", context), rpath.TrimStart(Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Gets object for gallery syncronization
        /// </summary>
        /// <param name="galleryID">gallery identifier</param>
        /// <returns></returns>
        public static object GetGallerySyncRoot(int galleryID)
        {
            lock (m_GallerySyncRoots)
            {
                object sync = null;

                if (!m_GallerySyncRoots.ContainsKey(galleryID))
                {
                    sync = new object();
                    m_GallerySyncRoots[galleryID] = sync;
                }
                else
                {
                    sync = m_GallerySyncRoots[galleryID];
                }

                return sync;
            }
        }
        private static Dictionary<int, object> m_GallerySyncRoots = new Dictionary<int, object>();

        protected static void RemoveGallerySyncRoot(int galleryID)
        {
            lock (m_GallerySyncRoots)
            {
                m_GallerySyncRoots.Remove(galleryID);
            }
        }

        /// <summary>
        /// Loads gallery content data by gallery identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bSync">True - automatic synchronization is required</param>
        /// <returns></returns>
        public static GalleryContent LoadGalleryContent(int id, bool bSync = true)
        {
            ActionHandler<string, GalleryContent> action = delegate(string path)
            { 
                XmlSerializer serializer = new XmlSerializer(typeof(GalleryContent));

                using (FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return serializer.Deserialize(fstream) as GalleryContent;
                }
            };

            if (bSync)
            {
                lock (GetGallerySyncRoot(id))
                {
                    return action(GetGallerySourcePath(id));
                }
            }

            return action(GetGallerySourcePath(id));
        }
        /// <summary>
        /// Loads gallery content data by gallery identifier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bSync">True - automatic synchronization is required</param>
        /// <returns></returns>
        public static void SaveGalleryContent(int id, GalleryContent content, bool bSync = true)
        {
            Action action = delegate()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GalleryContent));

                using (FileStream fstream = new FileStream(GetGallerySourcePath(id), FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fstream, content);
                }
            };

            if (bSync)
            {
                lock (GetGallerySyncRoot(id))
                {
                    action();
                }
            }
            else
            {
                action();
            }
        }

        public static void UpdateGalleryContentImage(int galleryID, string imageID, Action<GalleryImageBase> handler, bool bSync = true)
        {
            Action action = delegate()
            {
                var content = LoadGalleryContent(galleryID);
                var image = content.Images.Single(x => x.ID == imageID);
                handler(image);
                image.ImageContentUrl = Utils.AbsoluteToVirtual(ImageContentGenerator.GererateImage(image as GalleryContentImage, galleryID), null);
                image.ImageContentName = Path.GetFileName(image.ImageContentUrl);
                SaveGalleryContent(galleryID, content);
            };

            if (bSync)
            {
                lock (GetGallerySyncRoot(galleryID))
                {
                    action();
                }
            }
            else
            {
                action();
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

            lock (gallery.GetSyncRoot())
            {
                string galleryRoot = gallery.GetRootPath();
                string galeryOutput = gallery.GetOutputPath();

                Directory.CreateDirectory(galleryRoot);
                Directory.CreateDirectory(galeryOutput);
                Directory.CreateDirectory(gallery.GetContentPath());

                IGalleryTemplate template = GetTemplate(gallery.TemplateID);

                var content = new GalleryContent()
                {
                    Name = name,
                    Font = template.GallerySettings.DefaultFontFamily == null ? null : new GalleryFont() { FamilyName = template.GallerySettings.DefaultFontFamily.Name }
                };

                content.GalleryImageSizes.AddRange(template.GallerySettings.ImageSizes.Select(x => ObjectMapper.DoMapping<GalleryImageSize>(x)));

                //add content file as system. It must be ignored
                content.SystemFilePathes.Add(GetGallerySourcePath(gallery.ID).Substring(galleryRoot.Length));

                //relevant output path
                var reloutputPath = galeryOutput.Substring(galleryRoot.Length).TrimEnd('\\');

                //ignore template cover
                if (template.Icon != null && template.Icon.Type == ImageSourceTypes.LocalFile)
                    content.SystemFilePathes.Add(string.Format("{0}\\{1}", reloutputPath, template.Icon.Source));

                if (!string.IsNullOrEmpty(template.DescriptorFilepath))
                    content.SystemFilePathes.Add(string.Format("{0}\\{1}", reloutputPath, template.DescriptorFilepath));

                gallery.SaveContent(content, false);
            }

            return gallery;
        }

        /// <summary>
        /// Deletes gallery
        /// </summary>
        /// <param name="id">Gallery identifier</param>
        public static bool DeleteGallery(int id)
        {
            lock (GetGallerySyncRoot(id))
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
                                try
                                {
                                    dir.Remove();
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteError(ex);
                                }
                            }

                            RemoveGallerySyncRoot(id);

                            return true;
                        }
                    case OperationResults.Failure:
                        return false;
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public static List<CuratedGallery> GetGalleries()
        {
            OperationResult<OperationResults, List<CuratedGallery>> rslt = null;

            try
            {
                rslt = GalleryRepository.GetGalleries();
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
                    break;
                case OperationResults.Failure:
                    throw new Exception("...");
                default:
                    throw new NotImplementedException();
            }
            return rslt.Output;
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

        /// <summary>
        /// Updates gallery data
        /// </summary>
        /// <param name="gallery"></param>
        /// <returns></returns>
        public static OperationResults UpdateGallery(CuratedGallery gallery, bool includePackage = false)
        {
            OperationResult<OperationResults, object> rslt = null;

            try
            {
                rslt = GalleryRepository.UpdateGallery(gallery, includePackage);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                throw;
            }

            return rslt.Result;
        }


        public static CuratedGallery BuildGalleryOutput(int id)
        {
            lock (GetGallerySyncRoot(id))
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


        public static void ChangeContentImageOrder(int galleryID, string firstImageID, string secondImageID)
        {
            lock (GetGallerySyncRoot(galleryID))
            {
                var content = LoadGalleryContent(galleryID, false);

                var first = content.Images.First(x => x.ID == firstImageID);
                content.Images.Remove(first);

                for (int i = 1; i < content.Images.Count; i++)
                {
                    if (content.Images[i].ID != secondImageID)
                        continue;

                    content.Images.Insert(i, first);
                    break;
                }

                for (int i = 1; i < content.Images.Count; i++)
                    content.Images[i].Order = i + 1;

                SaveGalleryContent(galleryID, content, false);
            }
        }
    }
}