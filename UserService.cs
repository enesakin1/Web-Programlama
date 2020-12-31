using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Data;
using myiotprojects.Models;
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
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(AuthDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> ChangePassword(string id, ChangePasswordModel passwordModel)
        {
            var user = GetById(id);
            return await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public AppUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
