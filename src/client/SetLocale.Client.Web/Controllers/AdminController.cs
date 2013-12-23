using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}