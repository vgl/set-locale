using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
    }
}