using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Common;
using Ionic.Zip;
using System.IO;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {
        public OperationResult<OperationResults, GalleryTemplate> AddTemplate(string filename, byte[] package)
        {
            //
            using (var stream = new MemoryStream(package))
            {
                using (var zip = ZipFile.Read(stream))
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                    zip.ExtractAll(tempDir);

                    //read template descriptor into 
                    var template = new GalleryTemplateInfo() { Package = new ZipArchivePackage() { FileName = filename, FileContent = package } };


                    //
                    var ares = GalleryRuntime.GalleryRepository.AddTemplate(template);


                    string tpath = Path.Combine(GalleryRuntime.TemplateDirectory, ares.Output.ID.ToString());

                    //move files from path to tpath

                }
            }


            throw new NotImplementedException();
        }

    }
}