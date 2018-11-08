using CodeChampsAI.Data;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeChampsAI.Controllers
{
    public class StaticController : Controller
    {
        // GET: Static
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult View(int id)
        {
            var repo = DIContainer.Kernel.Get<IRepository>();
            var model = repo.GetStaticPageById(id);
            return View(model);
        }
    }
}