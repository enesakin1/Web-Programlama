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

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Post> GetUserPosts(string id)
        {
            return _context.Posts.Include(post => post.User).Where(post => post.User.Id == id).OrderByDescending(post => post.Created).Take(10).Include(post => post.Replies);
        }
    }
}
