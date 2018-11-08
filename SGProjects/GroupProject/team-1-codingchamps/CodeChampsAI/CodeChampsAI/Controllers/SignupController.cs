using CodeChampsAI.Data;
using CodeChampsAI.Models;
using CodeChampsAI.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeChampsAI.Controllers
{
    public class SignupController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public SignupController()
        {
        }

        public SignupController(ApplicationUserManager userManager, ApplicationSignInManager signinManager)
        {
            UserManager = userManager;
            SignInManager = signinManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        // GET: Signup
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser userToCreate = new AppUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var createUserResult = UserManager.Create(userToCreate, model.Password);

            if (!createUserResult.Succeeded)
            {
                AddErrors(createUserResult);
                return View(model);
            }

            var addRoleResult = UserManager.AddToRole(userToCreate.Id, "User");

            if (!addRoleResult.Succeeded)
            {
                UserManager.Delete(userToCreate);
                AddErrors(addRoleResult);
                return View(model);
            }

            SignInManager.SignIn(userToCreate, false, false);

            string code = UserManager.GenerateEmailConfirmationToken(userToCreate.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userToCreate.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userToCreate.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return RedirectToAction("Index", "Home", new { SignupSuccess = true });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}