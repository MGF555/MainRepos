using CodeChampsAI.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeChampsAI.Models
{
    public class UsersViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
    }
}