using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using DotNetOpenAuth.AspNet;
using Newtonsoft.Json;
using Corbis.Public.Entity;
using Corbis.CMS.Entity;
using Corbis.Public.Repository.Interface;
using Microsoft.Practices.Unity;
using Corbis.CMS.Web.Code;
using Corbis.CMS.Repository.Interface;
using Corbis.CMS.Web.Models;
using Corbis.Common;


namespace Corbis.CMS.Web.Controllers
{
    public class UserController : CMSControllerBase
    {
        [Dependency]
        public IAdminUserRepository UserRepository { get; set; }

        public ActionResult Index()
        {
            var users = this.UserRepository.GetUsers().Output;

            return this.View(users);
        }


        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new AdminUserMembershipModel()
            {
                IsActive = true,
                Roles = AdminUserRoles.Admin
            };
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Create(AdminUserMembershipModel model)
        {
            if (!this.ModelState.IsValid)
                return this.View("Create", model);

            var user = this.ObjectMapper.DoMapping<AdminUser>(model);

            OperationResult<OperationResults, int?> rslt = null;

            try
            {
                rslt = this.UserRepository.CreateUser(this.CurrentUser.ID, user);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
            }

            if (rslt == null || rslt.Result != OperationResults.Success)
            {
                this.ModelState.AddModelError(string.Empty, "User was not created due to server error");
                return this.View("Create", model);
            }

            return this.RedirectToAction("Index", "User");
        }


        public ActionResult Edit(int id)
        {
            //AdminUser item = this.UserRepository.GetById(id);
            //return View(item);

            throw new NotImplementedException();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(int id, AdminUserModel user)
        {
            //AdminUser item = this.UserRepository.GetById(id);
            //try
            //{
            //    if (!string.IsNullOrEmpty(user.Password) && user.Password.Equals(user.PlainPassword)) {
            //        this.UserRepository.UpdateUser(id, user.Login, user.IsActive, user.Email, (int)user.Role, user.Password);
            //    }
            //    else
            //    {
            //        this.UserRepository.UpdateUser(id, user.Login, user.IsActive, user.Email, (int)user.Role);
            //    }
            //    return View(item);

            //}
            //catch
            //{
            //    return View(item);
            //}

            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            OperationResult<OperationResults, object> rslt = null;

            try
            {
                rslt = this.UserRepository.DeleteUser(id);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
            }

            if (rslt == null || rslt.Result != OperationResults.Success)
                return this.Json(new { success = false, error = "Server error" });

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Makes admin user or active or inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeUserActivation(int id, bool isActive)
        {
            OperationResult<OperationResults, object> rslt = null;

            try
            {
                rslt = this.UserRepository.ChangeUserActivation(id, isActive);
            }
            catch (Exception ex)
            {
                this.Logger.WriteError(ex);
            }

            if (rslt == null || rslt.Result != OperationResults.Success)
                return this.Json(new { success = false, error = "Server error" });

            return this.Json(new { success = true });
        }

        [HttpGet]
        public ActionResult ChangePasswordPopup(string popupID, int userID)
        {
            this.ViewBag.PopupID = popupID;
            this.ViewBag.UserID = userID;
            return this.PartialView("ChangePasswordPopup", new ChangePasswordModel());
        }
        [HttpPost]
        public ActionResult ChangePasswordPopup(string popupID, int userID, ChangePasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.PopupID = popupID;
                this.ViewBag.UserID = userID;
                return this.PartialView("ChangePasswordPopup", model);
            }
            var rslt = this.UserRepository.ChangeUserPassword(userID, model.Password);
            return this.Json(new { success = rslt.Result == OperationResults.Success });
        }

    }
}
