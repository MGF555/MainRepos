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
    public class HomeController : Controller
    {
        private static IRepository _repo;

        static HomeController()
        {
            _repo = DIContainer.Kernel.Get<IRepository>();
        }


        // GET: Home
        public ActionResult Index()
        {
            var model = _repo.GetFeaturedPosts();

            return View(model);
        }

        public PartialViewResult Sidebar()
        {
            List<StaticPage> model = _repo.GetAllStaticPages();
            return PartialView("~/Views/Shared/_Sidebar.cshtml", model);
        }
    }
}