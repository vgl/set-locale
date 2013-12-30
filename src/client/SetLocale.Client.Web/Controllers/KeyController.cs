using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

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
        public ViewResult Detail()
        {
            var model = _demoDataService.GetAKey();
            return View(model);
        }

        [HttpGet]
        public ViewResult My()
        {
            var model = _demoDataService.GetMyKeys();
            return View(model);
        }

        [HttpGet]
        public ViewResult All()
        {
            var model = _demoDataService.GetAllKeys();
            return View(model);
        }

        [HttpGet]
        public ViewResult NotTranslated()
        {
            var model = _demoDataService.GetNotTranslatedKeys();
            return View(model);
        }

        [HttpGet]
        public ViewResult New()
        {
            var model = _demoDataService.GetAKey();
            return View(model);
        }

        [HttpGet]
        public ViewResult Edit(string id, string lang = ConstHelper.tr)
        {
            var model = _demoDataService.GetATranslation();
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