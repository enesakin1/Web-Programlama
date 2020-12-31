using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class AdminPostIndexModel
    {
        public IEnumerable<PostListingModel> AllPosts { get; set; }
        public int NumberOfPages { get; set; }
    }

}
