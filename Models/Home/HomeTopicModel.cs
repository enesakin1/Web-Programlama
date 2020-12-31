using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class HomeTopicModel
    {
        public IEnumerable<PostListingModel> Posts { get; set; }
    }
}
