using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using Corbis.Common;
using System.IO;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        public static string GetFullName(this AdminUserInfo item)
        {
            return string.Format("{0} {1}", item.FirstName, item.LastName);
        }

        public static string[] GetRoleNames(this AdminUserRoles roles)
        {
            return roles.GetItems().Select(x => x.ToString().ToLower()).ToArray();
        }
    }
}