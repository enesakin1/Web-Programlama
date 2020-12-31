using myiotprojects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myiotprojects
{
    public interface IPost
    {
        Post GetById(int id);
        Task UpdateReply(PostReply model);
        Task UpdatePost(Post model);
        List<PostReply> GetAllReplies();
        List<PostReply> GetAllRepliesWithPage(int page);
        PostReply GetReplyById(int id);
        List<PostReply> GetAllUserReplies(string id);
        Post GetByIdWithPage(int id, int page);
        IEnumerable<Post> GetAll();
        List<Post> GetUserPosts(string id);
        IEnumerable<Post> GetUserPostsForProfile(string id);
        IEnumerable<Post> GetAllWithPage(int page);
        int NumberOfPosts();
        IEnumerable<Post> GetFilteredPosts(string searchQuery,int page);
        int GetAllFilteredPosts(string searchQuery);
        IEnumerable<Post> GetLatestPosts(int numberofposts);
        Task Add(Post post);
        Task Delete(int id);
        Task DeleteReply(int id);
        Task AddReply(PostReply reply);

    }
}
