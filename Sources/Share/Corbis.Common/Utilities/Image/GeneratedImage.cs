using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Corbis.Common.Utilities.Image.Enums;
using Corbis.Common.Utilities.Image.Interfaces;

namespace Corbis.Common.Utilities.Image
{
    public class GeneratedImage : IGeneratedImage
    {
        #region Fields

        private Graphics _objGraphics;

        private Bitmap _objBmpImage;

        private Rectangle _objRectangle;

        private StringFormat _stringFormat;

        private DockStyle _dockStyle;

        #endregion

        #region Constructor

        /// <summary>
        /// Private Construcor
        /// </summary>
        private GeneratedImage()
        {

        }

        /// <summary>
        /// Class to generate Image
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
        public GeneratedImage(string text, Font textFont, string filePathToSave, string fileName, Color? textColor = null, DockStyle dockStyle = DockStyle.Center, int? imageWidth = null, int? imageHeight = null, bool isBorderRequired = false, Pen borderColor = null, ImageFormat imageFormat = null)
        {
            Initialize();

            this.Text = text;
            this.TextFont = textFont;
            this.TextColor = textColor.HasValue ? textColor.Value : Color.Black;
            this.DockStyle = dockStyle;
            this.ImageWidth = imageWidth.HasValue
                                  ? imageWidth.Value
                                  : (int)_objGraphics.MeasureString(Text, TextFont).Width;
            this.ImageHeight = imageHeight.HasValue ? imageHeight.Value : (int)_objGraphics.MeasureString(Text, TextFont).Height;
            this.BorderColor = borderColor ?? Pens.Black;
            this.IsBorderRequired = isBorderRequired;
            this.FilePathToSave = filePathToSave;
            this.ImageFormat = imageFormat ?? ImageFormat.Png;
            this.FileName = fileName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize base fields.
        /// </summary>
        private void Initialize()
        {
            _objBmpImage = new Bitmap(1, 1);
            _objGraphics = Graphics.FromImage(_objBmpImage);
            _stringFormat = new StringFormat();
        }


        #endregion

        #region Implementation of IGeneratedImage

        /// <summary>
        /// Style to determinate text position relatively to image size.
        /// Center by  default.
        /// </summary>
        public DockStyle DockStyle
        {
            get { return _dockStyle; }
            private set
            {
                _dockStyle = value;
                switch (value)
                {
                    case DockStyle.Left:
                        {
                            _stringFormat.Alignment = StringAlignment.Near;
                            _stringFormat.LineAlignment = StringAlignment.Center;
                            break;
                        }
                    case DockStyle.LeftTop:
                        {

                            _stringFormat.Alignment = StringAlignment.Near;
                            _stringFormat.LineAlignment = StringAlignment.Near;
                            break;
                        }
                    case DockStyle.LeftBottom:
                        {
                            _stringFormat.Alignment = StringAlignment.Near;
                            _stringFormat.LineAlignment = StringAlignment.Far;
                            break;
                        }
                    case DockStyle.RightBottom:
                        {
                            _stringFormat.Alignment = StringAlignment.Far;
                            _stringFormat.LineAlignment = StringAlignment.Far;
                            break;
                        }
                    case DockStyle.RightTop:
                        {
                            _stringFormat.Alignment = StringAlignment.Far;
                            _stringFormat.LineAlignment = StringAlignment.Near;
                            break;
                        }
                    case DockStyle.Right:
                        {
                            _stringFormat.Alignment = StringAlignment.Far;
                            _stringFormat.LineAlignment = StringAlignment.Center;
                            break;
                        }
                    case DockStyle.CenterTop:
                        {
                            _stringFormat.Alignment = StringAlignment.Center;
                            _stringFormat.LineAlignment = StringAlignment.Near;
                            break;
                        }
                    case DockStyle.CenterBottom:
                        {
                            _stringFormat.Alignment = StringAlignment.Center;
                            _stringFormat.LineAlignment = StringAlignment.Far;
                            break;
                        }
                    default:
                        {

                            _stringFormat.Alignment = StringAlignment.Center;
                            _stringFormat.LineAlignment = StringAlignment.Center;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Color to define text color on the image.
        /// </summary>
        public Color? TextColor { get; private set; }

        /// <summary>
        /// Determinates Font of text.
        /// </summary>
        public Font TextFont { get; private set; }

        /// <summary>
        /// Text to display on image.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Image Width. If no set image size will be defined as Font size.
        /// </summary>
        public int? ImageWidth { get; private set; }

        /// <summary>
        /// Image Height. If no set image size will be defined as Font Height.
        /// </summary>
        public int? ImageHeight { get; private set; }

        /// <summary>
        /// Propperty to create a Border around image. False by default.
        /// </summary>
        public bool IsBorderRequired { get; private set; }

        /// <summary>
        /// Border Color. If not set it will be Black.
        /// </summary>
        public Pen BorderColor { get; private set; }

        /// <summary>
        /// Creates a Bitmap Image.
        /// </summary>
        /// <returns> Bitmap image.</returns>
        public Bitmap DrawImage()
        {
            // Create the bmpImage again with the correct size for the text and font.
            _objBmpImage = new Bitmap(_objBmpImage, new Size(ImageWidth.Value, ImageHeight.Value));
            // Add the colors to the new bitmap.
            _objGraphics = Graphics.FromImage(_objBmpImage);
            _objRectangle = new Rectangle(new Point(0, 0), new Size(ImageWidth.Value - 1, ImageHeight.Value - 1));
            _objGraphics.DrawString(Text, TextFont, new SolidBrush(TextColor.Value), _objRectangle, _stringFormat);

            if (IsBorderRequired)
            {
                _objGraphics.DrawRectangle(this.BorderColor, _objRectangle);
            }
            _objGraphics.Flush();

            return (_objBmpImage);
        }

        /// <summary>
        /// Path to save an Image.
        /// </summary>
        public string FilePathToSave { get; private set; }

        /// <summary>
        /// FileName to save.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Image format to Save.
        /// </summary>
        public ImageFormat ImageFormat { get; private set; }

        /// <summary>
        /// Saves image to selected Path and in corresponed format.
        /// </summary>
        /// <returns>Saved image path.</returns>
        public string SaveImage()
        {
            DrawImage().Save(Path.Combine(FilePathToSave, FileName + "." + ImageFormat), ImageFormat);

            return Path.Combine(FilePathToSave, FileName + "." + ImageFormat);
        }

        #endregion
    }
}
