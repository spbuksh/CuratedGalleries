using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Corbis.Public.Entity
{
    /// <summary>
    /// Corbis User OAuth token
    /// </summary>
    [Serializable]
    public class UserToken
    {
        /// <summary>
        /// Access token. 
        /// In order to create and use right workflow in app logic access token is null if it ie expired
        /// </summary>
        public Nullable<Guid> AccessToken 
        {
            get { return this.IsExpired ? null : this.m_AccessToken; }
            set { this.m_AccessToken = value; }
        }
        private Nullable<Guid> m_AccessToken = null;

        /// <summary>
        /// Refresh toket
        /// </summary>
        public Guid RefreshToken { get; set; }

        /// <summary>
        /// TODO: Question .. what is this? It is returned from CorbisAPI
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Application scope
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Access token expiration date time in UTC format
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Indicates if token expired or not
        /// </summary>
        public bool IsExpired 
        {
            get { return this.ExpirationDate < DateTime.UtcNow; }
        }
    }
}
