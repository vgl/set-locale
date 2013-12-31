using System.Threading.Tasks;
using System.Web.Mvc;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(
            IUserService userService,
            IFormsAuthenticationService formsAuthenticationService,
            IDemoDataService demoDataService)
            : base(formsAuthenticationService, demoDataService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewTranslator()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTranslator(UserModel model)
        {
            if (!model.IsValidForNewTranslator())
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            var userId = await _userService.Create(model, SetLocaleRole.Translator.Value);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            return Redirect("/admin/users");
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