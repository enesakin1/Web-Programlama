using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Controllers
{
    [Authorize]
    public class ReplyController : Controller
    {
        private readonly IPost _postService;
        private static UserManager<AppUser> _userManager;

        public ReplyController(IPost postService, UserManager<AppUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,
                AuthorName = user.Nickname,
                AuthorImageUrl = user.ProfileImageUrl,
                IsAuthorAdmin = User.IsInRole("Admin"),
                Created = DateTime.Now
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(PostReplyModel invalidModel)
        {
            return View(invalidModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                var reply = BuildReply(model, user);

                await _postService.AddReply(reply);

                return RedirectToAction("Index", "Post", new { id = model.PostId });

            }
            return View("Create", model);
        }

        private PostReply BuildReply(PostReplyModel model, AppUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}
