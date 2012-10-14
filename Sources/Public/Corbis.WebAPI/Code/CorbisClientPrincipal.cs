using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Corbis.WebAPI.Code
{

    /// <summary>
    /// 
    /// </summary>
    public class CorbisClientPrincipal : GenericPrincipal
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roles"></param>
        /// <param name="accessToken"> </param>
        public CorbisClientPrincipal(IIdentity identity, string[] roles, Guid? accessToken)
            : base(identity, roles)
        {
            _accessToken = accessToken;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Guid? AccessToken
        {
            get { return _accessToken; }
        }

        #endregion

        #region Fields

        private readonly Guid? _accessToken;

        #endregion





    }
}