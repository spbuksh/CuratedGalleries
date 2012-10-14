using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Admin user registration data
    /// </summary>
    [Serializable, DataContract]
    public class AdminRegistrationForm
    {
        /// <summary>
        /// User login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User first name
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// User middle name
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Is admin user active or not
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Admin user roles
        /// </summary>
        public virtual Nullable<AdminUserRoles> Roles { get; set; }
    }
}
