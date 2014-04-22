using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Entities;
using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFeedbackService _feedbackService;
        private readonly IAppService _appService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.GetRoleName() != ConstHelper.Admin)
            {
                filterContext.Result = RedirectToHome();
            }

            base.OnActionExecuting(filterContext);
        }

        public AdminController(IUserService userService, IFeedbackService feedbackService, IAppService appService)
        {
            _userService = userService;
            _feedbackService = feedbackService;
            _appService = appService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Users(int id = 1)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var users = await _userService.GetUsers(pageNumber);
            var list = users.Items.Select(UserModel.Map).ToList();

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeUserStatus(string id, bool isActive)
        {
            var model = new ResponseModel { IsOk = false };
            if (string.IsNullOrEmpty(id))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _userService.ChangeStatus(id, isActive);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<ViewResult> Feedbacks(int id = 1)
        {
            var result = await _feedbackService.GetFeedbacks(id);
            var list = result.Items.Select(FeedbackModel.Map).ToList();
            var model = new PageModel<FeedbackModel>
            {
                Items = list,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                Number = result.Number,
                TotalCount = result.TotalCount,
                TotalPageCount = result.TotalPageCount
            };
            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ContactMessages(int id = 1)
        {
            var result = await _feedbackService.GetContactMessages(id);
            var list = result.Items.Select(ContactMessageModel.Map).ToList();
            var model = new PageModel<ContactMessageModel>
            {
                Items = list,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                Number = result.Number,
                TotalCount = result.TotalCount,
                TotalPageCount = result.TotalPageCount
            };
            return View(model);
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
            SetPleaseTryAgain(model);

            model.Id = Guid.NewGuid().ToNoDashString();
            model.Password = Guid.NewGuid().ToNoDashString();
            if (model.IsNotValid())
            {
                return View(model);
            }

            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            var status = await _userService.Create(model, SetLocaleRole.Translator.ToString());
            if (status)
            {
                //todo:send mail to translator to welcome and ask for reset password
                return Redirect("/admin/users");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Apps(int id = 0)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var apps = await _appService.GetApps(pageNumber);

            var list = apps.Items.Select(AppModel.Map).ToList();

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