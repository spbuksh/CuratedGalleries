using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.DB.Linq
{
    public partial class GalleryTemplateRecord
    {
        #region Properties

        /// <summary>
        /// Flag that indicates if record can be modified or not.
        /// </summary>
        public bool CanModify
        {
            get { return !this.CuratedGalleryRecords.Any(); }
        }

        #endregion
    }
}
