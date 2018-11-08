using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeChampsAI.Models
{
    public class PostsPageViewModel
    {
        public int PageNumber { get; set; }
        public int MaxPages { get; set; }
        public List<Post> Posts { get; set; }
        public string SearchTerm { get; set; }
    }
}