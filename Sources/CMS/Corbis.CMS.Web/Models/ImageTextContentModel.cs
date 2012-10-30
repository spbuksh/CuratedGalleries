using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Corbis.CMS.Web.Code;

namespace Corbis.CMS.Web.Models
{
    public abstract class ImageTextContentModelBase
    {
        protected ImageTextContentModelBase()
        { }
        protected ImageTextContentModelBase(TextContentPositions position) : this()
        {
            this.Position = position;
        }

        /// <summary>
        /// Text position
        /// </summary>
        [Required(ErrorMessage = "Text position is not set")]
        public Nullable<TextContentPositions> Position { get; set; }
    }

    public class EmptyTextContentModel : ImageTextContentModelBase
    { }

    public class QnATextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public QnATextContentModel() : base(TextContentPositions.Left)
        { }

        /// <summary>
        /// Question text
        /// </summary>
        [Required]
        public string Question { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        [Required]
        public string Answer { get; set; }
    }
    public class PullQuotedTextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PullQuotedTextContentModel() : base(TextContentPositions.Center)
        { }

        /// <summary>
        /// Question text
        /// </summary>
        [Required]
        public string Text { get; set; }
    }


    public class BodyCopyTextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BodyCopyTextContentModel() : base(TextContentPositions.Center)
        { }

        /// <summary>
        /// Drop cap of the text content
        /// </summary>
        [Required]
        public string DropCap { get; set; }

        /// <summary>
        /// Body copy
        /// </summary>
        [Required]
        public string BodyCopy { get; set; }
    }

}