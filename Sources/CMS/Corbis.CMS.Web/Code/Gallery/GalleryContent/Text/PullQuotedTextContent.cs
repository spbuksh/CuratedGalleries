using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Pull quoted text
    /// </summary>
    [Serializable]
    public class PullQuotedTextContent : ImageTextContentBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PullQuotedTextContent()
        {
            this.ContentType = TextContents.Pullquote;
        }

        /// <summary>
        /// Pullquoted text
        /// </summary>
        public string Text { get; set; }
    }
}
