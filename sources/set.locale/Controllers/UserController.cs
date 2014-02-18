using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IAppService _appService;
        private readonly IWordService _wordService;

        public UserController(
            IAuthService authService,
            IUserService userService, IAppService appService, IWordService wordService)
        {
            _authService = authService;
            _userService = userService;
            _appService = appService;
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<ActionResult> Detail()
        {
            var result = await _userService.Get(User.Identity.GetId());

            if (result == null) return RedirectToHome();

            var model = UserModel.Map(result);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Apps(string id, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.GetId();
            }

            ViewBag.UserId = id;

            var apps = await _appService.GetByUserId(id);

            if (apps == null)
            {
                return RedirectToHome();
            }

            var model = apps.Select(AppModel.Map);

            return View(model);
        }

        public async Task<ViewResult> Words(string id, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.GetId();
            }


            ViewBag.UserId = id;


            var words = await _wordService.GetByUserId(id, pageNumber);
            var list = words.Items.Select(WordModel.Map).ToList();


            var model = new PageModel<WordModel>
            {
                Items = list,
                HasNextPage = words.HasNextPage,
                HasPreviousPage = words.HasPreviousPage,
                Number = words.Number,
                TotalCount = words.TotalCount,
                TotalPageCount = words.TotalPageCount
            };


            return View(model);
        }



        #region Membership
        [HttpGet, AllowAnonymous]
        public ActionResult New()
        {
            return View(new UserModel());
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> New(UserModel model)
        {
            if (!model.IsValid())
            {
                SetPleaseTryAgain(model);
                return View(model);
            }

            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            model.Id = Guid.NewGuid().ToNoDashString();
            var status = await _userService.Create(model, ConstHelper.User);
            if (!status)
            {
                SetPleaseTryAgain(model);
                return View(model);
            }

            _authService.SignIn(model.Id, model.Name, model.Email, ConstHelper.User, true);

            TempData["newMember"] = "new_member_create_app_message".Localize();

            return Redirect("/app/new");
        }

        [HttpGet, AllowAnonymous]
        public ActionResult PasswordReset()
        {
            var model = new PasswordResetModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Email = User.Identity.GetEmail();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> PasswordReset(PasswordResetModel model)
        {
            SetPleaseTryAgain(model);
            if (model.IsNotValid())
            {
                return View(model);
            }

            var isOk = await _userService.RequestPasswordReset(model.Email);
            if (isOk)
            {
                model.Msg = "password_reset_request_successful".Localize();
            }

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> PasswordChange(string email, string token)
        {
            var model = new PasswordChangeModel { Email = email, Token = token };

            if (!await _userService.IsPasswordResetRequestValid(model.Email, model.Token))
            {
                return Redirect("/User/Login");
            }

            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> PasswordChange(PasswordChangeModel model)
        {
            SetPleaseTryAgain(model);
            if (model.IsNotValid())
            {
                return View(model);
            }

            if (!await _userService.ChangePassword(model.Email, model.Token, model.Password))
            {
                return View(model);
            }

            return Redirect("/User/Login");
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            SetPleaseTryAgain(model);

            if (!model.IsValid())
            {
                return View(model);
            }

            var authenticated = await _userService.Authenticate(model.Email, model.Password);
            if (!authenticated) return View(model);

            var user = await _userService.GetByEmail(model.Email);
            _authService.SignIn(user.Id, user.Name, user.Email, user.RoleName, true);

            return Redirect(!string.IsNullOrEmpty(model.ReturnUrl) ? model.ReturnUrl : "/");
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Logout()
        {
            _authService.SignOut();
            return RedirectToHome();
        }
        #endregion
    }
}