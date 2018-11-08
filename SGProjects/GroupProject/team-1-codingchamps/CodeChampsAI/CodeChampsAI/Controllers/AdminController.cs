using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeChampsAI.Data;
using CodeChampsAI.Models;
using Ninject;
using CodeChampsAI.Models.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace CodeChampsAI.Controllers
{
    [Authorize(Roles = "Admin, Contributor")]
    public class AdminController : Controller	 
    {
        private IRepository _repo = DIContainer.Kernel.Get<IRepository>();

        private ApplicationUserManager _userManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpGet]
        public ActionResult PostView(int id = 1)
		{
			var repository = DIContainer.Kernel.Get<IRepository>();
            PostsPageViewModel model = new PostsPageViewModel();
            if (User.IsInRole("Contributor"))
            {
                string userName = User.Identity.Name;
                var posts = repository.GetApprovedPostByPageNumberContributor(id, userName);
                model.Posts = posts.Where(x => x.User.UserName == User.Identity.Name).ToList();
                model.PageNumber = id;
                model.MaxPages = _repo.GetTotalNumberOfPagesContributor(userName);
            }
            else
            {
                var posts = repository.GetApprovedPostByPageNumber(id);
                model.Posts = posts;
                model.PageNumber = id;
                model.MaxPages = _repo.GetTotalNumberOfPages();
            }
			return View(model);
		}

        [HttpGet]
        public ActionResult PostEdit(int postId)
		{
			var repository = DIContainer.Kernel.Get<IRepository>();
			var model = repository.GetPostById(postId);
			return View(model);
		}

		[HttpPost]
		public ActionResult PostEdit(PostEditViewModel viewModel)
		{
            var model = new Post { PostId = viewModel.PostId };

			ApprovalEnums autoApproval = ApprovalEnums.Pending;
			if (UserManager.IsInRole(UserManager.FindByName(User.Identity.Name).Id, "Admin"))
			{
				autoApproval = ApprovalEnums.Approved;
			}

			model.ApprovalStatus = autoApproval;
			model.Tags = _repo.TagStringToTags(viewModel.ListOfTags);
			model.Subject = viewModel.Subject;
			model.Body = viewModel.Body;

            _repo.EditPost(model);

			return Redirect("PostView");
		}

		[HttpGet]
		public ActionResult NewPost()
		{
			NewPostViewModel viewModel = new NewPostViewModel();
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult NewPost(NewPostViewModel viewModel)
		{
			var repository = DIContainer.Kernel.Get<IRepository>();
			ApprovalEnums autoApproval = ApprovalEnums.Pending;
			if (UserManager.IsInRole(UserManager.FindByName(User.Identity.Name).Id, "Admin"))
			{
				autoApproval = ApprovalEnums.Approved;
			}

            var tags = repository.TagStringToTags(viewModel.ListOfTags);

            Post newPost = new Post
			{
				ApprovalStatus = autoApproval,
				Body = viewModel.Body,
				Date = DateTime.Now,
				IsFeatured = false,
				Subject = viewModel.Subject,
				Tags = tags,
				User = UserManager.FindByName(User.Identity.Name)
			};
			repository.SaveNewPost(newPost);
			return RedirectToAction("PostView");
		}

		[HttpGet]
		public ActionResult Approval()
		{
            string userName = null;
            if (User.IsInRole("Contributor"))
            {
                userName = User.Identity.Name;
            }
            var repository = DIContainer.Kernel.Get<IRepository>();
            var model = repository.GetPendingPosts(userName);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Users(int pageNumber = 1)
        {
            UserPageViewModel model = new UserPageViewModel();
            model.Users = new List<UsersViewModel>();
            model.MaxPages = _repo.MaxUserPages();
            model.PageNumber = pageNumber;

            var users = _repo.GetUsersByPage(pageNumber);
            var roles = _repo.GetUserRoles();

            foreach (var user in users)
            {
                var userToAdd = new UsersViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    UserRole = roles.Single(r => r.Id == user.Roles.First().RoleId).Name
                };
                model.Users.Add(userToAdd);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.Success = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var user = UserManager.FindByName(User.Identity.Name);

            if (ModelState.IsValid)
            {
                if(user != null)
                {
                    var result = UserManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        model.Success = true;
                    }else
                    {
                        AddErrors(result);
                    }
                }
            }
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Pages()
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            var model = repo.GetAllStaticPages();
            return View(model);
        }

        [HttpGet]
        public ActionResult Rejected()
        {
            string userName = null;
            if (User.IsInRole("Contributor"))
            {
                userName = User.Identity.Name;
            }
            var repo = DIContainer.Kernel.Get<IRepository>();
            var model = repo.GetRejectedPosts(userName);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult PageEdit(int pageId)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            var model = repo.GetStaticPageById(pageId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult PageEdit(StaticPage model)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.EditStaticPage(model);
            return RedirectToAction("Pages");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult NewPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult NewPage(StaticPage model)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.SaveNewStaticPage(model);
            return RedirectToAction("Pages");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.RemoveStaticPage(id);
            return RedirectToAction("Pages");
        }

        [HttpGet]
        public ActionResult PostsRedirect()
        {
            return RedirectToAction("PostView");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult PageRedirect()
        {
            return RedirectToAction("Pages");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ApprovePost(int id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.ApprovePost(id);
            return RedirectToAction("Approval");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RejectPost(int id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.RejectPost(id);
            return RedirectToAction("Approval");
        }

        [HttpGet]
        public ActionResult PostDelete(Post id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.DeletePost(id);
            return RedirectToAction("Approval");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ToggleFeatured(int id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            repo.ToggleFeatured(id);
            return RedirectToAction("PostView");
        }
	}
}