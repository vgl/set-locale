using System.Web.Mvc;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(
            IFormsAuthenticationService formsAuthenticationService, 
            IDemoDataService demoDataService) 
            : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeStatsModel
            {
                ApplicationCount = 5,
                DeveloperCount = 3,
                TranslatorCount = 2,
                KeyCount = 154,
                TranslationCount = 654
            };

            model.Summary = string.Format(ViewBag.Txt["home_summary"],
                model.DeveloperCount,
                model.ApplicationCount,
                model.TranslatorCount,
                model.KeyCount,
                model.TranslationCount);

            return View(model);
        }
    }
}