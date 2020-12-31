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
        [Required]
        [MinLength(8, ErrorMessage = "Minimum number of characters that can be entered is 8!")]
        public string Title { get; set; }
        [Required]
        [MinLength(40, ErrorMessage = "Minimum number of characters that can be entered is 40!")]
        public string Content { get; set; }
    }
}
