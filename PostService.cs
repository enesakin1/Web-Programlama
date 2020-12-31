using Microsoft.EntityFrameworkCore;
using myiotprojects.Data;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects
{
    public class PostService : IPost 
    {
        private readonly AuthDbContext _context;
        public PostService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.Add(reply);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAllWithPage(int page)
        {
            return _context.Posts.OrderByDescending(post => post.Created).Skip((page - 1) * 10).Take(10).Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User);
        }
        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User).OrderByDescending(post => post.Created);
        }
        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .First();
        }
        public Post GetByIdWithPage(int id, int page)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies).Skip((page - 1) * 9).Take(9).Include(reply => reply.User)
                .First();
        }
        public IEnumerable<Post> GetFilteredPosts(string searchQuery, int page)
        {
            return GetAll().Where(post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery)).Skip((page - 1) * 10).Take(10);
        }

        public IEnumerable<Post> GetLatestPosts(int numberofposts)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(numberofposts);
        }

        public int NumberOfPosts()
        {
            return _context.Posts.Count();
        }

        public int GetAllFilteredPosts(string searchQuery)
        {
            return GetAll().Where(post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery)).Count();
        }

        public List<Post> GetUserPosts(string id)
        {
            return _context.Posts.Include(post => post.User).Where(post => post.User.Id == id).Include(post => post.Replies).ToList();
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
             _context.Remove(post);
           await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetUserPostsForProfile(string id)
        {
            return _context.Posts.Include(post => post.User).Where(post => post.User.Id == id).OrderByDescending(post => post.Created).Take(10).Include(post => post.Replies);
        }

        public async Task DeleteReply(int id)
        {
            var reply = GetReplyById(id);
            _context.Remove(reply);
            await _context.SaveChangesAsync();
        }

        public PostReply GetReplyById(int id)
        {
            return _context.PostReplies.Where(reply => reply.Id == id)
                 .FirstOrDefault();
        }

        public List<PostReply> GetAllUserReplies(string id)
        {
            return _context.PostReplies.Where(reply => reply.User.Id == id).ToList();
        }

        public async Task UpdatePost(Post model)
        {
            _context.Posts.Update(model);
            await _context.SaveChangesAsync();
        }

        public List<PostReply> GetAllReplies()
        {
            return _context.PostReplies.ToList();
        }

        public List<PostReply> GetAllRepliesWithPage(int page)
        {
            return _context.PostReplies.Skip((page - 1) * 10).Take(10).Include(reply => reply.User)
                .Include(reply => reply.Post).ToList();
        }

        public async Task UpdateReply(PostReply model)
        {
            _context.PostReplies.Update(model);
            await _context.SaveChangesAsync();
        }

    }
}
