using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Corbis.CMS.Entity;

namespace Corbis.CMS.Web.Models
{
    public class AdminUserMembershipModel : UserProfileDetailsModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ComfirmPassword { get; set; }

        [Required]
        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Role")]
        public AdminUserRoles Roles { get; set; }
    }
}