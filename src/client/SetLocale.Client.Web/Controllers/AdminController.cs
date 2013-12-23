using System.Web.Mvc;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewTranslator()
        {
            var model = new TranslatorModel()
            {
                Email = "user@test.com",
                Name = "Translator"
            };
            return View(model);
        }
    }
}