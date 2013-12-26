using System;
using System.Collections.Generic;
using SetLocale.Client.Web.Models;
using System.Web.Mvc;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AppController : BaseController
    {
        public AppController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AppModel
            {
                Id = 1,
                AppName = "SetLocale",
                AppDescription = "an application desc.",
                Url = "setlocale.com",
                UsageCount = 1356,
                IsActive = true,
                Tokens = new List<TokenModel>()
            };

            model.Tokens.Add(new TokenModel
            {
                CreationDate = DateTime.Today,
                UsageCount = 2352,
                Token = Guid.NewGuid().ToString().Replace("-", "")
            });
            model.Tokens.Add(new TokenModel
            {
                CreationDate = DateTime.Today.AddDays(-23),
                UsageCount = 34,
                Token = Guid.NewGuid().ToString().Replace("-", "")
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new NewAppModel
            {
                AppName = "AppName",
                Url = "app.com",
                Description = "description"
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(NewAppModel model)
        {
            if (model.IsValid())
            {
                return Redirect("/user/apps");
            }

            model.Msg = "bir sorun oluştu...";
            return View(model);
        }


    }
}