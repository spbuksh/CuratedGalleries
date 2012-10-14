using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.Public.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Corbis.WebAPI.Models
{
    /// <summary>
    /// Base user token model. This model is used for authentication response
    /// </summary>
    [DataContract]
    public class UserTokenModel
    {
        /// <summary>
        /// User access token
        /// </summary>
        [DataMember]
        public virtual Nullable<Guid> AccessToken { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        [DataMember]
        public virtual Nullable<Guid> RefreshToken { get; set; }

        /// <summary>
        /// Application scope
        /// </summary>
        [DataMember]
        public virtual Nullable<Scope> Scope { get; set; }

        /// <summary>
        /// Access token expiration date time in UTC format
        /// </summary>
        [DataMember]
        public virtual Nullable<long> ExpirationDate { get; set; }
    }

    /// <summary>
    /// User token model for refresh request
    /// </summary>
    [DataContract]
    public class UserTokenRequestModel : UserTokenModel
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        [Required(ErrorMessage = "Refresh user token is required")]
        [DataMember(IsRequired = true)] 
        public override Nullable<Guid> RefreshToken { get; set; }
    }
}