using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;
using Corbis.Public.Repository;
using Corbis.DB.Linq;

namespace Corbis.CMS.Repository
{
    public partial class CuratedGalleryRepository : RepositoryBase, ICuratedGalleryRepository
    {
        public OperationResult<OperationResults, GalleryTemplate> AddTemplate(GalleryTemplateInfo template)
        {
            var output = new OperationResult<OperationResults, GalleryTemplate>();

            using (var context = this.CreateMainContext())
            {
                try
                {
                    context.Transaction = context.Connection.BeginTransaction();

                    var frec = new FileRecord() { Name = template.Package.FileName, Content = new System.Data.Linq.Binary(template.Package.FileContent) };
                    context.FileRecords.InsertOnSubmit(frec);
                    context.SubmitChanges();


                    var trec = this.ObjectMapper.DoMapping<GalleryTemplateRecord>(template);
                    trec.Archive = frec.ID;
                    trec.DateCreated = DateTime.UtcNow;

                    if (context.GalleryTemplateRecords.Count() == 0)
                    {
                        trec.IsDefault = true;
                    }
                    else if (template.IsDefault)
                    {
                        var dtrec = context.GalleryTemplateRecords.Where(x => x.IsDefault).Single();
                        dtrec.IsDefault = false;
                    }

                    context.GalleryTemplateRecords.InsertOnSubmit(trec);
                    context.SubmitChanges();

                    context.Transaction.Commit();

                    output.Result = OperationResults.Success;
                    output.Output = this.ObjectMapper.DoMapping<GalleryTemplate>(trec);
                    output.Output.PackageID = trec.Archive;
                }
                catch (Exception ex)
                {
                    context.Transaction.Rollback();
                    this.Logger.WriteError(ex, "Gallery template adding error");

#if DEBUG
                    throw;
#else
                    return new OperationResult<OperationResults, GalleryTemplate>() { Result = OperationResults.Failure };
#endif
                }
            }

            return output;
        }

        public OperationResult<OperationResults, object> RemoveTemplate(int templateID)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, List<GalleryTemplate>> GetTemplates(GalleryTemplateFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, GalleryTemplate> GetTemplate(int id, GalleryTemplateContent content = GalleryTemplateContent.All)
        {
            throw new NotImplementedException();
        }

    }
}
