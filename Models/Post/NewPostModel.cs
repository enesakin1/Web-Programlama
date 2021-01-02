using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects.Models
{
    public class NewPostModel
    {
        public string AuthorName { get; set; }
        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        [MinLength(8, ErrorMessageResourceName = "TitleValidation", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Title { get; set; }
        [Required(ErrorMessageResourceName = "ContentRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        [MinLength(40, ErrorMessageResourceName = "PostContentValidation", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Content { get; set; }
    }
}
