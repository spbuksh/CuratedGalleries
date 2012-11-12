using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Corbis.Common.Utilities.Image.Enums;

namespace Corbis.Common.Utilities.Image
{
    /// <summary>
    /// Class to help with images.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Resize image to provided Size.
        /// </summary>
        /// <param name="imgToResize">Image to resize</param>
        /// <param name="size">Size</param>
        /// <returns>Returns resized BitMap</returns>
        public static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var bitMapImage = new Bitmap(destWidth, destHeight);
            var graphis = Graphics.FromImage(bitMapImage);
            graphis.InterpolationMode = InterpolationMode.HighQualityBicubic;

            graphis.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            graphis.Dispose();

            return bitMapImage;
        }

        /// <summary>
        /// Generates an Image for Questions and Answers template.
        /// </summary>
        /// <param name="quiestion">Question Text</param>
        /// <param name="answer"> Answer Text</param>
        /// <param name="width"> Width in px</param>
        /// <param name="height"> Height in px</param>
        /// <param name="font">Text Font</param>
        /// <param name="filePathToSave">File Path to save</param>
        /// <param name="fileName">FileName</param>
        /// <param name="imageFormat">Image Format</param>
        /// <returns>Saved full Path</returns>
        public static string SaveQaImage(string quiestion, string answer, int width, int height, Font font, string filePathToSave, string fileName , ImageFormat imageFormat)
        {
            var imageWrapper = new Bitmap(width + 1, height + 1);
            var objGraphics = Graphics.FromImage(imageWrapper);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            objGraphics.CompositingQuality = CompositingQuality.HighQuality;
            objGraphics.InterpolationMode = InterpolationMode.High;
            var wrapperRectangle = new Rectangle(new Point(0, 0), new Size(width, height));
            var questionHeight = (int)objGraphics.MeasureString(quiestion, font).Height;
            var questionWidth = (int)objGraphics.MeasureString(quiestion, font).Width;
            var answerHeight = (int)objGraphics.MeasureString(answer, font).Height;
            var answerWidth = (int)objGraphics.MeasureString(answer, font).Width;
            var questionRectangle = new Rectangle(new Point(0, 0), new Size(questionWidth + 1, questionHeight + 1));
            var isHorisontal = height < width;
            objGraphics.FillRectangle(Brushes.White, wrapperRectangle);
            var answerRectangle = isHorisontal ? new Rectangle(new Point(width - answerWidth, 0), new Size(answerWidth + 1, answerHeight + 1)) : new Rectangle(new Point(0, height - answerHeight), new Size(answerWidth + 1, answerHeight + 1));
            objGraphics.DrawString(quiestion, font, new SolidBrush(Color.Black), questionRectangle);
            objGraphics.DrawString(answer, font, new SolidBrush(Color.Black), answerRectangle);
            objGraphics.DrawRectangle(Pens.Black, wrapperRectangle);
            objGraphics.DrawRectangle(Pens.Transparent, questionRectangle);
            objGraphics.DrawRectangle(Pens.Transparent, answerRectangle);
            objGraphics.Flush();
            imageWrapper.Save(Path.Combine(filePathToSave, fileName + "." + imageFormat), imageFormat);

            return Path.GetFullPath(Path.Combine(filePathToSave, fileName + "." + imageFormat));
        }

        /// <summary>
        /// Generates an Image from text.
        /// </summary>
        /// <param name="text">Text to write on image.</param>
        /// <param name="textFont">Font of text.</param>
        /// <param name="filePathToSave">Path to save image. </param>
        /// <param name="fileName">FileName to save. </param>
        /// <param name="textColor">Text color.</param>
        /// <param name="dockStyle">Style to define place of text.</param>
        /// <param name="imageWidth">Image Width.</param>
        /// <param name="imageHeight">Image Height.</param>
        /// <param name="isBorderRequired">If set to true: it will drow a border around an Image. False by default.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="imageFormat"> Image format (PNG by default)</param>
        public static System.Drawing.Image CreateImage(string text, Font textFont, string filePathToSave, string fileName, Color? textColor = null, DockStyle dockStyle = DockStyle.Center, int? imageWidth = null, int? imageHeight = null, bool isBorderRequired = false, Pen borderColor = null, ImageFormat imageFormat = null)
        {
            var generatedImage = new GeneratedImage(text, textFont, filePathToSave, fileName, textColor, dockStyle,
                                                    imageWidth, imageHeight, isBorderRequired, borderColor, imageFormat);
            return generatedImage.DrawImage();
        }

        /// <summary>
        /// Saves image to selected Path and in corresponed format.
        /// </summary>
        /// <param name="text">Text to write on image.</param>
        /// <param name="textFont">Font of text.</param>
        /// <param name="filePathToSave">Path to save image. </param>
        /// <param name="fileName">FileName to save. </param>
        /// <param name="textColor">Text color.</param>
        /// <param name="dockStyle">Style to define place of text.</param>
        /// <param name="imageWidth">Image Width.</param>
        /// <param name="imageHeight">Image Height.</param>
        /// <param name="isBorderRequired">If set to true: it will drow a border around an Image. False by default.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="imageFormat"> Image format (PNG by default)</param>
        /// <returns>Saved image path.</returns>
        public static string SaveImage(string text, Font textFont, string filePathToSave, string fileName, Color? textColor = null, DockStyle dockStyle = DockStyle.Center, int? imageWidth = null, int? imageHeight = null, bool isBorderRequired = false, Pen borderColor = null, ImageFormat imageFormat = null)
        {
            var generatedImage = new GeneratedImage(text, textFont, filePathToSave, fileName, textColor, dockStyle,
                                                    imageWidth, imageHeight, isBorderRequired, borderColor, imageFormat);
            return Path.GetFullPath(generatedImage.SaveImage());
        }

    }
}