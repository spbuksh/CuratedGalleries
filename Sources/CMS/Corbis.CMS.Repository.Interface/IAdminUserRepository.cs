using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Entity;
using Corbis.Public.Entity;
using Corbis.Common;


namespace Corbis.CMS.Repository.Interface
{
    public interface IAdminUserRepository 
    {
        //TODO: These methods are obsolete!!! Refactor them and delete
        IEnumerable<AdminUser> GetUsers(AdminUserRoles role);
        void AddUser(string login, bool isactive, string email, AdminUserRoles roleId, string password);
        void UpdateUser(int id, string login, bool isactive, string email, int roleId, string password);
        void UpdateUser(int id, string login, bool isactive, string email, int roleId);
        AdminUser GetById(int id);
        void DeleteUser(int id);


        //*********************

        /// <summary>
        /// Authenticates admin user
        /// </summary>
        /// <param name="login">user login name</param>
        /// <param name="password">plain user password</param>
        /// <returns></returns>
        OperationResult<UserAuthResults, AdminUserInfo> Authenticate(string login, string password);

        /// <summary>
        /// Registers user as admin member
        /// </summary>
        /// <param name="form">Admin user registration data</param>
        /// <returns>Admin user member identifier</returns>
        OperationResult<OperationResults, Nullable<int>> Register(AdminRegistrationForm form);
    }
}
