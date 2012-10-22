﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    [Serializable, DataContract]
    public class GalleryTemplateInfo
    {
        /// <summary>
        /// ctor
        /// </summary>
        public GalleryTemplateInfo()
        {
            this.Enabled = true;
            this.IsDefault = false;
        }

        /// <summary>
        /// Gallery Template Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gallery Template Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gallery Template Images.
        /// </summary>
        public string ImagesUploadPath { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery template image url. It is "face" of the gallery
        /// </summary>
        public GalleryIcon GalleryIcon { get; set; }

        /// <summary>
        /// Gallery template description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is gallery active or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Is template default or not
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// It is zip archive of the template package
        /// </summary>
        public ZipArchivePackage Package { get; set; }
    }
}
