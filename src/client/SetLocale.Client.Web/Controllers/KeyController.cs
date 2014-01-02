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
        public KeyController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, IWordService wordService) : base(userService, formsAuthenticationService)
        {
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            var entity = await _wordService.GetId(id);
            if (entity == null)
            {
                return Redirect("/key/detail/"+id);
            }

            var model = KeyModel.MapIdToKeyModel(id);
            return View(model);
        }

        [HttpGet]
        public ViewResult All()
        {
            return null;
            //var model = _demoDataService.GetAllKeys();
            //return View(model);
        }

        [HttpGet]
        public ViewResult NotTranslated()
        {
            //var model = _demoDataService.GetNotTranslatedKeys();
            //return View(model);
            return null;
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

            return Redirect("/key/detail/" + key);
        }

        [HttpGet]
        public ViewResult Edit(string id, string lang = ConstHelper.tr)
        {
            //var model = _demoDataService.GetATranslation();
            //return View(model);
            return null;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TranslationModel model)
        {
            if (model.IsValid())
            {


                return Redirect("/tag/detail/" + model.Key);
            }

            return View(model);
        }
    }
}