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