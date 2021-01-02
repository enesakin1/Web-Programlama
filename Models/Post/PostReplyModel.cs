using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class PostReplyModel
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public DateTime Created { get; set; }
        [Required(ErrorMessageResourceName = "ContentRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        [MinLength(16, ErrorMessageResourceName = "ContentValidation", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string ReplyContent { get; set; }
        public int PostId { get; set; }
        public bool IsAuthorAdmin { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
    }
}
