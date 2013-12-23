using SetLocale.Client.Web.Models;
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
            var model = new NewAppModel
            {
                AppName = "AppName",
                Url = "",
                Description = "Description"
            };

            return View(model);
        }

       
    }
}