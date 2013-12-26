using System.Web.Mvc;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}