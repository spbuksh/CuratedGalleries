using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Presentation.Common.Models
{
    /// <summary>
    /// Image selection modes
    /// </summary>
    public enum GalleryImageSelectionModes
    {
        /// <summary>
        /// Selection is forbidden. Only preview mode
        /// </summary>
        None,
        /// <summary>
        /// Single image selection
        /// </summary>
        Single,
        /// <summary>
        /// Multiple image selection
        /// </summary>
        Multiple
    }

    /// <summary>
    /// Image gallery model
    /// </summary>
    public class CorbisImageGalleryModel
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public CorbisImageGalleryModel()
        {
            //by default use image preview only
            this.SelectionMode = GalleryImageSelectionModes.None;
        }

        /// <summary>
        /// Image selection mode
        /// </summary>
        public GalleryImageSelectionModes SelectionMode { get; set; }

        /// <summary>
        /// Images
        /// </summary>
        public List<CorbisImageModel> Images
        {
            get { return this.m_Images; }
        }
        private readonly List<CorbisImageModel> m_Images = new List<CorbisImageModel>();
    }
}
