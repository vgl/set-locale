using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class KeyController : BaseController
    {
        private readonly IWordService _wordService;

        public KeyController(
            IWordService wordService,
            IFormsAuthenticationService formsAuthenticationService,
            IDemoDataService demoDataService)
            : base(formsAuthenticationService, demoDataService)
        {
            _wordService = wordService;
        }

        [HttpGet]
        public ViewResult Detail()
        {
            var model = _demoDataService.GetAKey();
            return View(model);
        }

        [HttpGet]
        public ViewResult All()
        {
            var model = _demoDataService.GetAllKeys();
            return View(model);
        }

        [HttpGet]
        public ViewResult NotTranslated()
        {
            var model = _demoDataService.GetNotTranslatedKeys();
            return View(model);
        }

        [HttpGet]
        public ViewResult New()
        {
            var model = new KeyModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(KeyModel model)
        {
            if (!model.IsValidForNew())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            model.CreatedBy = User.Identity.GetUserId();
            var key = await _wordService.Create(model);
            if (key == null)
            {
                model.Msg = "bir sorun oluştu, daha önce eklenmiş olabilir";
                return View(model);
            }

            return Redirect("/user/key/" + key);
        }

        [HttpGet]
        public ViewResult Edit(string id, string lang = ConstHelper.tr)
        {
            var model = _demoDataService.GetATranslation();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TranslationModel model)
        {
            if (model.IsValid())
            {


                return RedirectToAction("Index", "Tag");
            }

            return View(model);
        }
    }
}