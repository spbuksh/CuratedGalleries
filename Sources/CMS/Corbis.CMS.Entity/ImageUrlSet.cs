using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Set of URLs to the image based on its size category
    /// </summary>
    public class ImageUrlSet
    {
        /// <summary>
        /// Url to the image with large size
        /// </summary>
        public virtual string Large 
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Large))
                {
                    return string.IsNullOrEmpty(this.m_Middle) ? 
                        (string.IsNullOrEmpty(this.m_Small) ? null : this.m_Small) : this.m_Middle;
                }
                return this.m_Large;
            }
            set 
            {
                this.m_Large = value;
            }
        }
        private string m_Large = null;

        /// <summary>
        /// Url to the image with middle size
        /// </summary>
        public virtual string Middle 
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Middle))
                {
                    return string.IsNullOrEmpty(this.Large) ?
                        (string.IsNullOrEmpty(this.m_Small) ? null : this.m_Small) : this.m_Large;
                }
                return this.m_Middle;
            }
            set
            {
                this.m_Middle = value;
            }
        }
        private string m_Middle = null;

        /// <summary>
        /// Url to the image with small size
        /// </summary>
        public virtual string Small
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_Small))
                {
                    return string.IsNullOrEmpty(this.m_Middle) ?
                        (string.IsNullOrEmpty(this.m_Large) ? null : this.m_Large) : this.m_Middle;
                }
                return this.m_Small;
            }
            set
            {
                this.m_Small = value;
            }
        }
        private string m_Small = null;
    }
}
