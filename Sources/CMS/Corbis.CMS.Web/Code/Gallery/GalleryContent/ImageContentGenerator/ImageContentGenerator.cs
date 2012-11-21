using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Corbis.Common.Utilities.Image;
using Corbis.Common.Utilities.Image.Enums;

namespace Corbis.CMS.Web.Code.Gallery.GalleryContent.ImageContentGenerator
{
    public  static class ImageContentGenerator
    {
        public static string GererateImage(GalleryContentImage image,int galleryId, Font galleryFont)
        {
            galleryFont = new Font(galleryFont.FontFamily, galleryFont.Size + 10);
            var text = string.Empty;
            var textColor = Color.Black;
            switch (image.TextContent.ContentType)
            {
                    case TextContents.QnA:
                    {
                        var result = image.TextContent as QnATextContent;

                        return ImageHelper.SaveQaImage(result.Question, result.Answer, result.Size.Value.Width,
                                                       result.Size.Value.Height, galleryFont,
                                                       GalleryRuntime.GetGalleryContentPath(galleryId),
                                                       Path.GetFileNameWithoutExtension(image.Name), ImageFormat.Png);
                    }
                case TextContents.Pullquote:
                    {
                        var result = image.TextContent as PullQuotedTextContent;
                        textColor = Color.White;
                        text = result.Text;
                        break;
                    }
                    case TextContents.BodyCopy:
                    {
                        var result = image.TextContent as BodyCopyTextContent;
                        var builder = new StringBuilder();
                        builder.Append(result.DropCap);
                        for (var i = 0; i < 4; i++)
                            builder.AppendLine();
                        builder.Append(result.BodyCopy);
                        text = builder.ToString();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

           

            var dock = DockStyle.Center;
            switch (image.TextContent.Position)
            {
                case TextContentPositions.Left:
                    {
                        dock = DockStyle.Left;
                        break;
                       
                    }
                    case TextContentPositions.Right:
                    {
                        dock = DockStyle.Right;
                        break;
                    }
                    case TextContentPositions.Top:
                    {
                        dock = DockStyle.CenterTop;
                        break;
                    }
                    case TextContentPositions.Bottom:
                    {
                        dock = DockStyle.CenterBottom;
                        break;
                    }
                default:
                    {
                        dock = DockStyle.Center;
                        break;
                    }
            }


            return ImageHelper.SaveImage(text, galleryFont, GalleryRuntime.GetGalleryContentPath(galleryId), Path.GetFileNameWithoutExtension(image.Name), textColor, dock, 400, 500,
                                  false, null, ImageFormat.Png);
        }
    }
}