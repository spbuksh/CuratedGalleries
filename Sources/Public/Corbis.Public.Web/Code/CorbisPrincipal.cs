using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Corbis.Public.Entity;

namespace Corbis.Public.Web.Code
{
    public class CorbisPrincipal : GenericPrincipal
    {
        /// <summary>
        /// Corbis user
        /// </summary>
        public CorbisUser User { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        public CorbisPrincipal(IIdentity identity, CorbisUser user, string[] roles = null)
            : base(identity, roles == null ? new string[] { } : roles)
        {
            this.User = user;
        }
    }
}