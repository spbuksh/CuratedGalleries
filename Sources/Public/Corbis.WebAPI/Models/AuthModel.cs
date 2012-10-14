using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Corbis.Public.Entity;

namespace Corbis.WebAPI.Models
{
    /// <summary>
    /// User authrntication model
    /// DO NOT MARK [SERIALIZABLE] !!!
    /// </summary>
    [DataContract]
    public class AuthModel
    {
        /// <summary>
        /// User name (login name). It is required
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "User name must not be empty")]
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// User password. It is required
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "User password must not be empty")]
        [DataMember(IsRequired = true)]
        public string Password { get; set; }

        /// <summary>
        /// Application scope. It is not required. If it is not pointed then all available scopes are used
        /// </summary>
        [DataMember]
        public Nullable<Scope> Scope { get; set; }

        /// <summary>
        /// Client identifier
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Client identifier is required")]
        [DataMember(IsRequired = true)]
        public string Client { get; set; }

        /// <summary>
        /// Client secret
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Client secret is required")]
        [DataMember(IsRequired = true)]
        public string ClientSecret { get; set; }
    }
}