using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Entity
{
    public class GalleryPublicationPeriod
    {
        /// <summary>
        /// Publication period identifier
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Start publication period datetime
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End publication period datetime
        /// </summary>
        public Nullable<DateTime> End { get; set; }
    }
}
