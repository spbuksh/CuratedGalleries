using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Common;
using Corbis.CMS.Entity;
using System.IO;

namespace Corbis.CMS.Web.Code
{
    public partial class GalleryRuntime
    {

        public CuratedGallery CreateGallery(string name, Nullable<int> templateID = null)
        {
            //var rslt = this.GalleryRepository.CreateGallery(name, templateID);

            //System.IO.Directory.CreateDirectory(Path.Combine(this.GalleryDirectory, rslt.Output.ID.ToString())
            //rslt.Output.TemplateID


            throw new NotImplementedException();
        }
    }
}