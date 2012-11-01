using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Corbis.Common;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.DB.Linq;
using Corbis.Common.ObjectMapping.Interface;

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

        public OperationResult<OperationResults, Nullable<bool>> DeleteGallery(int id)
        {
            using (var context = this.CreateMainContext())
            {
                if (context.Connection.State != System.Data.ConnectionState.Open)
                    context.Connection.Open();

                context.Transaction = context.Connection.BeginTransaction();

                try
                {
                    var record = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                    if (record != null)
                    {
                        var packageID = record.Archive;

                        context.CuratedGalleryRecords.DeleteOnSubmit(record);
                        context.SubmitChanges();

                        if(packageID.HasValue)
                        {
                            var file = context.FileRecords.Where(x => x.ID == packageID.Value).Single();
                            context.FileRecords.DeleteOnSubmit(file);
                            context.SubmitChanges();
                        }
                    }
                    else
                    {
                        return new OperationResult<OperationResults, bool?>() { Result = OperationResults.NotFound };
                    }

                    context.Transaction.Commit();
                    return new OperationResult<OperationResults, bool?>() { Result = OperationResults.Success, Output = true };
                }
                catch (Exception ex)
                {
                    this.Logger.WriteError(ex);
                    context.Transaction.Rollback();
#if DEBUG
                    throw;
#else
                    return new OperationResult<OperationResults, bool?>() { Result = OperationResults.Failure };
#endif
                }

                throw new NotImplementedException();
            }
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
            var rslt = new OperationResult<OperationResults, List<CuratedGallery>>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            { 
                IQueryable<CuratedGalleryRecord> query = context.CuratedGalleryRecords;

                if (filter != null)
                {
                    //
                    if (!string.IsNullOrEmpty(filter.NamePattern))
                        query = query.Where(x => x.Name.ToLower().Contains(filter.NamePattern.ToLower()));

                    if (filter.Enabled.HasValue)
                        query = query.Where(x => x.Enabled == filter.Enabled.Value);

                    //default sorting
                    query = query.OrderByDescending(x => x.DateCreated).ThenBy(x => x.Name);

                    //point gallery portion at end of query building
                    query = query.Skip(filter.StartIndex);

                    if (filter.Count.HasValue)
                        query.Take(filter.Count.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.DateCreated).ThenBy(x => x.Name);
                }

                //it converts datetime from UTC to local
                ActionHandler<object, object> dtHandler = delegate(object from) { return from == null ? (DateTime?)null : ((DateTime)from).ToLocalTime(); };

                MappingData md = new MappingData();
                md.PropertiesMapping.Add(new MappingDataItem(Utils.GetPropertyName<CuratedGalleryRecord, DateTime>(x => x.DateCreated), null, x => ((DateTime)x).ToLocalTime()));
                md.PropertiesMapping.Add(new MappingDataItem(Utils.GetPropertyName<CuratedGalleryRecord, DateTime?>(x => x.DateModified), null, x => (x == null ? (DateTime?)null : ((DateTime)x).ToLocalTime())));

                rslt.Output = query.Select(x => this.ObjectMapper.DoMapping<CuratedGallery>(x, md)).ToList();
            }

            return rslt;
        }

        public OperationResult<OperationResults, object> UpdateGallery(CuratedGallery gallery, bool includePackage = false)
        {
            using (var context = this.CreateMainContext())
            {
                if (context.Connection.State != System.Data.ConnectionState.Open)
                    context.Connection.Open();

                try
                {
                    context.Transaction = context.Connection.BeginTransaction();

                    var grec = context.CuratedGalleryRecords.Where(x => x.ID == gallery.ID).SingleOrDefault();

                    if (grec == null)
                        return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                    this.ObjectMapper.DoMapping(gallery, grec, new MappingData(new string[] { "ID", "DateCreated", "Package" }));

                    if (includePackage)
                    {
                        var frec = context.FileRecords.Where(x => x.ID == grec.Archive).Single();
                        frec.Content = new System.Data.Linq.Binary(gallery.Package.FileContent);
                        frec.Name = gallery.Package.FileName;
                    }

                    context.SubmitChanges();

                    context.Transaction.Commit();
                }
                catch(Exception ex)
                {
                    if (context.Transaction != null)
                        context.Transaction.Rollback();

                    this.Logger.WriteError(ex);

                    throw;
                }
            }

            return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
        }
    }
}
