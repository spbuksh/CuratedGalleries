using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{
    public class CustomImageTextContent: ImageTextContentBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public CustomImageTextContent()
        {
            this.ContentType = TextContents.CustomImage;
        }
        

    }

}