using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeChampsAI.Models
{
    public class UserPageViewModel
    {
        public int PageNumber { get; set; }
        public int MaxPages { get; set; }
        public List<UsersViewModel> Users { get; set; }
    }
}