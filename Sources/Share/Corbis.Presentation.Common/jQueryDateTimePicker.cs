using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Corbis.Presentation.Common
{
    public class jQueryDateTimePicker
    {
        /// <summary>
        /// Convert server datetime format to client(JavaScript) datetime format
        /// </summary>
        public static string GetJSClientDateFormat(string format)
        {
            //convertation based on http://docs.jquery.com/UI/Datepicker/%24.datepicker.formatDate & http://msdn.microsoft.com/en-us/library/system.globalization.datetimeformatinfo%28v=VS.71%29.aspx
            format = format.Replace("MMMM", "<LONG_NAME_MONTH>").Replace("MMM", "<SHORT_NAME_MONTH>").Replace("MM", "mm").Replace("M", "m").Replace("<LONG_NAME_MONTH>", "MM").Replace("<SHORT_NAME_MONTH>", "M");
            format = format.Replace("yyyy", "<LONG_YEAR>").Replace("yy", "<SHORT_YEAR>").Replace("<LONG_YEAR>", "yy").Replace("<SHORT_YEAR>", "y");
            return format;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetJSClientTimeFormat(string format)
        {
            format = format.Replace("HH", "hh").Replace("H", "h").Replace("tt", "TT").Replace("t", "T");
            return format;
        }

    }
}
