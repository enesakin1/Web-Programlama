using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Data;
using myiotprojects.Models;
using myiotprojects.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace myiotprojects
{
    class UserService : IUser
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(AuthDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddPhoto(string userid, string photourl)
        {
            if(!string.IsNullOrEmpty(photourl))
            {
                var user = GetById(userid);
                user.ProfileImageUrl = photourl;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task CreateRole(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                await _roleManager.CreateAsync(new IdentityRole(name));
            }
        }

        public async Task CreateUser(AppUser user, string password)
        {
            if(!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Nickname) )
            {
                await _userManager.CreateAsync(user, password);
            }
        }

        public async Task DeleteRole(IdentityRole role)
        {
            await _roleManager.DeleteAsync(role);
        }

        public async Task DeleteUser(string id)
        {
            var user = GetById(id);
            var rolesForUser = await _userManager.GetRolesAsync(user);
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }
                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public List<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public IQueryable<IdentityRole> GetAllRolesWithPages(int page)
        {
            return _context.Roles.Skip((page - 1) * 5).Take(5);
        }

        public IEnumerable<AppUser> GetAllWithPage(int page)
        {
            return _context.Users.Skip((page - 1) * 5).Take(5);
        }

        public AppUser GetById(string id)
        {
            return _context.Users.Where(user => user.Id == id).First();
        }

        public async Task<bool> GetIfUserInRole(AppUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public IdentityRole GetRoleById(string id)
        {
            return _context.Roles.Where(role => role.Id == id).First();
        }

        public async Task<IList<string>> GetUserRoles(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRole(IdentityRole model)
        {
            if(!string.IsNullOrEmpty(model.Name))
            {
                await _roleManager.UpdateAsync(model);
            }
        }

        public async Task UpdateUser(UserModel model)
        {
            var user = GetById(model.UserId);
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.Email = model.Email;
            user.Nickname = model.Nickname;
            user.NormalizedEmail = model.Email.ToUpper();
            user.NormalizedUserName = model.Email.ToUpper();
            user.UserName = model.Email;
            await _context.SaveChangesAsync();
        }

        public async Task UserRoleAdd(string userid, string roleName)
        {
            if(!string.IsNullOrEmpty(roleName))
            {
                var user = GetById(userid);
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        public async Task UserRoleDelete(string userid, string roleName)
        {
            if(!string.IsNullOrEmpty(roleName))
            {
                var user = GetById(userid);
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }
    }
}
