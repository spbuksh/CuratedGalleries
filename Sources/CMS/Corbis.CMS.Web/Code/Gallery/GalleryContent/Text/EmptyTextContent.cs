using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Empty image text content
    /// </summary>
    public class EmptyTextContent : ImageTextContentBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public EmptyTextContent()
        {
            this.ContentType = TextContents.None;
        }

    }
}