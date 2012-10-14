using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Models
{
    [Serializable]
    public class AdminUserModel
    {
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string PlainPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Role")]
        public AdminUserRoles Role { get; set; }

    }
}