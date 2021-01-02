using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace myiotprojects.Models.Admin
{
    public class Role
    {
        public string Id { get; set; }
        [Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Name { get; set; }
    }
}
