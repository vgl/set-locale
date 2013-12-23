using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}