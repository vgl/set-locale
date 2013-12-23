using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class AppController : BaseController
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

       
    }
}