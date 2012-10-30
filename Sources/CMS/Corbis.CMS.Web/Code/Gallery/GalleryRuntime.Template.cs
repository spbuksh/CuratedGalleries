using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Repository.Interface;
using Corbis.Common;
using Ionic.Zip;
using System.IO;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {

        /// <summary>
        /// Gets absolute folder path to the template based on its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTemplatePath(int id)
        {
            return Path.Combine(GalleryRuntime.TemplateDirectory, id.ToString());
        }


        /// <summary>
        /// Adds template into the system
        /// </summary>
        /// <param name="package">template zip package</param>
        /// <returns></returns>
        public static IGalleryTemplate AddTemplate(ZipArchivePackage package)
        {
            CuratedGalleryTemplate output = null;

            DirectoryInfo extractdir = null;

            try
            {
                extractdir = ExtractPackage(package, Path.Combine(TemporaryDirectory, Guid.NewGuid().ToString()));

                #region Get package descriptor and validate package

                if (extractdir.GetFiles("*.html.xslt").Count() != 1)
                    throw new Exception("Package does not have entry point file (*.html.xslt)");

                var xfiles = extractdir.GetFiles("*.xml");

                if (xfiles.Length == 0)
                    throw new Exception("Template descriptor was not found");

                if (xfiles.Length != 1)
                    throw new Exception("Template package has several xml files. Template descriptor resolving problem");

                GalleryTemplateDescriptor descriptor = LoadTemplateDescriptor(xfiles[0]);

                #endregion Get package descriptor and validate package

                #region Add template into application storage and make some actions with file system

                //add the template into the app storage
                var template = ObjectMapper.DoMapping<GalleryTemplateInfo>(descriptor);
                template.Package = package;

                OperationResult<OperationResults, GalleryTemplate> rslt = null;

                try
                {
                    rslt = GalleryRuntime.GalleryRepository.AddTemplate(template);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                    throw;
                }

                extractdir.CopyTo(GetTemplatePath(rslt.Output.ID));

                output = new CuratedGalleryTemplate();
                ObjectMapper.DoMapping(descriptor, output);
                ObjectMapper.DoMapping(rslt.Output, output);

                #endregion Add template into application storage
            }
            finally
            {
                if (extractdir != null && extractdir.Exists)
                {
                    try
                    {
                        extractdir.Remove();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteInfo(ex.ToString());
                    }
                }
            }

            //add template into registered template list
            lock (m_TemplatesSyncRoot)
            {
                if (m_Templates == null)
                    m_Templates = LoadTemplates();

                m_Templates.Add(output);
            }

            return output;
        }

        protected static GalleryTemplateDescriptor LoadTemplateDescriptor(FileInfo xfile)
        {
            XmlSerializer xserializer = new XmlSerializer(typeof(GalleryTemplateDescriptor));

            using (FileStream stream = xfile.OpenRead())
            {
                try
                {
                    return xserializer.Deserialize(stream) as GalleryTemplateDescriptor;
                }
                catch
                {
                    throw new Exception("Template descriptor has invalid format");
                }
            }
        }

        /// <summary>
        /// List of all registered templates
        /// </summary>
        private static List<IGalleryTemplate> m_Templates = null;
        private static readonly object m_TemplatesSyncRoot = new object();

        /// <summary>
        /// Gets gallery template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IGalleryTemplate GetTemplate(int id)
        {
            lock (m_TemplatesSyncRoot)
            {
                if (m_Templates == null)
                    m_Templates = LoadTemplates();

                return m_Templates.Where(x => x.ID == id).SingleOrDefault();
            }
        }

        /// <summary>
        /// Gets gallery templates
        /// </summary>
        /// <returns></returns>
        public static IGalleryTemplate[] GetTemplates()
        {
            lock (m_TemplatesSyncRoot)
            {
                if (m_Templates == null)
                    m_Templates = LoadTemplates();

                return m_Templates.ToArray();
            }
        }

        /// <summary>
        /// Loads registered templates
        /// </summary>
        /// <returns></returns>
        protected static List<IGalleryTemplate> LoadTemplates()
        {
            var output = new List<IGalleryTemplate>();

            var root = new DirectoryInfo(TemplateDirectory);

            //TODO: Temporary I delete only folders due to this folders can have Web.config or othe configuration files
            foreach (var item in root.GetDirectories())
                item.Remove();

            var templates = GalleryRepository.GetTemplates().Output;

            foreach (var item in templates)
            {
                var extractdir = new DirectoryInfo(item.GetFolderPath());
                ExtractPackage(item.Package, extractdir.FullName);

                var xfiles = extractdir.GetFiles("*.xml");

                GalleryTemplateDescriptor descriptor = LoadTemplateDescriptor(xfiles[0]);

                var template = new CuratedGalleryTemplate();
                ObjectMapper.DoMapping(descriptor, template);
                ObjectMapper.DoMapping(item, template);

                output.Add(template);
            }

            return output;
        }
        /// <summary>
        /// Deletes Template
        /// </summary>
        /// <param name="id">Template identifier</param>
        public static bool DeleteTemplate(int id)
        {
            OperationResult<OperationResults, Nullable<bool>> rslt = null;

            rslt = GalleryRepository.DeleteTemplate(id);

            switch (rslt.Result)
            {
                case OperationResults.Success:
                    return true;
                case OperationResults.Failure:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}