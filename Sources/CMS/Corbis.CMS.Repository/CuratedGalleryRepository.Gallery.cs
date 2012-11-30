using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Corbis.Common;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.DB.Linq;
using Corbis.Common.ObjectMapping.Interface;
using Corbis.CMS.Repository.Interface;
using Microsoft.Practices.Unity;

namespace Corbis.CMS.Repository
{
    public partial class CuratedGalleryRepository
    {
        [Dependency]
        public IAdminUserRepository UserRepository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
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
                    TemplateID = template.ID,
                    StatusID = (int)CuratedGalleryStatuses.UnPublished
                };
                    
                context.CuratedGalleryRecords.InsertOnSubmit(gallery);
                context.SubmitChanges();

                rslt.Result = OperationResults.Success;
                rslt.Output = this.ObjectMapper.DoMapping<CuratedGallery>(gallery);
            }

            return rslt;
        }

        public OperationResult<OperationResults, object> LockGallery(int id, int userID)
        {
            return this.SetEditor(id, userID);
        }
        public OperationResult<OperationResults, object> UnLockGallery(int id)
        {
            return this.SetEditor(id, null);
        }
        protected OperationResult<OperationResults, object> SetEditor(int id, int? userID)
        { 
            using (var context = this.CreateMainContext())
            {
                var record = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                if (record == null)
                    return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                if (userID.HasValue)
                {
                    if (record.Editor.HasValue && record.Editor != userID)
                        return new OperationResult<OperationResults, object>() { Result = OperationResults.Failure };
                }

                record.Editor = userID;
                context.SubmitChanges();
            }
            return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
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

        protected CuratedGallery Convert(CuratedGalleryRecord record)
        {
            MappingData md = new MappingData(new string[] { Utils.GetPropertyName<CuratedGallery, AdminUserInfo>(x => x.Editor) });
            md.PropertiesMapping.Add(new MappingDataItem(Utils.GetPropertyName<CuratedGalleryRecord, short>(x => x.StatusID), Utils.GetPropertyName<CuratedGallery, CuratedGalleryStatuses>(x => x.Status), x => x.ToEnum<CuratedGalleryStatuses>()));
            md.PropertiesMapping.Add(new MappingDataItem(Utils.GetPropertyName<CuratedGalleryRecord, DateTime>(x => x.DateCreated), null, x => ((DateTime)x).ToLocalTime()));
            md.PropertiesMapping.Add(new MappingDataItem(Utils.GetPropertyName<CuratedGalleryRecord, DateTime?>(x => x.DateModified), null, x => (x == null ? (DateTime?)null : ((DateTime)x).ToLocalTime())));
            return this.ObjectMapper.DoMapping<CuratedGallery>(record, md);
        }

        public OperationResult<OperationResults, CuratedGallery> GetGallery(int id, bool includePackage = false)
        {
            using (var context = this.CreateMainContext())
            {
                var record = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                if (record == null)
                    return new OperationResult<OperationResults, CuratedGallery>() { Result = OperationResults.NotFound };

                var rslt = new OperationResult<OperationResults, CuratedGallery>() { Result = OperationResults.Success };

                rslt.Output = this.Convert(record);

                var period = record.GalleryPublicationPeriodRecords.SingleOrDefault();
                rslt.Output.PublicationPeriod = period == null ? null : this.ObjectMapper.DoMapping<GalleryPublicationPeriod>(period);

                if (record.Editor.HasValue)
                    rslt.Output.Editor = this.UserRepository.GetUserInfo(record.Editor.Value).Output;

                if (period != null && period.PublisherID.HasValue)
                    rslt.Output.Publisher = this.UserRepository.GetUserInfo(period.PublisherID.Value).Output;

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
            //!!! TODO: refactor this method!!! I imaplemented quite fast and it has performance issue and logic issues which I skipped due to some reasons !!!

            var rslt = new OperationResult<OperationResults, List<CuratedGallery>>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            { 
                var query1 = 
                    from p in context.GalleryPublicationPeriodRecords
                    join m in context.AdminUserMembershipRecords on p.PublisherID equals m.ID into set
                    select new { period = p, publisher = set == null ? null : set.FirstOrDefault() };

                var query = from g in context.CuratedGalleryRecords
                         join e in context.AdminUserMembershipRecords on g.Editor equals e.ID into editors
                         join p in query1 on g.ID equals p.period.GalleryID into periods
                            select new { gallery = g, period = (periods == null ? null : periods.First().period), publisher = (periods == null ? null : periods.First().publisher), editor = (editors == null ? null : editors.First()) };

                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.NamePattern))
                        query = query.Where(x => x.gallery.Name.ToLower().Contains(filter.NamePattern.ToLower()));

                    if (filter.Enabled.HasValue)
                        query = query.Where(x => x.gallery.Enabled == filter.Enabled.Value);

                    if (filter.Status.HasValue)
                        query = query.Where(x => x.gallery.StatusID == (short)filter.Status.Value);

                    if (filter.PublicationPeriodFrom.HasValue)
                        query = query.Where(x => x.period != null && (x.period.Start <= filter.PublicationPeriodFrom.Value && (!x.period.End.HasValue || filter.PublicationPeriodFrom.Value < x.period.End.Value)));

                    if (filter.PublisherID.HasValue)
                        query = query.Where(x => x.publisher != null && x.publisher.ID == filter.PublisherID.Value);
                }

                query = query.OrderByDescending(x => x.gallery.DateCreated).ThenBy(x => x.gallery.Name);

                if (filter != null)
                {
                    query = query.Skip(filter.StartIndex);

                    if (filter.Count.HasValue)
                        query = query.Take(filter.Count.Value);
                }

                rslt.Output = new List<CuratedGallery>();

                foreach (var item in query)
                {
                    var gallery = this.Convert(item.gallery);
                    gallery.PublicationPeriod = item.period == null ? null : this.ObjectMapper.DoMapping<GalleryPublicationPeriod>(item.period);

                    gallery.Editor = item.editor == null ? null : this.UserRepository.GetUserInfo(item.editor.ID).Output;
                    gallery.Publisher = item.publisher == null ? null : this.UserRepository.GetUserInfo(item.publisher.ID).Output;

                    rslt.Output.Add(gallery);
                }
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

        /// <summary>
        /// Publishes/republishes gallery
        /// </summary>
        /// <param name="userID">Gallery publisher</param>
        /// <param name="id">Unique gallery identifier</param>
        /// <param name="dtStartUTC">Publication start date</param>
        /// <param name="dtEnd">Publication end date</param>
        /// <returns></returns>
        public OperationResult<OperationResults, GalleryPublicationPeriod> Publish(int? userID, int id, DateTime dtStartUTC, DateTime? dtEndUTC)
        {
            using (var context = this.CreateMainContext())
            {
                var galleryRec = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                if (galleryRec == null)
                    return new OperationResult<OperationResults, GalleryPublicationPeriod>() { Result = OperationResults.NotFound };

                if (galleryRec.StatusID == (int)CuratedGalleryStatuses.Published)
                    return new OperationResult<OperationResults, GalleryPublicationPeriod>() { Result = OperationResults.Failure };

                foreach (var item in context.GalleryPublicationPeriodRecords.Where(x => x.GalleryID == id).ToArray())
                    context.GalleryPublicationPeriodRecords.DeleteOnSubmit(item);

                var period = new GalleryPublicationPeriodRecord() { GalleryID = id, Start = dtStartUTC, End = dtEndUTC, PublisherID = userID };
                context.GalleryPublicationPeriodRecords.InsertOnSubmit(period);

                galleryRec.Editor = null;
                galleryRec.StatusID = (int)CuratedGalleryStatuses.Published;

                context.SubmitChanges();

                return new OperationResult<OperationResults, GalleryPublicationPeriod>() { Result = OperationResults.Success, Output = this.ObjectMapper.DoMapping<GalleryPublicationPeriod>(period) };
            }
        }

        /// <summary>
        /// Un-Publishes gallery
        /// </summary>
        /// <param name="id">Unique gallery identifier</param>
        /// <returns></returns>
        public OperationResult<OperationResults, object> UnPublish(int id)
        {
            using (var context = this.CreateMainContext())
            {
                var galleryRec = context.CuratedGalleryRecords.Where(x => x.ID == id).SingleOrDefault();

                if (galleryRec == null)
                    return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                if (galleryRec.StatusID == (int)CuratedGalleryStatuses.UnPublished)
                    return new OperationResult<OperationResults, object>() { Result = OperationResults.Failure };

                foreach (var item in context.GalleryPublicationPeriodRecords.Where(x => x.GalleryID == id).ToArray())
                    context.GalleryPublicationPeriodRecords.DeleteOnSubmit(item);

                galleryRec.StatusID = (int)CuratedGalleryStatuses.UnPublished;

                context.SubmitChanges();

                return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
            } 
        }
    }
}
