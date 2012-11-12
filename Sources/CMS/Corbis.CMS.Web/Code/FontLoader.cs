using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Drawing;
using System.Configuration;
using System.IO;

namespace Corbis.CMS.Web.Code
{
    public static class FontLoader
    {
        //[DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        //public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
        //                                 string lpFileName);

        //[DllImport("gdi32.dll", EntryPoint = "RemoveFontResourceW", SetLastError = true)]
        //public static extern int RemoveFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
        //                                    string lpFileName);

        /// <summary>
        /// 
        /// </summary>
        public static PrivateFontCollection LoadFonts()
        {
            string root = ConfigurationManager.AppSettings["fontDirPath"];

            if (string.IsNullOrEmpty(root))
            {
                root = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                if (!Path.IsPathRooted(root))
                    root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, root.TrimStart(Path.DirectorySeparatorChar));
            }

            PrivateFontCollection fonts = new PrivateFontCollection();
            LoadFonts(fonts, root, true);
            return fonts;
        }

        private static void LoadFonts(PrivateFontCollection fonts, string folderPath, bool recursive = true)
        {
            if (!Directory.Exists(folderPath))
                return;

            foreach (var item in Directory.GetFiles(folderPath, "*.*tf"))
            {
                string ext = Path.GetExtension(item);

                if (string.IsNullOrEmpty(ext) || ext.Length != 4)
                    continue;

                try
                {
                    fonts.AddFontFile(item);
                }
                catch(Exception ex)
                {
                    Logging.LogManagerProvider.Instance.WriteError(ex, string.Format("Font '{0}' can not be loaded", item));
                }
            }

            if (!recursive)
                return;

            foreach (var subdir in Directory.GetDirectories(folderPath))
                LoadFonts(fonts, subdir, recursive);
        }

    }
}