﻿using System;
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
        public OperationResult<OperationResults, int?> CreateUser(int? creatorID, AdminUser user)
        {
            var output = new OperationResult<OperationResults, int?>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            {
                if(context.Connection.State != System.Data.ConnectionState.Open)
                    context.Connection.Open();

                try
                {
                    if (!this.CheckLogin(context, user.Login))
                        return new OperationResult<OperationResults, int?>() { Result = OperationResults.Failure };

                    context.Transaction = context.Connection.BeginTransaction();

                    var profile = new AdminUserProfileRecord()
                    {
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        Email = user.Email
                    };

                    context.AdminUserProfileRecords.InsertOnSubmit(profile);
                    context.SubmitChanges();

                    var member = new AdminUserMembershipRecord()
                    {
                        Login = user.Login,
                        Password = this.PasswordUtility.HashPassword(user.Password, this.PasswordHashKey),
                        ProfileID = profile.ID,
                        IsActive = user.IsActive
                    };
                    context.AdminUserMembershipRecords.InsertOnSubmit(member);
                    context.SubmitChanges();

                    context.AdminUserToRoleRecords.InsertOnSubmit(new AdminUserToRoleRecord() { RoleID = (int)user.Roles.Value, MemberID = member.ID });
                    context.SubmitChanges();

                    context.Transaction.Commit();

                    output.Output = member.ID;
                }
                catch(Exception ex)
                {
                    output.Result = OperationResults.Failure;

                    this.Logger.WriteError(ex);

                    if (context.Transaction != null)
                        context.Transaction.Rollback();
                }

                return output;
            }
        }

        public OperationResult<OperationResults, object> DeleteUser(int id)
        {
            var output = new OperationResult<OperationResults, object>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            { 
                if(context.Connection.State != System.Data.ConnectionState.Open)
                    context.Connection.Open();

                try
                {
                    context.Transaction = context.Connection.BeginTransaction();

                    //
                    foreach (var gallery in context.CuratedGalleryRecords.Where(x => x.Editor != null && x.Editor.Value == id))
                        gallery.Editor = null;

                    context.SubmitChanges();

                    //
                    foreach (var period in context.GalleryPublicationPeriodRecords.Where(x => x.PublisherID != null && x.PublisherID.Value == id))
                        period.PublisherID = null;

                    context.SubmitChanges();

                    //
                    foreach (var user2role in context.AdminUserToRoleRecords.Where(x => x.MemberID == id).ToList())
                        context.AdminUserToRoleRecords.DeleteOnSubmit(user2role);

                    context.SubmitChanges();

                    var memeber = context.AdminUserMembershipRecords.Where(x => x.ID == id).Single();
                    var profileID = memeber.ProfileID;
                    context.AdminUserMembershipRecords.DeleteOnSubmit(memeber);
                    context.SubmitChanges();

                    context.AdminUserProfileRecords.DeleteOnSubmit(context.AdminUserProfileRecords.Where(x => x.ID == profileID).Single());
                    context.SubmitChanges();

                    context.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    output.Result = OperationResults.Failure;

                    this.Logger.WriteError(ex);

                    if (context.Transaction != null)
                        context.Transaction.Rollback();
                }
            }

            return output;
        }


        public OperationResult<OperationResults, object> ChangeUserActivation(int id, bool isActive)
        {
            using (var context = this.CreateMainContext())
            {
                var record = context.AdminUserMembershipRecords.Where(x => x.ID == id).SingleOrDefault();

                if (record == null)
                    return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                record.IsActive = isActive;
                context.SubmitChanges();

                return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OperationResult<OperationResults, List<AdminUser>> GetUsers()
        {
            using (var context = this.CreateMainContext())
            {
                var query = from m in context.AdminUserMembershipRecords
                            join p in context.AdminUserProfileRecords on m.ProfileID equals p.ID
                            join m2r in context.AdminUserToRoleRecords on m.ID equals m2r.MemberID into roles
                            select new { member = m, profile = p, roles = roles };

                var users = new List<AdminUser>();

                foreach (var item in query)
                {
                    var user = new AdminUser()
                    {
                        ID = item.member.ID,
                        FirstName = item.profile.FirstName,
                        MiddleName = item.profile.MiddleName,
                        LastName = item.profile.LastName,
                        IsActive = item.member.IsActive,
                        Email = item.profile.Email,
                        Login = item.member.Login
                    };

                    if (item.roles != null)
                    {
                        foreach (var irole in item.roles)
                            user.Roles = user.Roles.HasValue ? (user.Roles.Value | (AdminUserRoles)irole.RoleID) : (AdminUserRoles)irole.RoleID;
                    }

                    users.Add(user);
                }

                return new OperationResult<OperationResults, List<AdminUser>>() { Result = OperationResults.Success, Output = users };
            }
        }



        /// <summary>
        /// Gets admin user info by his memebership identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperationResult<OperationResults, AdminUser> GetUser(int id)
        {
            var result = new OperationResult<OperationResults, AdminUser>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            {
                var query = from m in context.AdminUserMembershipRecords
                            join p in context.AdminUserProfileRecords on m.ProfileID equals p.ID
                            join m2r in context.AdminUserToRoleRecords on m.ID equals m2r.MemberID into roles
                            where m.ID == id
                            select new { member = m, profile = p, roles = roles };

                var record = query.SingleOrDefault();

                if (record == null)
                {
                    result.Result = OperationResults.NotFound;
                    return result;
                }

                var user = new AdminUser() 
                {
                    FirstName = record.profile.FirstName,
                    Email = record.profile.Email,
                    ID = id,
                    IsActive = record.member.IsActive,
                    LastName = record.profile.LastName,
                    Login = record.member.Login,
                    MiddleName = record.profile.MiddleName,
                    Password = null
                };

                foreach (var item in record.roles)
                    user.Roles = user.Roles.HasValue ? (user.Roles.Value | (AdminUserRoles)item.RoleID) : (AdminUserRoles)item.RoleID;

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
                            where m.Login.ToLower() == loginLower && m.IsActive
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
                    user.Roles = user.Roles.HasValue ? (user.Roles.Value | (AdminUserRoles)item) : (AdminUserRoles)item;
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

                    if (!this.CheckLogin(context, form.Login))
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
        public OperationResult<OperationResults, bool?> CheckLogin(string login)
        {
            using (var context = this.CreateMainContext())
            {
                return new OperationResult<OperationResults, bool?>()
                {
                    Result = OperationResults.Success,
                    Output = this.CheckLogin(context, login)
                };
            }
        }

        protected bool CheckLogin(MainDataContext context, string login)
        {
            return context.AdminUserMembershipRecords.Where(x => string.Equals(x.Login, login)).FirstOrDefault() == null;
        }


        public OperationResult<OperationResults, object> ChangeUserPassword(int userID, string password) 
        {
            using (var context = this.CreateMainContext())
            {
                var record = context.AdminUserMembershipRecords.Where(x => x.ID == userID).SingleOrDefault();

                if (record == null)
                    return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                record.Password = this.PasswordUtility.HashPassword(password, this.PasswordHashKey);
                context.SubmitChanges();

                return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
            }
        }
        public OperationResult<OperationResults, object> ChangeUserRoles(int userID, AdminUserRoles roles)
        {
            using (var context = this.CreateMainContext())
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();

                try
                {
                    var record = context.AdminUserMembershipRecords.Where(x => x.ID == userID).SingleOrDefault();

                    if (record == null)
                        return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                    foreach (var item in record.AdminUserToRoleRecords)
                        context.AdminUserToRoleRecords.DeleteOnSubmit(item);

                    context.SubmitChanges();

                    //TODO: refactor, temporary we use 2 roles => for that reason I insert 1 item
                    context.AdminUserToRoleRecords.InsertOnSubmit(new AdminUserToRoleRecord() { MemberID = userID, RoleID = (int)roles });
                    context.SubmitChanges();

                    context.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    if (context.Transaction != null)
                        context.Transaction.Rollback();

                    this.Logger.WriteError(ex);
                    throw;
                }

                return new OperationResult<OperationResults, object>() { Result = OperationResults.Success };
            }
        }



        public OperationResult<OperationResults, object> UpdateUser(AdminUser user)
        {
            var output = new OperationResult<OperationResults, object>() { Result = OperationResults.Success };

            using (var context = this.CreateMainContext())
            {
                if (context.Connection.State != System.Data.ConnectionState.Open)
                    context.Connection.Open();

                try
                {
                    context.Transaction = context.Connection.BeginTransaction();

                    var member = context.AdminUserMembershipRecords.Where(x => x.ID == user.ID.Value).SingleOrDefault();

                    if (member == null)
                        return new OperationResult<OperationResults, object>() { Result = OperationResults.NotFound };

                    member.IsActive = user.IsActive;
                    member.Login = user.Login;
                    context.SubmitChanges();

                    var profile = context.AdminUserProfileRecords.Where(x => x.ID == member.ProfileID).Single();
                    profile.Email = user.Email;
                    profile.FirstName = user.FirstName;
                    profile.LastName = user.LastName;
                    profile.MiddleName = user.MiddleName;
                    context.SubmitChanges();

                    foreach (var item in member.AdminUserToRoleRecords)
                        context.AdminUserToRoleRecords.DeleteOnSubmit(item);

                    context.SubmitChanges();

                    context.AdminUserToRoleRecords.InsertOnSubmit(new AdminUserToRoleRecord() { MemberID = user.ID.Value, RoleID = (int)user.Roles });
                    context.SubmitChanges();

                    context.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    output.Result = OperationResults.Failure;

                    this.Logger.WriteError(ex);

                    if (context.Transaction != null)
                        context.Transaction.Rollback();
                }

                return output;
            }
        }
    }
}
