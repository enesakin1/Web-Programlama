using Microsoft.AspNetCore.Authorization;
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
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHtmlLocalizer<PostController> _localizer;
        public PostController(IPost postService, UserManager<AppUser> userManager, IHtmlLocalizer<PostController> localizer)
        {
            _postService = postService;
            _userManager = userManager;
            _localizer = localizer;
        }
        public IActionResult Index(int id, int page = 1)
        {
            ViewData["PostReply"] = _localizer["PostReply"];
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies, page);

            var numberofPages = post.Replies.Count();
            var pagesCount = numberofPages % 10;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 10 + 1;
            }
            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.Nickname,
                AuthorImageUrl = post.User.ProfileImageUrl,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies,
                IsAuthorAdmin = IsAuthorAdmin(post.User),
                NumberOfPage = pagesCount
            };
            return View(model);
        }

        
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SubmitPost"] = _localizer["SubmitPost"];
            ViewData["Title"] = _localizer["Title"];
            ViewData["Content"] = _localizer["Content"];
            var model = new NewPostModel
            {
                AuthorName = User.Identity.Name
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(NewPostModel invalidModel)
        {
            ViewData["SubmitPost"] = _localizer["SubmitPost"];
            ViewData["Title"] = _localizer["Title"];
            ViewData["Content"] = _localizer["Content"];
            return View(invalidModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                var post = BuildPost(model,user);
                _postService.Add(post).Wait();
                return RedirectToAction("Index", "Post", new { id = post.Id });
            }
            ViewData["SubmitPost"] = _localizer["SubmitPost"];
            ViewData["Title"] = _localizer["Title"];
            ViewData["Content"] = _localizer["Content"];
            return View("Create",model);

        }
        private bool IsAuthorAdmin(AppUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }
        [Authorize]
        private Post BuildPost(NewPostModel model,AppUser user)
        {
            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies, int page)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.Nickname,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            }).Skip((page - 1) * 9).Take(9);
        }
    }
}
