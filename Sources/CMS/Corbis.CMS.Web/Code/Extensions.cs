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


        public static AdminUserInfo GetCurrentUser(this HttpSessionStateBase session)
        {
            return session["CurrentUser_{50E9BEBB-3352-4259-9487-4B96F9D3E549}"] as AdminUserInfo;
        }
        public static void SetCurrentUser(this HttpSessionStateBase session, AdminUserInfo user)
        {
            session["CurrentUser_{50E9BEBB-3352-4259-9487-4B96F9D3E549}"] = user;
        }
        public static AdminUserInfo GetCurrentUser(this System.Web.Mvc.WebViewPage page)
        {
            return page.Session.GetCurrentUser();
        }
        public static AdminUserInfo GetCurrentUser(this HttpContextBase context)
        {
            return context.Session.GetCurrentUser();
        }


        public static string GetText(this CuratedGalleryStatuses item)
        {
            switch (item)
            {
                case CuratedGalleryStatuses.Published:
                    return "Published";
                case CuratedGalleryStatuses.UnPublished:
                    return "Un-Published";
                default:
                    throw new NotImplementedException();
            }            
        }
    }
}