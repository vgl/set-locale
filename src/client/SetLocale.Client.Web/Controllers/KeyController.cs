using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class KeyController : BaseController
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
        public ActionResult Edit()
        {
            return View();
        }
    }
}