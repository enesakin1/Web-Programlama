using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Models;
using myiotprojects.Models.Admin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUser _userService;
        private readonly IPost _postService;
        private readonly IHtmlLocalizer<AdminController> _localizer;
        public AdminController(UserManager<AppUser> userManager, IUser userService, IPost postService, IHtmlLocalizer<AdminController> localizer)
        {
            _userService = userService;
            _postService = postService;
            _localizer = localizer;
        }
        public ActionResult Index ()
        {
            return RedirectToAction("Users");
        }
        public ActionResult Users(int page = 1)
        {
            ViewData["Manage"] = _localizer["Manage"];
            ViewData["Name"] = _localizer["Name"];
            ViewData["Created"] = _localizer["Created"];
            ViewData["Role"] = _localizer["Role"];
            ViewData["Actions"] = _localizer["Actions"];
            ViewData["Create"] = _localizer["Create"];
            ViewData["User"] = _localizer["User"];
            ViewData["Add/Delete"] = _localizer["Add/Delete"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
            ViewData["Sure"] = _localizer["Sure"];
            ViewData["Undone"] = _localizer["Undone"];
            ViewData["Cancel"] = _localizer["Cancel"];

            var allUsers = _userService.GetAll();
            var user = _userService.GetAllWithPage(page);
            var users = user.Select(user => new UserModel
            {
                UserId = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                Created = user.Created
            });
            var numberofPages = allUsers.Count();
            var pagesCount = numberofPages % 5;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 5 + 1;
            }
            var model = new UserIndexModel
            {
                AllUsers = users,
                NumberOfPages = pagesCount
            };
            
            return View(model);
        }
        [HttpGet]
        public ActionResult UserEdit(string userid)
        {
            ViewData["Edit"] = _localizer["Manage"];
            ViewData["User"] = _localizer["User"];
            ViewData["Nickname"] = _localizer["Nickname"];
            ViewData["ProfileImageUrl"] = _localizer["ProfileImageUrl"];
            var user = _userService.GetById(userid);
            var selecteduser = new UserModel()
            {
                UserId = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(selecteduser);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(UserModel model)
        {
            await _userService.UpdateUser(model);
            return RedirectToAction("Users");
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDelete(string userid)
        {
            var userposts = _postService.GetUserPosts(userid);
            var userreplies = _postService.GetAllUserReplies(userid);
            foreach (var reply in userreplies)
            {
                await _postService.DeleteReply(reply.Id);
            }
            foreach (var post in userposts)
            {
                foreach(var reply in post.Replies)
                {
                    await _postService.DeleteReply(reply.Id);
                }
                await _postService.Delete(post.Id);
            }
            await _userService.DeleteUser(userid);
            return RedirectToAction("Users");
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewData["Nickname"] = _localizer["Nickname"];
            ViewData["Password"] = _localizer["Password"];
            ViewData["Create"] = _localizer["Create"];
            ViewData["User"] = _localizer["User"];
            ViewData["Save"] = _localizer["Save"];
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserModel user)
        {
            if (user.ProfileImageUrl == null)
            {
                user.ProfileImageUrl = "/images/user/user.png";
            }
            AppUser newuser = new AppUser
            {
                Email = user.Email,
                Nickname = user.Nickname,
                UserName = user.Email,
                NormalizedUserName = user.Email.ToUpper(),
                NormalizedEmail = user.Email.ToUpper(),
                ProfileImageUrl = user.ProfileImageUrl
            };

            await _userService.CreateUser(newuser, user.Password);
            return RedirectToAction("Users");
        }
        public ActionResult Posts(int page = 1)
        {
            ViewData["Manage"] = _localizer["Manage"];
            ViewData["Posts"] = _localizer["Posts"];
            ViewData["Created"] = _localizer["Created"];
            ViewData["Title"] = _localizer["Title"];
            ViewData["Author"] = _localizer["Author"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
            ViewData["Sure"] = _localizer["Sure"];
            ViewData["Undone"] = _localizer["Undone"];
            ViewData["Cancel"] = _localizer["Cancel"];
            ViewData["Post"] = _localizer["Post"];

            var allPosts = _postService.GetAll();
            var post = _postService.GetAllWithPage(page);
            var posts = post.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                DatePosted = post.Created,
                Author = post.User.Nickname
            });
            var numberofPages = allPosts.Count();
            var pagesCount = numberofPages % 10;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 10 + 1;
            }
            var model = new AdminPostIndexModel
            {
                AllPosts = posts,
                NumberOfPages = pagesCount
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult PostEdit(int postid)
        {

            ViewData["Title"] = _localizer["Title"];
            ViewData["Content"] = _localizer["Content"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Save"] = _localizer["Save"];
            ViewData["Post"] = _localizer["Post"];
            var post = _postService.GetById(postid);
            var selectedpost = new Post()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
            return View(selectedpost);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostEdit(Post model)
        {
            var post = _postService.GetById(model.Id);
            post.Title = model.Title;
            post.Content = model.Content;
            await _postService.UpdatePost(post);
            return RedirectToAction("Posts");
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostDelete(int postid)
        {
            var post = _postService.GetById(postid);
            foreach (var reply in post.Replies.ToList())
            {
                await _postService.DeleteReply(reply.Id);
            };
            await _postService.Delete(postid);
            return RedirectToAction("Posts");
        }
        public ActionResult PostReplies(int page = 1)
        {
            ViewData["Manage"] = _localizer["Manage"];
            ViewData["Replies"] = _localizer["Replies"];
            ViewData["PostTitle"] = _localizer["PostTitle"];
            ViewData["Created"] = _localizer["Created"];
            ViewData["Author"] = _localizer["Author"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
            ViewData["Sure"] = _localizer["Sure"];
            ViewData["Undone"] = _localizer["Undone"];
            ViewData["Cancel"] = _localizer["Cancel"];
            ViewData["Reply"] = _localizer["Reply"];

            var allReplies = _postService.GetAllReplies();
            var reply = _postService.GetAllRepliesWithPage(page);
            var replies = reply.Select(reply => new PostReply
            {
               Id = reply.Id,
               Created = reply.Created,
               User = reply.User,
               Post = reply.Post,
               Content = reply.Content
            });
            var numberofPages = allReplies.Count();
            var pagesCount = numberofPages % 10;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 10 + 1;
            }
            var model = new ReplyIndexModel
            {
                AllReplies = replies,
                NumberOfPages = pagesCount
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult ReplyEdit(int replyid)
        {
            var reply = _postService.GetReplyById(replyid);
            return View(reply);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReplyEdit(PostReply model)
        {

            ViewData["Content"] = _localizer["Content"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Reply"] = _localizer["Reply"];
            ViewData["Save"] = _localizer["Save"];
            var reply = _postService.GetReplyById(model.Id);
            reply.Content = model.Content;
            await _postService.UpdateReply(reply);
            return RedirectToAction("PostReplies");
        }
        [HttpPost]
        public async Task<ActionResult> ReplyDelete(int replyid)
        {
            await _postService.DeleteReply(replyid);
            return RedirectToAction("PostReplies");
        }
        public ActionResult Roles(int page = 1)
        {
            ViewData["Manage"] = _localizer["Manage"];
            ViewData["Roles"] = _localizer["Roles"];
            ViewData["Name"] = _localizer["Name"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
            ViewData["Create"] = _localizer["Create"];
            ViewData["Role"] = _localizer["Role"];
            ViewData["Sure"] = _localizer["Sure"];
            ViewData["Undone"] = _localizer["Undone"];
            ViewData["Cancel"] = _localizer["Cancel"];

            var allRoles = _userService.GetAllRoles();
            var role = _userService.GetAllRolesWithPages(page);
            var roles = role.Select(role => new Role
            {
                Id = role.Id,
                Name = role.Name
            });
            var numberofPages = allRoles.Count();
            var pagesCount = numberofPages % 5;
            if (pagesCount > 0)
            {
                pagesCount = numberofPages / 5 + 1;
            }
            var model = new RoleIndexModel
            {
                AllRoles = roles,
                NumberOfPages = pagesCount
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateRole()
        {
            ViewData["Name"] = _localizer["Name"];
            ViewData["Save"] = _localizer["Save"];
            ViewData["Create"] = _localizer["Create"];
            ViewData["Role"] = _localizer["Role"];
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole(Role rol)
        {
            await _userService.CreateRole(rol.Name);
            return RedirectToAction("Roles");
        }
        [HttpGet]
        public ActionResult RoleEdit(string roleid)
        {
            ViewData["Name"] = _localizer["Name"];
            ViewData["Save"] = _localizer["Save"];
            ViewData["Create"] = _localizer["Create"];
            ViewData["Role"] = _localizer["Role"];
            var role = _userService.GetRoleById(roleid);
            var selectedrole = new Role()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(selectedrole);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleEdit(Role model)
        {
            var role = _userService.GetRoleById(model.Id);
            role.Name = model.Name;
            await _userService.UpdateRole(role);
            return RedirectToAction("Roles");
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RoleDelete(string roleid)
        {
            var role = _userService.GetRoleById(roleid);
            await _userService.DeleteRole(role);
            return RedirectToAction("Roles");
        }
        public async Task<ActionResult> ManageUserRole(string userid)
        {
            ViewData["Add"] = _localizer["Add"];
            ViewData["Delete"] = _localizer["Delete"];
            ViewData["Save"] = _localizer["Save"];
            ViewData["Role"] = _localizer["Role"];
            var roles = _userService.GetAllRoles();
            var appuser = _userService.GetById(userid);
            var user = new UserModel
            {
                UserId = appuser.Id
            };
            user.Roles = new List<IdentityRole>();
            foreach (var role in roles.ToList())
            {
                if(await _userService.GetIfUserInRole(appuser,role.Name))
                {
                    user.Roles.Add(role);
                    roles.Remove(role);
                }
            }
            var model = new UserRoleModel
            {
                user = user,
                roles = roles
            };
            return View(model);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoleAdd(string userid, string roleName)
        {
            await _userService.UserRoleAdd(userid, roleName);
            return RedirectToAction("ManageUserRole", new { userid = userid });
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoleDelete(string userid, string roleName)
        {
            await _userService.UserRoleDelete(userid, roleName);
            return RedirectToAction("ManageUserRole", new { userid = userid });
        }
    }
}
