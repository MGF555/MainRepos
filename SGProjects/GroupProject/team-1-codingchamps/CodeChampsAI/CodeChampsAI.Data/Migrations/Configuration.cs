namespace CodeChampsAI.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Identity;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeChampsAI.Data.Identity.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CodeChampsAI.Data.Identity.ApplicationDbContext context)
        {
            var userMgr = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));

            if (!roleMgr.RoleExists("Admin"))
            {
                roleMgr.Create(new AppRole { Name = "Admin" });
            }
            if (!roleMgr.RoleExists("Contributor"))
            {
                roleMgr.Create(new AppRole { Name = "Contributor" });
            }
            if (!roleMgr.RoleExists("User"))
            {
                roleMgr.Create(new AppRole { Name = "User" });
            }
            if (!roleMgr.RoleExists("Disabled"))
            {
                roleMgr.Create(new AppRole { Name = "Disabled" });
            }

            List<string> userEmails = new List<string>()
            { "Adam@mail.com", "Connie@mail.com",
            "Simple@mail.com", "Dan@mail.com"};

            foreach(var user in userEmails)
            {
                if (userMgr.FindByEmail(user) != null)
                {
                    userMgr.Delete(userMgr.FindByEmail(user));
                }
            }

            var adminUser = new AppUser
            {
                FirstName = "Adam",
                LastName = "Admin",
                UserName = "Adam@mail.com",
                Email = "Adam@mail.com"
            };

            var contribUser = new AppUser
            {
                FirstName = "Connie",
                LastName = "Contributor",
                UserName = "Connie@mail.com",
                Email = "Connie@mail.com"
            };

            var userUser = new AppUser
            {
                FirstName = "Simple",
                LastName = "User",
                UserName = "Simple@mail.com",
                Email = "Simple@mail.com"
            };

            var disabledUser = new AppUser
            {
                FirstName = "Dan",
                LastName = "Disabled",
                UserName = "Dan@mail.com",
                Email = "Dan@mail.com"
            };

            userMgr.Create(adminUser, "admin123");
            userMgr.AddToRole(adminUser.Id, "Admin");

            userMgr.Create(contribUser, "contrib123");
            userMgr.AddToRole(contribUser.Id, "Contributor");

            userMgr.Create(userUser, "user123");
            userMgr.AddToRole(userUser.Id, "User");

            userMgr.Create(disabledUser, "disabled123");
            userMgr.AddToRole(disabledUser.Id, "Disabled");

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
