using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface;
using Corbis.DataImporter.Interface;
using Corbis.Presentation.Common.RepositoryProvider;
using Ionic.Zip;
using Microsoft.Practices.Unity;

namespace Corbis.DataImporter.Importers
{
    /// <summary>
    /// Class to work with 
    /// </summary>
    public class TemplateImporter : IImporter
    {
        /// <summary>
        /// Imports Data to the System.
        /// </summary>
        public void DoImport()
        {
            var container = new UnityContainer();
            RepositoryProvider.Register(container, "RepositoryProvider");
            var repository = container.Resolve<ICuratedGalleryRepository>();
            var path = ImportHelper.GetPath(ConfigurationManager.AppSettings["TemplatesStorage"]);
            foreach (var file in Directory.EnumerateFiles(path, "*.zip*", SearchOption.AllDirectories))
            {
                var fileinfo = new FileInfo(file);
                using (var stream = fileinfo.OpenRead())
                {
                    using (var zip = ZipFile.Read(stream))
                    {
                        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                        zip.ExtractAll(tempDir);

                        var files = Directory.GetFiles(tempDir, "*.xml*").ToList();
                        if (files.Count == 0 || files.Count > 1)
                        {
                            throw new Exception("There is no Discriptors in a folder!");
                        }

                        var doc = XDocument.Load(files.First());
                        var discriptor = new GalleryTemplateInfo
                                             {
                                                 Author = doc.XPathSelectElement("//*/Author").Value,
                                                 Company = doc.XPathSelectElement("//*/Company").Value,
                                                 Description = doc.XPathSelectElement("//*/Description").Value,
                                                 Name = doc.XPathSelectElement("//*/Name").Value,
                                                 ImagesUploadPath = doc.XPathSelectElement("//*/UploadPath").Value,
                                                 GalleryIcon = new GalleryIcon
                                                                   {
                                                                       GalleryIconType =
                                                                           (GalleryIconType)
                                                                           Enum.Parse(typeof (GalleryIconType),
                                                                                      doc.XPathSelectElement(
                                                                                          "//*/GalleryIconType").Value),
                                                                       Value = doc.XPathSelectElement("//*/Value").Value
                                                                   },
                                                 Package =
                                                     new ZipArchivePackage
                                                         {
                                                             FileName = fileinfo.Name,
                                                             FileContent = File.ReadAllBytes(fileinfo.FullName)
                                                         }
                                             };
                        repository.AddTemplate(discriptor);
                    }
                }
            }
        }
    }
}
