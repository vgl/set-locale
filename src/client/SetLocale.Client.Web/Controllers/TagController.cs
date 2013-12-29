using System.Web.Mvc;

using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class TagController : BaseController
    {
        public TagController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ViewResult Index(string id = "set-locale")
        {
            var model = _demoDataService.GetMyKeys();
            return View(model);
        }
    }
}