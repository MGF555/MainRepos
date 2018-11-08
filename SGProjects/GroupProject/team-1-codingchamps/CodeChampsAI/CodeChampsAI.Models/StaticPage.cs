using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeChampsAI.Models
{
    public class StaticPage
    {
        public int StaticPageId { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string LinkName { get; set; }
    }
}
