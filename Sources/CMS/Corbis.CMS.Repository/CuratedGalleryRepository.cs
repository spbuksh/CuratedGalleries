using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Repository.Interface.Communication;
using Corbis.Common;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Repository
{
    public class CuratedGalleryRepository : ICuratedGalleryRepository
    {
        public OperationResult<OperationResults, GalleryTemplate> AddTemplate(byte[] archive)
        {
            throw new NotImplementedException();
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

        public OperationResult<OperationResults, CuratedGallery> CreateGallery(string name, int? templateID = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, CuratedGallery> GetGallery(int id, CuratedGalleryContent contect = CuratedGalleryContent.All)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, List<CuratedGallery>> GetGalleries(CuratedGalleryFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public OperationResult<OperationResults, object> UpdateGallery(CuratedGallery gallery, CuratedGalleryContent content = CuratedGalleryContent.All)
        {
            throw new NotImplementedException();
        }
    }
}
