using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Public.Entity;
using System.Runtime.Serialization;

namespace Corbis.CMS.Entity
{
    /// <summary>
    /// Enumerates all possible admin user roles
    /// </summary>
    [Serializable, DataContract]
    [Flags]
    public enum AdminUserRoles
    {
        /// <summary>
        /// Super admin role
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// Regular admin user role
        /// </summary>
        Admin = 2
    }


    [Serializable, DataContract]
    public class AdminUserInfo
    {
        /// <summary>
        /// User unique identifier
        /// </summary>
        public virtual int ID { get; set; }

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

    [Serializable, DataContract]
    public class AdminUser : AdminUserInfo
    {
        /// <summary>
        /// User login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User explicit password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
    }
}
