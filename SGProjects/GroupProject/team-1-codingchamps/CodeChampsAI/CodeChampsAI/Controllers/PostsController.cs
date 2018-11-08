using CodeChampsAI.Data;
using CodeChampsAI.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeChampsAI.Controllers
{
    [AllowAnonymous]
    public class PostsController : Controller
    {
        private static IRepository _repo;

        static PostsController()
        {
            _repo = DIContainer.Kernel.Get<IRepository>();
        }

        // GET: Post
        public ActionResult Index(int id = 1)
        {
            var posts = _repo.GetApprovedPostByPageNumber(id);

            PostsPageViewModel model = new PostsPageViewModel();
            model.Posts = posts;
            model.PageNumber = id;
            model.MaxPages = _repo.GetTotalNumberOfPages();

            return View(model);
        }

        public ActionResult View(int id)
        {
            var model = _repo.GetPostById(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Search(int? id, string searchTerm)
        {
            PostsPageViewModel model = new PostsPageViewModel();

            if(id == null)
            {
                id = 1;
            }

            model.PageNumber = id.Value;
            model.Posts = _repo.SearchResults(id, searchTerm);
            model.MaxPages = _repo.GetPostMaxPagesBySearchTerm(searchTerm);
            model.SearchTerm = searchTerm;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                return View(model);
            }
            else
            {
                return View(new PostsPageViewModel {Posts = new List<Post>() });
            }
        }

        [HttpGet]
        public ActionResult Tag(int? id)
        {
            var model = _repo.GetPostByTagId(id);
            return View(model);
        }
    }
}