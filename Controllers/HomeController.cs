using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPost _postService;

        public HomeController(ILogger<HomeController> logger, IPost postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public IActionResult Index(int page = 1)
        {
            var model = BuildHomeIndex(page);
            return View(model);
        }

        private HomeIndexModel BuildHomeIndex(int page)
        {
            var allPosts = _postService.GetAllWithPage(page);
            var posts = allPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorID = post.User.Id,
                Author = post.User.Nickname,
                DatePosted = post.Created,
                RepliesCount = post.Replies.Count(),
                AuthorProfileImage = post.User.ProfileImageUrl
            });

            var numberofPages = _postService.NumberOfPosts();
            var pagesCount = numberofPages % 10;
            if(pagesCount > 0)
            {
                pagesCount = numberofPages / 10 + 1;
            }
            return new HomeIndexModel
            {
                AllPosts = posts,
                SearchQuery = "",
                NumberOfPages = pagesCount
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
