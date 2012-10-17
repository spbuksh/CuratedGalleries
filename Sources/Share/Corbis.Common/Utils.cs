using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        public static void DirectoryCopy(DirectoryInfo from, DirectoryInfo to)
        {
            if (!to.Exists)
                to.Create();

            foreach (var fi in from.GetFiles())
                fi.CopyTo(Path.Combine(to.ToString(), fi.Name), true);

            foreach (var subdir in from.GetDirectories())
                DirectoryCopy(subdir, to.CreateSubdirectory(subdir.Name));
        }

    }
}
