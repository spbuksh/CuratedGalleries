using System.Drawing;
using System.Drawing.Imaging;
using Corbis.Common.Utilities.Image.Enums;

namespace Corbis.Common.Utilities.Image.Interfaces
{
    /// <summary>
    /// Interface to work with generated Images.
    /// </summary>
    public interface IGeneratedImage
    {
        /// <summary>
        /// Style to determinate text position relatively to image size.
        /// </summary>
        DockStyle DockStyle { get; }

        /// <summary>
        /// Color to define text color on the image.
        /// </summary>
        Color? TextColor { get; }

        /// <summary>
        /// Determinates Font of text.
        /// </summary>
        Font TextFont { get; }

        /// <summary>
        /// Text to display on image.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Image Width. If no set image size will be defined as Font size.
        /// </summary>
        int? ImageWidth { get; }

        /// <summary>
        /// Image Height. If no set image size will be defined as Font Height.
        /// </summary>
        int? ImageHeight { get; }

        /// <summary>
        /// Propperty to create a Border around image. False by default.
        /// </summary>
        bool IsBorderRequired { get; }

        /// <summary>
        /// Border Color. If not set it will be Black.
        /// </summary>
        Pen BorderColor { get; }

        /// <summary>
        /// Creates a Bitmap Image.
        /// </summary>
        /// <returns> Bitmap image.</returns>
        Bitmap DrawImage();

        /// <summary>
        /// Path to save an Image.
        /// </summary>
        string FilePathToSave { get; }

        /// <summary>
        /// FileName to save.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Image format to Save.
        /// </summary>
        ImageFormat ImageFormat { get; }

        /// <summary>
        /// Saves image to selected Path and in corresponed format.
        /// </summary>
        /// <returns>Saved image path.</returns>
        string SaveImage();
    }
}