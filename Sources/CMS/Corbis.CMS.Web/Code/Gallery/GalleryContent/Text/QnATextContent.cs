using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// 'Question & Answer' image text content
    /// </summary>
    [Serializable]
    public class QnATextContent : ImageTextContentBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public QnATextContent()
        {
            this.ContentType = TextContents.QnA;
        }

        /// <summary>
        /// Question text
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        public string Answer { get; set; }
    }

}
