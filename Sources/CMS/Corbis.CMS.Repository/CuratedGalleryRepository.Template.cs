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
                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();

                    var frec = new FileRecord() { Name = template.Package.FileName, Content = new System.Data.Linq.Binary(template.Package.FileContent) };
                    context.FileRecords.InsertOnSubmit(frec);
                    context.SubmitChanges();


                    var trec = this.ObjectMapper.DoMapping<GalleryTemplateRecord>(template);
                    trec.PackageID = frec.ID;
                    trec.DateCreated = DateTime.UtcNow;

                    if (!context.GalleryTemplateRecords.Any())
                    {
                        trec.IsDefault = true;
                    }
                    else if (template.IsDefault)
                    {
                        var dtrec = context.GalleryTemplateRecords.Single(x => x.IsDefault);
                        dtrec.IsDefault = false;
                    }

                    context.GalleryTemplateRecords.InsertOnSubmit(trec);
                    context.SubmitChanges();

                    context.Transaction.Commit();

                    output.Result = OperationResults.Success;
                    output.Output = this.ObjectMapper.DoMapping<GalleryTemplate>(trec);
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
            using (var context = this.CreateMainContext())
            {
                if (filter == null)
                {
                    var query = from t in context.GalleryTemplateRecords
                                join f in context.FileRecords on t.PackageID equals f.ID
                                select new { Template = this.ObjectMapper.DoMapping<GalleryTemplate>(t), Package = new ZipArchivePackage() { FileName = f.Name, FileContent = f.Content.ToArray() } };

                    List<GalleryTemplate> templates = new List<GalleryTemplate>();

                    foreach (var item in query.ToArray())
                    {
                        item.Template.Package = item.Package;
                        templates.Add(item.Template);
                    }

                    return new OperationResult<OperationResults, List<GalleryTemplate>>() { Result = OperationResults.Success, Output = templates };
                }
            }

            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, GalleryTemplate> GetTemplate(int id, GalleryTemplateContent content = GalleryTemplateContent.All)
        {
            throw new NotImplementedException();
        }

    }
}
