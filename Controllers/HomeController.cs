using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
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
        private readonly IPost _postService;
        private readonly IHtmlLocalizer<HomeController> _localizer;

        public HomeController(IPost postService, IHtmlLocalizer<HomeController> localizer)
        {
            _postService = postService;
            _localizer = localizer;
        }

        public IActionResult Index(int page = 1)
        {
            ViewData["NewThread"] = _localizer["NewThread"];
            ViewData["DaysAgo"] = _localizer["DaysAgo"];
            ViewData["Topics"] = _localizer["Topics"];
            ViewData["Replies"] = _localizer["Replies"];
            ViewData["Author"] = _localizer["Author"];
            ViewData["Search"] = _localizer["Search"];
            ViewData["By"] = _localizer["By"];
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
        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
