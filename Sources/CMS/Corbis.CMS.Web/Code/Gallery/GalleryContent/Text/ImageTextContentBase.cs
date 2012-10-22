using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Corbis.CMS.Web.Code
{
    /// <summary>
    /// Possible image text content types
    /// </summary>
    [Serializable]
    public enum TextContents
    {
        /// <summary>
        /// Do not use any text content for gallery image
        /// </summary>
        None,
        /// <summary>
        /// "Question and Answer"
        /// </summary>
        QnA,
        /// <summary>
        /// Pull quoted text
        /// </summary>
        Pullquote,
    }
    /// <summary>
    /// Text content position
    /// </summary>
    [Serializable]
    public enum TextContentPositions
    {
        /// <summary>
        /// Text stands at left part of the refult displayed gallery image
        /// </summary>
        Left,
        /// <summary>
        /// Text stands at right part of the refult displayed gallery image
        /// </summary>
        Right,
        /// <summary>
        /// Text stands at top part of the refult displayed gallery image
        /// </summary>
        Top,
        /// <summary>
        /// Text stands at bottom part of the refult displayed gallery image
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Base class for any image text content. Based on this content png file will be generated
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(QnATextContent))]
    [XmlInclude(typeof(PullQuotedTextContent))]
    public abstract class ImageTextContentBase
    {
        /// <summary>
        /// Defines text content type
        /// </summary>
        public TextContents ContentType { get; set; }

        /// <summary>
        /// Text position on the gallery image
        /// </summary>
        public TextContentPositions Position { get; set; }
    }

}
