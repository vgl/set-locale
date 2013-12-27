using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewTranslator()
        {
            var model = _demoDataService.GetAUser();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewTranslator(UserModel model)
        {
            if (model.IsValidForNewTranslator())
            {

                return Redirect("/admin/users");
            }

            model.Msg = "bir sorun oluştu...";
            return View(model);
        }

        [HttpGet]
        public ActionResult Users()
        {
            var model = _demoDataService.GetAllUsers();
            return View(model);
        }

        [HttpGet]
        public ActionResult Apps()
        {
            var model = _demoDataService.GetAllApps();
            return View(model);
        }
    }

}