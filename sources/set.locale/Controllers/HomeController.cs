using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFeedbackService _feedbackService;
        private readonly IReportService _reportService;

        public HomeController(
            IUserService userService,
            IFeedbackService feedbackService, 
            IReportService reportService)
        {
            _userService = userService;
            _feedbackService = feedbackService;
            _reportService = reportService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var isThereAnyUser = await _userService.IsThereAnyUser();
            if (!isThereAnyUser)
            {
                return Redirect(User.Identity.IsAuthenticated ? "user/logout" : "user/newadmin");
            }

            var model = await _reportService.GetHomeStats();
            model.Summary = string.Format("SetLocale's <strong>{2}</strong> translator provided <strong>{4}</strong> translation for <strong><a href='/word/all' id='aAllWords' style='text-decoration:underline;color:red;'>{3}</a></strong> keys and <strong>{0}</strong> developer is consuming this service with <strong>{1}</strong> application",
                model.DeveloperCount,
                model.ApplicationCount,
                model.TranslatorCount,
                model.KeyCount,
                model.TranslationCount);

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ViewResult Contact()
        {
            var model = new ContactMessageModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Email = User.Identity.GetEmail();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Contact(ContactMessageModel model)
        {
            if (!model.IsValid())
            {
                SetPleaseTryAgain(model);
                return View(model);
            }

            model.IsOk = await _feedbackService.CreateContactMessage(model.Subject, model.Email, model.Message);
            if (model.IsOk)
            {
                model.Msg = "data_saved_successfully_msg".Localize();
            }

            return View(model);
        }
    }
}