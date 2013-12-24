using SetLocale.Client.Web.Models;
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
            var model = new NewKeyModel
            {
                Key = "",
                Tag = "",
                Description = "Description"
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }
    }
}