using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Common;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;

namespace Corbis.CMS.Repository.Interface
{
    public partial interface ICuratedGalleryRepository
    {
        /// <summary>
        /// Creates curated gallery
        /// </summary>
        /// <param name="name">Gallery name</param>
        /// <param name="templateID">Template identifier. If it is null then default template will be used</param>
        /// <returns></returns>
        OperationResult<OperationResults, CuratedGallery> CreateGallery(string name, Nullable<int> templateID = null);

        /// <summary>
        /// Set exclusive gallery lock. Ensure that only one user can edit this gallery at the same time 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID">user</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> LockGallery(int id, int userID);

        /// <summary>
        /// Unlocks gallery
        /// </summary>
        /// <param name="id">Unique gallery identifier</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> UnLockGallery(int id);

        /// <summary>
        /// Creates curated gallery
        /// </summary>
        /// <param name="id">Curated gallery unique identifier</param>
        /// <param name="includePackage"></param>
        /// <returns></returns>
        OperationResult<OperationResults, CuratedGallery> GetGallery(int id, bool includePackage = false);

        /// <summary>
        /// Creates curated gallery
        /// </summary>
        /// <param name="id">Curated gallery unique identifier</param>
        /// <param name="includePackage"></param>
        /// <returns></returns>
        OperationResult<OperationResults, Nullable<bool>> DeleteGallery(int id);

        /// <summary>
        /// Gets filtered list of curated galleries
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OperationResult<OperationResults, List<CuratedGallery>> GetGalleries(CuratedGalleryFilter filter = null);

        /// <summary>
        /// Updates curated gallery
        /// </summary>
        /// <param name="gallery">Gallery object</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> UpdateGallery(CuratedGallery gallery, bool includePackage = false);

        /// <summary>
        /// Publishes gallery
        /// </summary>
        /// <param name="id">Unique gallery identifier</param>
        /// <param name="dtStart">Publication start date in UTC</param>
        /// <param name="dtEnd">Publication end date in UTC</param>
        /// <returns></returns>
        OperationResult<OperationResults, GalleryPublicationPeriod> Publish(int id, DateTime dtStartUTC, DateTime? dtEndUTC);

        /// <summary>
        /// Un-Publishes gallery
        /// </summary>
        /// <param name="id">Unique gallery identifier</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> UnPublish(int id);

    }
}
