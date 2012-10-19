using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.IO;

namespace Corbis.Common
{
    public static class Extensions
    {
        public static string GetUrlEncoded(this string item)
        {
            return HttpUtility.UrlEncode(item);
        }

        /// <summary>
        /// Gets separated items as string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> items, string separator, ActionHandler<T, string> converter = null)
        {
            StringBuilder builder = new StringBuilder();

            foreach (T item in items)
                builder.AppendFormat("{0}{1}", (builder.Length == 0 ? string.Empty : separator), (converter == null ? item.ToString() : converter(item)));

            return builder.ToString();
        }

        public static string ToString(this Enum eitem, string separator, ActionHandler<Enum, string> converter = null)
        {
            return eitem.GetItems().ToString(separator, converter);
        }


        /// <summary>
        /// Get enum items as list. If the enum is marked with FlagsAttribute then it returns list of included enum items
        /// </summary>
        /// <param name="eitem"></param>
        /// <param name="excludes"></param>
        /// <returns></returns>
        public static List<Enum> GetItems(this Enum eitem)
        {
            Type etype = eitem.GetType();

            var attr = etype.GetCustomAttributes(typeof(FlagsAttribute), false).SingleOrDefault();

            if (attr == null)
                return new List<Enum>() { eitem };

            List<Enum> output = new List<Enum>();

            ulong eival = ((IConvertible)eitem).ToUInt64(null);

            SortedList<ulong, Enum> etems = new SortedList<ulong, Enum>();

            foreach (Enum item in Enum.GetValues(etype))
            {
                var ival = ((IConvertible)item).ToUInt64(null);

                if (ival <= eival)
                    etems.Add(((IConvertible)item).ToUInt64(null), item);
            }

            bool bFlag = false;

            foreach (var entry in etems)
            {
                if ((entry.Key & eival) != entry.Key)
                    continue;

                //Filter enum values. We do not consider composite items
                bFlag = false;

                foreach (var key in etems.Keys.Where(x => x < entry.Key))
                {
                    if ((key & entry.Key) != key) continue;
                    bFlag = true;
                    break;
                }

                if (!bFlag)
                    output.Add(entry.Value);
            }

            return output;
        }

        public static string ToISO8601(this DateTime datetime, bool dateOnly = false)
        {
            return datetime.ToString(dateOnly ? "yyyy-MM-dd" : "yyyy-MM-ddTHH-mm-ss");
        }



        public static DirectoryInfo CopyTo(this DirectoryInfo from, string to)
        {
            var dir = new DirectoryInfo(to);
            Utils.DirectoryCopy(from, dir);
            return dir;
        }

        public static void Clear(this DirectoryInfo dir)
        {
            Utils.DirectoryClear(dir);
        }

    }
}
