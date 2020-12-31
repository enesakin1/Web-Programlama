using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models.Admin
{
    public class ReplyIndexModel
    {
        public IEnumerable<PostReply> AllReplies { get; set; }
        public int NumberOfPages { get; set; }
    }
}
