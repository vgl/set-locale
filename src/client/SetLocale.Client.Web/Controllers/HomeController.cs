using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IReportService _reportService;

        public HomeController(
            IReportService reportService,
            IUserService userService,
            IFormsAuthenticationService formsAuthenticationService)
            : base(userService, formsAuthenticationService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ViewResult> Index()
        {
            var model = await _reportService.GetHomeStats();
            model.Summary = string.Format(_htmlHelper.LocalizationString("home_summary"),
                model.DeveloperCount,
                model.ApplicationCount,
                model.TranslatorCount,
                model.KeyCount,
                model.TranslationCount);

            return View(model);
        }
    }
}