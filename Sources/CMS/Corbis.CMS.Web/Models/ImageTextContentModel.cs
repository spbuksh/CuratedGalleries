using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Corbis.CMS.Entity;
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

        /// <summary>
        /// Background color
        /// </summary>
        public string BackgrouondColor { get; set; }

        public virtual Nullable<int> Width { get; set; }

        public virtual Nullable<int> Height { get; set; }


    }

    public class EmptyTextContentModel : ImageTextContentModelBase
    { }


    public class CustomImageContentModel : ImageTextContentModelBase
    {
        private ImageUrlSet _contentImageUrl;
        private int _galleryID;

        /// <summary>
        /// ctor
        /// </summary>
        public CustomImageContentModel()
            : base(TextContentPositions.Left)
         {
         }

        public string ImageID { get; set; }

        public int GalleryID
        {
            get { return _galleryID; }
            set { _galleryID = value; }
        }

        public string Name { get; set; }

        [Required]
        public override Nullable<int> Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        public ImageUrlSet ContentImageUrl
        {
            get { return _contentImageUrl; }
            set { _contentImageUrl = value; }
        }

        [Required]
        public override Nullable<int> Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
    }

    public class QnATextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public QnATextContentModel()
            : base(TextContentPositions.Left)
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

        [Required]
        public override Nullable<int> Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        [Required]
        public override Nullable<int> Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
    }
    public class PullQuotedTextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PullQuotedTextContentModel()
            : base(TextContentPositions.Center)
        { }

        /// <summary>
        /// Question text
        /// </summary>
        [Required]
        public string Text { get; set; }

        [Required]
        public override Nullable<int> Width
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        [Required]
        public override Nullable<int> Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
    }


    public class BodyCopyTextContentModel : ImageTextContentModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BodyCopyTextContentModel()
            : base(TextContentPositions.Center)
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

        [Required]
        public override Nullable<int> Width
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        [Required]
        public override Nullable<int> Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
    }

}