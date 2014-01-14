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

        //[HttpGet]
        //public async Task<ActionResult> Users(int id = 0)
        //{
        //    List<User> users;
        //    var roleId = id;
        //    if (SetLocaleRole.IsValid(roleId))
        //    {
        //        users = await _userService.GetAllByRoleId(roleId);
        //    }
        //    else
        //    {
        //        users = await _userService.GetAll();
        //    }

        //    var model = new List<UserModel>();
        //    foreach (var user in users)
        //    {
        //        model.Add(UserModel.MapUserToUserModel(user));
        //    }

        //    return View(model);
        //}

        [HttpGet]
        public async Task<ActionResult> Users(int id = 0, int pageNum = 1)
        {
            var pageNumber = pageNum;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            PagedList<User> users;

            var roleId = id;
            if (SetLocaleRole.IsValid(roleId))
            {
                users = await _userService.GetAllByRoleId(roleId, pageNumber);
            }
            else
            {
                users = await _userService.GetAll(pageNumber);
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

        [HttpGet]
        public async Task<ActionResult> Apps()
        {
            var apps = await _appService.GetAll();
            var model = new List<AppModel>();
            foreach (var app in apps)
            {
                model.Add(AppModel.MapFromEntity(app));
            }
            return View(model);
        }
    }
}