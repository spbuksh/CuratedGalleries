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
        OperationResult<OperationResults, object> UpdateGallery(CuratedGallery gallery);
    }
}
