using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Corbis.Common;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.DB.Linq;

namespace Corbis.CMS.Repository
{
    public partial class CuratedGalleryRepository
    {
        public OperationResult<OperationResults, CuratedGallery> CreateGallery(string name, int? templateID = null)
        {
            var rslt = new OperationResult<OperationResults, CuratedGallery>();

            using (var context = this.CreateMainContext())
            {
                var template = templateID.HasValue ? context.GalleryTemplateRecords.Where(x => x.ID == templateID.Value).SingleOrDefault() :
                    context.GalleryTemplateRecords.Where(x => x.IsDefault).SingleOrDefault();

                if (template == null)
                    throw new Exception("Template was not found");

                var gallery = new CuratedGalleryRecord()
                {
                    DateCreated = DateTime.UtcNow,
                    Enabled = true,
                    Name = name,
                    TemplateID = template.ID
                };
                    
                context.CuratedGalleryRecords.InsertOnSubmit(gallery);
                context.SubmitChanges();

                rslt.Result = OperationResults.Success;
                rslt.Output = this.ObjectMapper.DoMapping<CuratedGallery>(gallery);
            }

            return rslt;
        }

        public OperationResult<OperationResults, CuratedGallery> GetGallery(int id, bool includePackage = false)
        {
            using (var context = this.CreateMainContext())
            {
                var record = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                if (record == null)
                    return new OperationResult<OperationResults, CuratedGallery>() { Result = OperationResults.NotFound };

                var rslt = new OperationResult<OperationResults, CuratedGallery>() { Result = OperationResults.Success };
                rslt.Output = this.ObjectMapper.DoMapping<CuratedGallery>(record);

                if (includePackage)
                {
                    var frec = record.FileRecord;
                    rslt.Output.Package = new ZipArchivePackage() { FileName = frec.Name, FileContent = frec.Content.ToArray() };
                }

                return rslt;
            }

            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, List<CuratedGallery>> GetGalleries(CuratedGalleryFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, object> UpdateGallery(CuratedGallery gallery)
        {
            throw new NotImplementedException();
        }
    }
}
