using System.Collections.Generic;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Util;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

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

        [HttpPost]
        public ActionResult NewTranslator(TranslatorModel model)
        {
            if (model.IsValid())
            {
                return Redirect("/admin/users");
            }

            model.Msg = "bir sorun oluştu...";
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

        [HttpGet]
        public ActionResult Apps()
        {
            var model = _demoDataService.GetAllApps();
            return View(model);
        }
    }

}