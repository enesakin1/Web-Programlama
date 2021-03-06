﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
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
        private readonly IHtmlLocalizer<ReplyController> _localizer;

        public ReplyController(IPost postService, UserManager<AppUser> userManager, IHtmlLocalizer<ReplyController> localizer)
        {
            _postService = postService;
            _userManager = userManager;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            ViewData["BackToPost"] = _localizer["BackToPost"];
            ViewData["Reply"] = _localizer["Reply"];
            ViewData["SubmitReply"] = _localizer["SubmitReply"];
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            ViewData["BackToPost"] = _localizer["BackToPost"];
            ViewData["Reply"] = _localizer["Reply"];
            ViewData["SubmitReply"] = _localizer["SubmitReply"];
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
