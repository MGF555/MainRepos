using CodeChampsAI.Models.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeChampsAI.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public bool IsFeatured { get; set; }
        public ApprovalEnums ApprovalStatus { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }

		[AllowHtml]
		public string Body { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
