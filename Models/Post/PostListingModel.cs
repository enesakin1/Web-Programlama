using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class PostListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorID { get; set; }
        public DateTime DatePosted { get; set; }
        public int RepliesCount { get; set; }

        public string AuthorProfileImage { get; set; }
    }
}
