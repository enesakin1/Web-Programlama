using myiotprojects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myiotprojects
{
    public interface IPost
    {
        Post GetById(int id);
        Post GetByIdWithPage(int id, int page);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetUserPosts(string id);
        IEnumerable<Post> GetAllWithPage(int page);
        int NumberOfPosts();
        IEnumerable<Post> GetFilteredPosts(string searchQuery,int page);
        int GetAllFilteredPosts(string searchQuery);
        IEnumerable<Post> GetLatestPosts(int numberofposts);
        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string newContent);
        Task AddReply(PostReply reply);

    }
}
