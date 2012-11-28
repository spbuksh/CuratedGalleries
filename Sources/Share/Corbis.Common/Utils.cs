using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web;

namespace Corbis.Common
{
    public static class Utils
    {
        /// <summary>
        /// Clones object using binary fomatter. Take into account that all classes must be marked with [Serializable] attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T Clone<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, item);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Gets property name by lambda expression. Use it instead of property name hardcodes (error occurs during building)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="expression">Lambda expression like [(PmtrType x) => x.Property]</param>
        /// <returns></returns>
        public static string GetPropertyName<T, V>(System.Linq.Expressions.Expression<Func<T, V>> expression)
        {
            var mexpr = (expression.Body as System.Linq.Expressions.MemberExpression);

            if (mexpr == null)
                throw new ArgumentException("Parameters does not contain valid expression to get member");

            return mexpr.Member.Name;
        }

        /// <summary>
        /// Copy all directory content recursively with files and sub directories
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="filter">This logic filters files and folders. string input parameter is fullpath, second boolean parameter indicates if it folder or file(true if folder).</param>
        public static void DirectoryCopy(DirectoryInfo from, DirectoryInfo to, ActionHandler<string, bool, bool> filter = null)
        {
            if (!to.Exists)
                to.Create();

            foreach (var fi in from.GetFiles())
            {
                if (filter != null)
                {
                    if(!filter(fi.FullName, false))
                        continue;
                }

                fi.CopyTo(Path.Combine(to.ToString(), fi.Name), true);
            }

            foreach (var subdir in from.GetDirectories())
            {
                if (filter != null)
                {
                    if (!filter(subdir.FullName, true))
                        continue;
                }

                DirectoryCopy(subdir, to.CreateSubdirectory(subdir.Name), filter);
            }
        }

        public static string AbsoluteToVirtual(string absolutePath, HttpContextBase context = null, bool vdirProcess = true)
        {
            context = (context == null) ? new HttpContextWrapper(HttpContext.Current) : context;
            string appdir = context.Server.MapPath("~/");
            string url = absolutePath.Substring(appdir.Length).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!url.StartsWith("/")) url = "/" + url;

            if (vdirProcess)
            {
                string vd = System.Configuration.ConfigurationManager.AppSettings["virtualDirPath"];

                if (!string.IsNullOrEmpty(vd))
                    url = string.Format("/{0}{1}", vd.Trim('/').Trim('\\').Replace('\\', '/'), url);
            }

            return url;
        }
        public static string AbsoluteToVirtual(string absolutePath, HttpContext context = null)
        {
            var cntx = new HttpContextWrapper((context == null) ? HttpContext.Current : context);
            return AbsoluteToVirtual(absolutePath, cntx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vpath"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string VirtualToAbsolute(string vpath, HttpContextBase context = null)
        {
            context = (context == null) ? new HttpContextWrapper(HttpContext.Current) : context;
            return context.Server.MapPath(vpath);
        }
        public static string VirtualToAbsolute(string vpath, HttpContext context = null)
        {
            var cntx = new HttpContextWrapper((context == null) ? HttpContext.Current : context);
            return VirtualToAbsolute(vpath, cntx);
        }


        public static void DirectoryClear(DirectoryInfo dir)
        {
            if (!dir.Exists)
                return;

            foreach (var file in dir.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
                file.Delete();
            }

            dir.Refresh();

            foreach (var child in dir.GetDirectories())
            {
                DirectoryClear(child);

                child.Refresh();

                foreach (var item in child.GetDirectories())
                {
                    item.Refresh();

                    if (Directory.Exists(item.FullName))
                        Directory.Delete(item.FullName, true);
                }

                child.Refresh();

                child.Delete();
            }

            dir.Refresh();
        }

        /// <summary>
        /// Generates file path for duplicate
        /// </summary>
        /// <param name="filepath">Absolute path for created file</param>
        /// <returns></returns>
        public static string GenerateFilePathForDuplicate(string filepath)
        {
            if (!File.Exists(filepath))
                return filepath;

            string filename = Path.GetFileName(filepath);

            string dirpath = Path.GetDirectoryName(filepath);

            int index = filename.LastIndexOf('.');

            string nameonly = index < 0 ? filename : filename.Substring(0, index);
            string ext = filename.Substring(nameonly.Length);

            List<int> postfixes = new List<int>();

            int findx = 0;

            foreach (var item in Directory.GetFiles(dirpath, nameonly + "*"))
            {
                if (item == filepath)
                    continue;

                string fname = Path.GetFileName(item);
                string pfx = fname.Substring(0, fname.Length - ext.Length).Substring(nameonly.Length);
                pfx = pfx.Trim().TrimStart('(').TrimEnd(')').Trim();

                if (int.TryParse(pfx, out findx))
                    postfixes.Add(findx);
            }

            filename = string.Format("{0}({1}){2}", nameonly, postfixes.Count == 0 ? 1 : postfixes.Max() + 1, ext);
            return Path.Combine(dirpath, filename);
        }

        public static string GetRelativePath(string absPath, string relTo)
        {
            string[] absDirs = absPath.Split(Path.DirectorySeparatorChar);
            string[] relDirs = relTo.Split(Path.DirectorySeparatorChar);

            // Get the shortest of the two paths
            int len = absDirs.Length < relDirs.Length ? absDirs.Length :
            relDirs.Length;

            // Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            // Find common root
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
                else break;
            }

            // If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                throw new ArgumentException("Paths do not have a common base");

            // Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // Add on the ..
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0) relativePath.Append(@"..\");
            }

            // Add on the folders
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + @"\");
            }
            relativePath.Append(relDirs[relDirs.Length - 1]);

            return relativePath.ToString();
        }

    }
}
