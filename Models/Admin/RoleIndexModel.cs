using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class RoleIndexModel
    {
        public IEnumerable<Role> AllRoles { get;set; }
        public int NumberOfPages { get; set; }
    }
}
