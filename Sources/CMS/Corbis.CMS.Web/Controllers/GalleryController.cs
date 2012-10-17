using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corbis.CMS.Web.Code;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Controllers
{
    public class GalleryController : CMSControllerBase
    {
        /// <summary>
        /// Gallety index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        #region Create/Edit Gallery

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Gallery(Nullable<int> id)
        {
            CuratedGallery gallery = null;

            if (id.HasValue)
            {
                //TODO: get existing gallery for editing
                throw new NotImplementedException();
            }
            else
            {
                gallery = GalleryRuntime.CreateGallery("New gallery");
            }

            //
            this.ViewBag.GalleryID = gallery.ID;
            return this.View("Gallery");
        }

        /// <summary>
        /// Create/Edit gallery
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Gallery")]
        public ActionResult UpdateGallery()
        {
            //save updates and transform xslt to html

            throw new NotImplementedException();
        }

        #endregion Create/Edit Gallery

    }
}
