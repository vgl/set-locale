using System.Collections.Generic;
using System.Web.Mvc;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Apps()
        {
            var model = new List<AppModel>();
            model.Add(new AppModel
            {
                Id = 1,
                AppName = "SetLocale",
                AppDescription = "an application desc.",
                Url = "setlocale.com",
                UsageCount = 1356,
                IsActive = true
            });
            model.Add(new AppModel
            {
                Id = 2,
                AppName = "SetCrm",
                AppDescription = "an application desc.",
                Url = "setcrm.com",
                UsageCount = 64212,
                IsActive = true
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new NewUserModel
            {
                UserName = "UserName",
                Email = "dev@test.com",
                Password = "password" 
            };

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
    }
}