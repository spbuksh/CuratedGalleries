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
        /// <summary>
        /// Creates admin user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User identifier</returns>
        OperationResult<OperationResults, int?> CreateUser(int? creatorID, AdminUser user);

        /// <summary>
        /// Deletes admin user
        /// </summary>
        /// <param name="id">user unique identifier</param>
        /// <returns></returns>
        OperationResult<OperationResults, object> DeleteUser(int id);

        /// <summary>
        /// Sets user as active or inactive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        OperationResult<OperationResults, object> ChangeUserActivation(int id, bool isActive);

        /// <summary>
        /// Gets admin users
        /// </summary>
        /// <returns></returns>
        OperationResult<OperationResults, List<AdminUser>> GetUsers(/* TODO: add filter in the future */);

        /// <summary>
        /// Gets admin user info by his memebership identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperationResult<OperationResults, AdminUser> GetUser(int id);

        /// <summary>
        /// Gets admin user info by his memebership identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperationResult<OperationResults, object> UpdateUser(AdminUser user);

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

        /// <summary>
        /// Changes user password
        /// </summary>
        OperationResult<OperationResults, object> ChangeUserPassword(int userID, string password);


        OperationResult<OperationResults, object> ChangeUserRoles(int userID, AdminUserRoles roles);

    }
}
