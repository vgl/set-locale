using System.Collections.Generic;
using System.Web.Mvc;
using SetLocale.Client.Web.Models;
using SetLocale.Util;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewTranslator()
        {
            var model = new TranslatorModel()
            {
                Email = "user@test.com",
                Name = "Translator"
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Users()
        {
            var model = new List<UserModel>();
            model.Add(new UserModel
            {
                Id = 1,
                Email = "admin@test.com",
                Name = "Admin",
                Role = ConstHelper.Admin,
                IsActive = true
            });
            model.Add(new UserModel
            {
                Id = 2,
                Email = "dev@test.com",
                Name = "Developer",
                Role = ConstHelper.Developer,
                IsActive = true
            });
            model.Add(new UserModel
            {
                Id = 3,
                Email = "user@test.com",
                Name = "Translator",
                Role = ConstHelper.User,
                IsActive = true
            });

            return View(model);
        }
    }

}