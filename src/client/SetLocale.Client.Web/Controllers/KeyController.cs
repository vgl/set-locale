using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Util;

namespace SetLocale.Client.Web.Controllers
{
    public class KeyController : BaseController
    {
        public KeyController(
            IFormsAuthenticationService formsAuthenticationService, 
            IDemoDataService demoDataService) 
            : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Detail()
        {
            var model = _demoDataService.GetAKey();
            return View(model);
        }

        [HttpGet]
        public ActionResult My()
        {
            var model = _demoDataService.GetMyKeys();
            return View(model);
        }

        [HttpGet]
        public ActionResult All()
        {
            var model = _demoDataService.GetAllKeys();
            return View(model);
        }

        [HttpGet]
        public ActionResult NotTranslated()
        {
            var model = _demoDataService.GetNotTranslatedKeys();
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = _demoDataService.GetAKey();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id, string lang = ConstHelper.tr)
        {
            var model = new TranslationModel
            {
                Key = "sign_up",
                LanguageImageUrl = "/public/img/tr.png",
                Tags = _demoDataService.GetSomeTag()
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TranslationModel model)
        {
            if (model.IsValid())
            {
                
            }

            return View(model);
        }
    }
}