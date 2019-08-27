using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

    public class EditUserAccountViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string CurrentJob { get; set; }
        [StringLength(200)]
        public string Company { get; set; }
        public double? CountExperience { get; set; }
        [StringLength(10)]
        public string TimeUnit { get; set; }

        public IEnumerable<string> RolesList { get; set; }
    }
}