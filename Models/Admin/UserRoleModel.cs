using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class UserRoleModel
    {
        public UserModel user { get; set; }
        public List<IdentityRole> roles { get; set; }
    }
}
