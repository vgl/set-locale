﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

﻿using SetLocale.Client.Web.Helpers;
﻿using SetLocale.Client.Web.Models;
﻿using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IWordService _wordService;

        public UserController(
            IUserService userService,
            IWordService wordService,
            IFormsAuthenticationService formsAuthenticationService,
            IAppService appService)
            : base(userService, formsAuthenticationService)
        {
            _appService = appService;
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<ActionResult> Apps(int id = 0, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (id == 0)
            {
                id = User.Identity.GetUserId();
            }

            ViewBag.UserId = id;

            var apps = await _appService.GetByUserId(id, pageNumber);

            if (apps == null)
            {
                return RedirectToHome();
            }

            var list = apps.Items.Select(AppModel.MapFromEntity).ToList();

            var model = new PageModel<AppModel>
            {
                Items = list,
                HasNextPage = apps.HasNextPage,
                HasPreviousPage = apps.HasPreviousPage,
                Number = apps.Number,
                TotalCount = apps.TotalCount,
                TotalPageCount = apps.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> Words(int id = 0, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (id == 0)
            {
                id = User.Identity.GetUserId();
            }

            ViewBag.UserId = id;

            var words = await _wordService.GetByUserId(id, pageNumber);     
            var list = words.Items.Select(WordModel.MapEntityToModel).ToList();

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeStatus(int id, bool isActive)
        {
            var model = new ResponseModel { Ok = false };
            if (id < 1)
            {
                return Json(model, JsonRequestBehavior.DenyGet);    
            }

            model.Ok = await _userService.ChangeStatus(id, isActive);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        #region Membership
        [HttpGet, AllowAnonymous]
        public ActionResult New()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> New(UserModel model)
        {
            if (!model.IsValidForNewDeveloper())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            var userId = await _userService.Create(model);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            return Redirect("/user/apps");
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Reset()
        {
            var model = new ResetModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Email = User.Identity.GetUserEmail();
            }

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            var model = new LoginModel();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {

            if (!model.IsValid())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            var authenticated = await _userService.Authenticate(model.Email, model.Password);
            if (!authenticated)
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            var user = await _userService.GetByEmail(model.Email);
            _formsAuthenticationService.SignIn(string.Format("{0}|{1}|{2}", user.Id, user.Name, user.Email), true);

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("/user/apps");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _formsAuthenticationService.SignOut();
            return RedirectToHome();
        }
        #endregion
    }
}