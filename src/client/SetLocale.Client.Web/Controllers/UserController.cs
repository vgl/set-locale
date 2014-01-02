﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
﻿using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, IAppService appService) : base(userService, formsAuthenticationService)
        {
            _appService = appService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Apps(int userId = 0)
        {
            if (userId==0)
            {
                userId = User.Identity.GetUserId();
            }

            var apps = await _appService.GetByUserId(userId);
            var model = AppModel.MapFromEntity(apps);
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Keys()
        {
            //var model = _demoDataService.GetMyKeys();
            //return View(model);
            return null;
        }


        #region Membership
        [HttpGet, AllowAnonymous]
        public ActionResult New()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
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
            var model = new ResetModel()
            {
                Email = "dev@test.com"
            };

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
             
            if (model.ReturnUrl != "" && model.ReturnUrl != "/")
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