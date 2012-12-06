using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Corbis.CMS.Web.Models
{
    public class UserProfileDetailsModel
    {
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Login")]
        public virtual string Login { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public virtual string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }
    }
}