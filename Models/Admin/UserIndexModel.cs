using Microsoft.AspNetCore.Identity;
using myiotprojects.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class UserIndexModel
    {
        
        public int NumberOfPages { get; set; }
        public IEnumerable<UserModel> AllUsers { get; set; }
        public IList<string> AllRoles { get; set; }
    }
}
