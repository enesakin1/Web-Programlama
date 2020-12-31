using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPost _postService;
        private readonly IHtmlLocalizer<SearchController> _localizer;

        public SearchController(IPost postService,IHtmlLocalizer<SearchController> localizer)
        {
            _postService = postService;
            _localizer = localizer;
        }

        public IActionResult Results(string searchQuery, int page = 1)
        {
            ViewData["SearchResultsFor"] = _localizer["SearchResultsFor"];
            ViewData["Replies"] = _localizer["Replies"];
            ViewData["Search"] = _localizer["Search"];
            ViewData["Thereisnoresultfoundfor"] = _localizer["Thereisnoresultfoundfor"];
            var allPosts = _postService.GetAllFilteredPosts(searchQuery);
            var posts = _postService.GetFilteredPosts(searchQuery, page);
            var areNoResults = !posts.Any();
            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorID = post.User.Id,
                Author = post.User.Nickname,
                Title = post.Title,
                DatePosted = post.Created,
                RepliesCount = post.Replies.Count()
            });

            var numberofPages = allPosts;
            var pagesCount = numberofPages % 10;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 10 + 1;
            }

            var model = new SearchResultModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults,
                NumberOfPages = pagesCount
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery});
        }
    }
}
