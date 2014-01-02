using System.Threading.Tasks;
using System.Web.Mvc;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AppController : BaseController
    {
        private readonly IAppService _appService;

        public AppController(IAppService appService, IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService)
            : base(formsAuthenticationService, demoDataService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<ViewResult> Detail(int id)
        {
            var entity = await _appService.Get(id);
            var model = AppModel.MapFromEntity(entity);
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new AppModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(AppModel model)
        {
            if (!model.IsValidForNew())
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            model.CreatedBy = User.Identity.GetUserId();
            model.Email = User.Identity.GetUserEmail();

            var appId = await _appService.Create(model);
            if (appId == 0)
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            return Redirect("/app/detail/" + appId);
        }
    }
}