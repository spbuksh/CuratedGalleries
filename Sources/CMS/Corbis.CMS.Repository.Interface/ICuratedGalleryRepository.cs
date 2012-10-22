using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Common;
using Corbis.CMS.Entity;
using Corbis.CMS.Repository.Interface.Communication;

namespace Corbis.CMS.Repository.Interface
{
    public interface ICuratedGalleryRepository
    {
        #region Template Management

        /// <summary>
        /// Registers template in the system
        /// </summary>
        /// <param name="template">Creating template details</param>
        /// <returns></returns>
        OperationResult<OperationResults, GalleryTemplate> AddTemplate(GalleryTemplateInfo template);

        /// <summary>
        /// Removes template from the system
        /// </summary>
        /// <param name="templateID">Removed template identifier</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> RemoveTemplate(int templateID);

        /// <summary>
        /// Gets templates based on filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OperationResult<OperationResults, List<GalleryTemplate>> GetTemplates(GalleryTemplateFilter filter = null);

        /// <summary>
        /// Gets templates based on filter
        /// </summary>
        /// <param name="id">Template identifier</param>
        /// <param name="content">Describes retriving content</param>
        /// <returns></returns>
        OperationResult<OperationResults, GalleryTemplate> GetTemplate(int id, GalleryTemplateContent content = GalleryTemplateContent.All);


        #endregion Template Management

        #region Curated gallery management

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

        #endregion Curated gallery management

    }
}
