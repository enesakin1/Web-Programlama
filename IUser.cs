using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Models;
using myiotprojects.Models.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects
{
    public interface IUser
    {
        AppUser GetById(string id);
        Task AddPhoto(string userid, string photourl);
        List<IdentityRole> GetAllRoles();
        Task UserRoleAdd(string userid, string roleName);
        Task UserRoleDelete(string userid, string roleName);
        Task<IList<string>> GetUserRoles(AppUser user);
        Task<bool> GetIfUserInRole(AppUser user, string roleName);
        IQueryable<IdentityRole> GetAllRolesWithPages(int page);
        IdentityRole GetRoleById(string id);
        Task DeleteRole(IdentityRole rol);
        Task CreateRole(string name);
        Task UpdateRole(IdentityRole model);
        IEnumerable<AppUser> GetAll();
        IEnumerable<AppUser> GetAllWithPage(int page);
        Task UpdateUser(UserModel model);
        Task SetProfileImage(string id, Uri uri);
        Task DeleteUser(string id);
        Task CreateUser(AppUser user, string password);

        Task<IdentityResult> ChangePassword(string id, ChangePasswordModel passwordModel);
    }
}
