using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IUserService _userService;

        public AdminController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, IAppService appService)
            : base(userService, formsAuthenticationService)
        {
            _appService = appService;
            _userService = userService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentUser.RoleId != SetLocaleRole.Admin.Value)
            {
                filterContext.Result = RedirectToHome();    
            }

            base.OnActionExecuting(filterContext); 
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult NewTranslator()
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

            model.Password = Guid.NewGuid().ToString().Replace("-", string.Empty);
            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            var userId = await _userService.Create(model, SetLocaleRole.Translator.Value);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            //send mail to translator to welcome and ask for reset password

            return Redirect("/admin/users");
        }
         
        [HttpGet]
        public async Task<ActionResult> Users(int id = 0, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            PagedList<User> users;

            ViewBag.RoleId = id;
            if (SetLocaleRole.IsValid(ViewBag.RoleId))
            {
                users = await _userService.GetAllByRoleId(ViewBag.RoleId, pageNumber);
            }
            else
            {
                users = await _userService.GetUsers(pageNumber);
            }

            var list = users.Items.Select(UserModel.MapUserToUserModel).ToList();

            var model = new PageModel<UserModel>
            {
                Items = list,
                HasNextPage = users.HasNextPage,
                HasPreviousPage = users.HasPreviousPage,
                Number = users.Number,
                TotalCount = users.TotalCount,
                TotalPageCount = users.TotalPageCount
            };
             
            return View(model);
        }

        //[HttpGet]
        //public async Task<ActionResult> Apps()
        //{
        //    var apps = await _appService.GetAll();
        //    var model = new List<AppModel>();
        //    foreach (var app in apps)
        //    {
        //        model.Add(AppModel.MapFromEntity(app));
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public async Task<ActionResult> Apps(int id = 0)
        {
            var pageNumber = id; 
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var apps = await _appService.GetApps(pageNumber);

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
    }
}