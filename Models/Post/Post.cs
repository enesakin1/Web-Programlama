﻿using myiotprojects.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace myiotprojects.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Title { get; set; }
        [Required(ErrorMessageResourceName = "ContentRequired", ErrorMessageResourceType = typeof(Resources.ValidationErrors))]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public virtual AppUser User { get; set; }
        public virtual IEnumerable<PostReply> Replies { get; set; }
    }
}
