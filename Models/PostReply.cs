using myiotprojects.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class PostReply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}
