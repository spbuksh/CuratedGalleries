using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class CreateGalleryModel
    {
        /// <summary>
        /// Gallery name
        /// </summary>
        [Required(ErrorMessage = "Enter gallery name")]
        public string Name { get; set; }

        /// <summary>
        /// Current template identifier
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// Available gallery templates
        /// </summary>
        public List<GalleryTemplateModel> Templates 
        {
            get { return this.m_Templates; } 
        }
        private readonly List<GalleryTemplateModel> m_Templates = new List<GalleryTemplateModel>();
    }
}