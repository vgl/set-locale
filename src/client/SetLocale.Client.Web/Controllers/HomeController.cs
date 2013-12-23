using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}