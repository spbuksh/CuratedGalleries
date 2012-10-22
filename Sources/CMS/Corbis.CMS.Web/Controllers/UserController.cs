using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Http;

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


namespace Corbis.CMS.Web.Controllers
{
    public class UserController : CMSControllerBase
    {
        [Dependency]
        public IAdminUserRepository UserRepository { get; set; }

        public ActionResult Index()
        {
            //return View(this.UserRepository.GetUsers(this.CurrentUser.Roles));

            throw new NotImplementedException();

        }


        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(AdminUser user)
        {
            //try
            //{

            //    this.UserRepository.AddUser(user.Login, user.IsActive, user.Email, user.Roles, user.Password);
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}

            throw new NotImplementedException();
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

        public ActionResult DeleteUser(int id)
        {
            //this.UserRepository.DeleteUser(id);
            //return PartialView("Controls/Grid/_UserGrid", this.UserRepository.GetUsers(((AdminUserPrincipal)this.HttpContext.User).User.Roles));

            throw new NotImplementedException();
        }
    }
}
