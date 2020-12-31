using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace myiotprojects.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUser _userService;
        private readonly IPost _postService;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager, IUser userService, IPost postService)
        {
            _userManager = userManager;
            _userService = userService;
            _postService = postService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel passmodel)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userService.GetById(userId);
            var posts = _postService.GetUserPosts(userId);
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.Nickname,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                AllPosts = posts,
                ProfileCreated = user.Created,
                ChangePasswordModel = passmodel
            };
            if (ModelState.IsValid)
            {
               var changePasswordResult = _userService.ChangePassword(userId, passmodel);
               
                if (!changePasswordResult.IsCompleted)
                {
                    foreach (var error in changePasswordResult.Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View("Detail", model);
            }
            return View("Detail", model);

        }
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var posts = _postService.GetUserPosts(id);
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.Nickname,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                AllPosts = posts,
                ProfileCreated = user.Created
            };
            return View(model);
        }
    }
}
