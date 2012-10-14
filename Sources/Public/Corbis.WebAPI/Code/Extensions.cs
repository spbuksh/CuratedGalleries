using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corbis.WebAPI.Code
{
    /// <summary>
    /// Class which contains extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets error list as list of formatted strings
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<string> GetErrorList(this System.Web.Http.ModelBinding.ModelStateDictionary state)
        {
            if (state.IsValid)
                return new List<string>();

            var output = new List<string>();

            foreach (var key in state.Keys)
            {
                foreach (var error in state[key].Errors)
                {
                    string emsg = string.IsNullOrEmpty(error.ErrorMessage) ? (error.Exception == null ? "Unknown server error" : error.Exception.ToString()) : error.ErrorMessage;
                    output.Add(string.Format("{0}: {1}", key, emsg));
                }
            }

            return output;
        }
    }
}