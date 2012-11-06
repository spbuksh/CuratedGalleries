using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace Corbis.CMS.Web.Code
{
    internal class CuratedGalleryTemplate : CMS.Entity.GalleryTemplate, IGalleryTemplate
    {
        public IImageSource Icon { get; set; }

        public ITemplateGallerySettings GallerySettings { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Company { get; set; }

        public string DescriptorFilepath { get; set; }
    }
}