using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
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
        private readonly IHtmlLocalizer<ProfileController> _localizer;

        public ProfileController(UserManager<AppUser> userManager, IUser userService, IPost postService, IHtmlLocalizer<ProfileController> localizer)
        {
            _userService = userService;
            _postService = postService;
            _localizer = localizer;
        }
        public IActionResult Detail(string id)
        {
            ViewData["Settings"] = _localizer["Settings"];
            ViewData["MemberSince"] = _localizer["MemberSince"];
            ViewData["Last"] = _localizer["Last"];
            ViewData["Posts"] = _localizer["Posts"];
            ViewData["Replies"] = _localizer["Replies"];
            ViewData["AddProfileImage"] = _localizer["AddProfileImage"];
            ViewData["Submit"] = _localizer["Submit"];
            ViewData["Cancel"] = _localizer["Cancel"];

            var user = _userService.GetById(id);
            var posts = _postService.GetUserPostsForProfile(id);
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
        [HttpPost]
        public async Task<IActionResult> AddPhoto(string userid, string photourl)
        {
            await _userService.AddPhoto(userid, photourl);
            return RedirectToAction("Detail", new { id = userid });
        }
    }
}
