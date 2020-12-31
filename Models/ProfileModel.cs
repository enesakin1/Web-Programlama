using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime ProfileCreated { get; set; }
        public IEnumerable<Post> AllPosts { get; set; }
        public ChangePasswordModel ChangePasswordModel { get; set; }
    }
}
