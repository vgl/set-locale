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
            var model = _demoDataService.GetAnApp();
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = _demoDataService.GetAnApp();
            return View(model);
        }

        [HttpPost]
        public ActionResult New(AppModel model)
        {
            if (model.IsValidForNew())
            {

                return Redirect("/user/apps");
            }

            model.Msg = "bir sorun oluştu...";
            return View(model);
        }


    }
}