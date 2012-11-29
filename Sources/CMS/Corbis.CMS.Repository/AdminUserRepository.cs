using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Entity;
using System.Security.Cryptography;
using Corbis.Public.Repository;
using Corbis.DB.Linq;
using Corbis.Public.Entity;
using Corbis.Common;
using System.Configuration;
using Corbis.Common.Utilities.Password;
using Corbis.Common.ObjectMapping.Mappers;


namespace Corbis.CMS.Repository
{
    [Serializable]
    public class AdminUserRepository : RepositoryBase, IAdminUserRepository
    {
        #region Obsolete

        public IEnumerable<Entity.AdminUser> GetUsers(AdminUserRoles role)
        {
            //using (var context = this.CreateMainContext())
            //{
            //    return context.AdminUserRecords.Where(u => (u.RoleID & (int)role) == (int)role).Select(u => new AdminUser() { UserID = u.ID, IsActive = u.IsActive, Login = u.Login, Email = u.Email, Role = (AdminUserRoles)u.RoleID }).ToList<AdminUser>();
            //}

            throw new NotImplementedException();
        }


        public void AddUser(string login, bool isactive, string email, AdminUserRoles role, string password)
        {
            //var user = new AdminUserRecord();
            //user.Login = login;
            //user.IsActive = isactive;
            //user.DateCreated = DateTime.Now;
            //user.Email = email;
            //user.RoleID = role;
            //user.Password = EncryptPassword(password);
            //using (var context = this.CreateMainContext())
            //{
            //    context.AdminUserRecords.InsertOnSubmit(user);
            //    context.SubmitChanges();
            //}

            throw new NotImplementedException();
        }

        public void UpdateUser(int id, string login, bool isactive, string email, int roleId, string password)
        {
            //using (var context = this.CreateMainContext())
            //{
            //    var user = context.AdminUserRecords.Single(u => u.UserId == id);
            //    user.Login = login;
            //    user.IsActive = isactive;
            //    user.Password = EncryptPassword(password);
            //    user.Email = email;
            //    user.UserRoleId = roleId;

            //    context.SubmitChanges();
            //}

            throw new NotImplementedException();
        }

        public void UpdateUser(int id, string login, bool isactive, string email, int roleId)
        {
            //using (var context = this.CreateMainContext())
            //{
            //    var user = context.AdminUserRecords.Single(u => u.UserId == id);
            //    user.Login = login;
            //    user.IsActive = isactive;
            //    user.Email = email;
            //    user.UserRoleId = roleId;

            //    context.SubmitChanges();
            //}

            throw new NotImplementedException();
        }

        public Entity.AdminUser GetById(int id)
        {
            //using (var context = this.CreateMainContext())
            //{
            //    return context.AdminUserRecords.Where(u => u.UserId == id).Select(u => new AdminUser() { UserID = u.UserId, Email = u.Email, Role = (AdminUserRoles)u.UserRoleId, Login = u.Login, IsActive = u.IsActive }).First<AdminUser>();
            //}

            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            //using (var context = this.CreateMainContext())
            //{
            //    var user = context.AdminUserRecords.Single(u => u.ID == id);
            //    context.AdminUserRecords.DeleteOnSubmit(user);
            //    context.SubmitChanges();
            //}

            throw new NotImplementedException();
        }

        #endregion Obsolete

        /// <summary>
        /// Gets admin user info by his memebership identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperationResult<OperationResults, AdminUserInfo> GetUserInfo(int id)
        {
            var result = new OperationResult<OperationResults, AdminUserInfo>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            {
                var query = from m in context.AdminUserMembershipRecords
                            join u in context.AdminUserProfileRecords on m.ProfileID equals u.ID
                            where m.ID == id
                            select new { ID = m.ID, FirstName = u.FirstName, MiddleName = u.MiddleName, LastName = u.LastName, IsActive = m.IsActive, Password = m.Password };

                var record = query.SingleOrDefault();

                if (record == null)
                {
                    result.Result = OperationResults.NotFound;
                    return result;
                }

                var user = this.ObjectMapper.DoMapping<AdminUserInfo>(record);

                foreach (int item in context.AdminUserToRoleRecords.Where(x => x.MemberID == record.ID).Select(x => x.RoleID))
                    user.Roles = user.Roles.HasValue ? (user.Roles.Value | this.GetRole(item)) : (AdminUserRoles)item;

                result.Output = user;
            }

            return result;
        }

        private string EncryptPassword(string password)
        {
            MD5CryptoServiceProvider cryptoSvc = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = cryptoSvc.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Password utilities
        /// </summary>
        protected virtual IPasswordUtility PasswordUtility
        {
            get { return Corbis.Common.Utilities.Password.PasswordUtility.Instance; }
        }

        /// <summary>
        /// Gets the password hash key.
        /// </summary>
        protected virtual string PasswordHashKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.m_PasswordHashKey))
                    this.m_PasswordHashKey = ConfigurationManager.AppSettings["PasswordHashKey"];

                return this.m_PasswordHashKey;
            }
        }
        private string m_PasswordHashKey = null;


        public OperationResult<UserAuthResults, AdminUserInfo> Authenticate(string login, string password)
        {
            var result = new OperationResult<UserAuthResults, AdminUserInfo>();

            using (var context = this.CreateMainContext())
            {
                string loginLower = login.ToLower();

                var query = from m in context.AdminUserMembershipRecords
                            join u in context.AdminUserProfileRecords on m.ProfileID equals u.ID
                            where m.Login.ToLower() == loginLower
                            select new { ID = m.ID, FirstName = u.FirstName, MiddleName = u.MiddleName, LastName = u.LastName, IsActive = m.IsActive, Password = m.Password };

                var record = query.SingleOrDefault();

                if (record == null)
                {
                    result.Result = UserAuthResults.NotFound;
                    return result;
                }

                if (record.Password != this.PasswordUtility.HashPassword(password, this.PasswordHashKey))
                {
                    result.Result = UserAuthResults.InvalidPassword;
                    return result;
                }

                var user = this.ObjectMapper.DoMapping<AdminUserInfo>(record);

                foreach (int item in context.AdminUserToRoleRecords.Where(x => x.MemberID == record.ID).Select(x => x.RoleID))
                {
                    user.Roles = user.Roles.HasValue ? (user.Roles.Value | this.GetRole(item)) : (AdminUserRoles)item;
                }

                result.Output = user;
                result.Result = Public.Entity.UserAuthResults.Success;
            }

            return result;
        }

        public OperationResult<OperationResults, Nullable<int>> Register(AdminRegistrationForm form)
        {
            using (var context = this.CreateMainContext())
            {
                try
                {
                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();

                    if (!this.Check(context, form.Login))
                        throw new NotImplementedException();

                    var profile = this.ObjectMapper.DoMapping<AdminUserProfileRecord>(form);

                    //create user profile
                    context.AdminUserProfileRecords.InsertOnSubmit(profile);
                    context.SubmitChanges();

                    //register user as admin
                    var member = new AdminUserMembershipRecord()
                    {
                        Login = form.Login,
                        IsActive = form.IsActive,
                        ProfileID = profile.ID,
                        Password = this.PasswordUtility.HashPassword(form.Password, this.PasswordHashKey),
                        DateCreated = DateTime.UtcNow
                    };
                    context.AdminUserMembershipRecords.InsertOnSubmit(member);
                    context.SubmitChanges();

                    //set user roles


                    context.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    this.Logger.WriteError(ex);
                    context.Transaction.Rollback();
#if DEBUG
                    throw;
#else
                    return new OperationResult<OperationResults, int?>() { Result = OperationResults.Failure };
#endif
                }

            }

           //TODO: Set Output param to ProfileID.
           return new OperationResult<OperationResults, int?>{Result = OperationResults.Success, Output = null};
        }

        /// <summary>
        /// Checks user login name for existance
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Boolean value. If it is true then login name can be used, otherwise this login is exists in the system</returns>
        public OperationResult<OperationResults, Nullable<bool>> Check(string login)
        {
            using (var context = this.CreateMainContext())
            {
                return new OperationResult<OperationResults, bool?>()
                {
                    Result = OperationResults.Success,
                    Output = this.Check(context, login)
                };
            }
        }

        protected bool Check(MainDataContext context, string login)
        {
            return context.AdminUserMembershipRecords.Where(x => string.Equals(x.Login, login)).Count() == 0;
        }


        private AdminUserRoles GetRole(int roleID)
        {
            switch(roleID)
            {
                case 1: return AdminUserRoles.Admin;
                case 2: return AdminUserRoles.SuperAdmin;
                default: throw new NotImplementedException();
            }            
        }
    }
}
