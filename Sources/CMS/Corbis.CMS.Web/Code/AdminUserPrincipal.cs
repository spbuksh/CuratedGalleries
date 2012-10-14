using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Code
{

    public class AdminUserPrincipal : GenericPrincipal
    {
        /// <summary>
        /// Corbis user
        /// </summary>
        public AdminUserInfo User { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        public AdminUserPrincipal(IIdentity identity, AdminUserInfo user, string[] roles = null)
            : base(identity, roles == null ? new string[] { } : roles)
        {
            this.User = user;
        }
    }
}