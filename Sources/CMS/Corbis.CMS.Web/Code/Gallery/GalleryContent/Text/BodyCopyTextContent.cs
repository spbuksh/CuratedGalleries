using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.CMS.Web.Code
{
    public class BodyCopyTextContent : ImageTextContentBase
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public BodyCopyTextContent()
            : base()
        {
            this.ContentType = TextContents.BodyCopy;
        }

        /// <summary>
        /// Drop cap of the text content
        /// </summary>
        public string DropCap { get; set; }

        /// <summary>
        /// Body copy
        /// </summary>
        public string BodyCopy { get; set; }
    }
}