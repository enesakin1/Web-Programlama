using Microsoft.AspNetCore.Mvc;
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

        public SearchController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Results(string searchQuery, int page = 1)
        {
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
