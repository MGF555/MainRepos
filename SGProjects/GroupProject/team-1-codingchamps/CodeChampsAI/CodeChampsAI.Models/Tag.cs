using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeChampsAI.Models
{
    public class Tag
    {
        public int TagId { get; set; }

		[AllowHtml]
        public string TagName { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}
