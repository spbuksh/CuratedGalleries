using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Corbis.DataImporter
{
    public static class ImportHelper
    {
        public static string GetPath(string path)
        {
            return Path.IsPathRooted(path) ? path :
             Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.TrimStart(Path.DirectorySeparatorChar));
        }
    }
}
