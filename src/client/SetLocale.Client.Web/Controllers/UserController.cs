using System.Collections.Generic;
using System.Web.Mvc;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Apps()
        {
            var model = _demoDataService.GetUsersApps();
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new NewUserModel
            {
                Name = "Ali Gel",
                Email = "dev@test.com",
                Password = "password"
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewUserModel model)
        {
            if (model.IsValid())
            {
                return Redirect("/user/apps");
            }

            model.Msg = "bir sorun oluştu";
            return View(model);
        }

        [HttpGet]
        public ActionResult Reset()
        {
            var model = new ResetModel()
            {
                Email = "dev@test.com"
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginModel()
            {
                Email = "dev@test.com",
                Password = "password"
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (model.IsValid())
            {
                return Redirect("/user/apps");
            }

            model.Msg = "bir sorun oluştu";
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            

            return RedirectToHome();
        }
    }
}