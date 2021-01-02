using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class UserModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Nickname")]
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Nickname { get; set; }
        public DateTime Created { get; set; }
        [Display(Name = "Profile Image Url")]
        public string ProfileImageUrl { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public List<IdentityRole> Roles { get; set;}
}
}
