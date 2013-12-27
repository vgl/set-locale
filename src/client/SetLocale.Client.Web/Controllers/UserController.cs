using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(
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
        public ActionResult Apps()
        {
            var model = _demoDataService.GetUsersApps();
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = _demoDataService.GetAUser();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(UserModel model)
        {
            if (!model.IsValidForNewDeveloper())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            var userId = _userService.Create(model);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            return Redirect("/user/apps");
        }

        [HttpGet]
        public ActionResult Reset()
        {
            var model = new ResetModel()
            {
                Email = "dev@test.com"
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginModel()
            {
                Email = "dev@test.com",
                Password = "password"
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (model.IsValid())
            {
                return Redirect("/user/apps");
            }

            model.Msg = "bir sorun oluştu";
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _formsAuthenticationService.SignOut();
            return RedirectToHome();
        }
    }
}