using Microsoft.AspNetCore.Identity;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects
{
    public interface IUser
    {
        AppUser GetById(string id);
        IEnumerable<AppUser> GetAll();
        Task SetProfileImage(string id, Uri uri);

        Task<IdentityResult> ChangePassword(string id, ChangePasswordModel passwordModel);
    }
}
