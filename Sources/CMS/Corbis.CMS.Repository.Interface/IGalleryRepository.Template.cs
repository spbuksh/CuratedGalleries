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
        /// <param name="filter">If it is null then full template data will be returned</param>
        /// <returns></returns>
        OperationResult<OperationResults, List<GalleryTemplate>> GetTemplates(GalleryTemplateFilter filter = null);

        /// <summary>
        /// Gets templates based on filter
        /// </summary>
        /// <param name="id">Template identifier</param>
        /// <param name="content">Describes retriving content</param>
        /// <returns></returns>
        OperationResult<OperationResults, GalleryTemplate> GetTemplate(int id, GalleryTemplateContent content = GalleryTemplateContent.All);

        /// <summary>
        /// Deletes curated gallery template
        /// </summary>
        /// <param name="id">Curated gallery template unique identifier</param>
        /// <returns></returns>
        OperationResult<OperationResults, Nullable<bool>> DeleteTemplate(int id);

    }
}
