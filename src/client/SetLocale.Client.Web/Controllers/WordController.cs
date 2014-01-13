using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        public WordController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, IWordService wordService) : base(userService, formsAuthenticationService)
        {
            _wordService = wordService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToHome();
            }

            var entity = await _wordService.GetByKey(id);           // Entity null kontrolü Test tarafında yapılmadı.
            if (entity == null)
            {
                return RedirectToHome();
            }

            var model = WordModel.MapEntityToModel(entity);
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<ViewResult> All()
        {
            var entities = await _wordService.GetAll();
            var model = new List<WordModel>();
            foreach (var entity in entities)
            {
                model.Add(WordModel.MapEntityToModel(entity));
            }

            return View(model);
        }

        [HttpGet]
        public  async Task<ViewResult> NotTranslated()
        {
            var entities = await _wordService.GetNotTranslated();
            var model = new List<WordModel>();
            foreach (var entity in entities)
            {
                model.Add(WordModel.MapEntityToModel(entity));
            }

            return View(model);
        }
         
        [HttpGet]
        public ViewResult New()
        {
            var model = new WordModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(WordModel model)
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

            return Redirect("/word/detail/" + key);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Translate(string key, string language, string translation)
        {
            var model = new ResponseModel { Ok = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(language)
                || string.IsNullOrEmpty(translation))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.Ok = await _wordService.Translate(key, language, translation);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Tag(string key, string tag)
        {
            var model = new ResponseModel { Ok = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(tag))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.Ok = await _wordService.Tag(key, tag);
            return Json(model, JsonRequestBehavior.DenyGet);
        }
        
    }
}