using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class AppController : BaseController
    {
        private readonly IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToHome();
            }

            var entity = await _appService.Get(id);
            if (entity == null)
            {
                return Redirect("/user/apps");
            }

            ViewBag.IsActive = entity.IsActive;

            var model = AppModel.Map(entity);
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
            SetPleaseTryAgain(model);

            if (!model.IsValid())
            {
                return View(model);
            }

            model.CreatedBy = User.Identity.GetId();
            model.Email = User.Identity.GetEmail();

            var appId = await _appService.Create(model);

            if (appId != null) return RedirectToAction("detail", "app", new { id = appId }); //Redirect("/app/detail/" + appId);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> NewToken(string appId)
        {
            var result = new ResponseModel { IsOk = false };

            var token = new TokenModel
            {
                CreationDate = DateTime.Now,
                UsageCount = 0,
                Token = Guid.NewGuid().ToNoDashString(),
                AppId = appId,
                CreatedBy = User.Identity.GetId()
            };

            var isOk = await _appService.CreateToken(token);
            if (!isOk) return Json(result, JsonRequestBehavior.DenyGet);

            result.IsOk = true;
            result.Result = token;

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeStatus(string id, bool isActive)
        {
            var model = new ResponseModel { IsOk = false };
            if (string.IsNullOrEmpty(id))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _appService.ChangeStatus(id, isActive);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteToken(string token)
        {
            var model = new ResponseModel { IsOk = false };
            if (string.IsNullOrEmpty(token))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _appService.DeleteToken(token, User.Identity.GetId());
            return Json(model, JsonRequestBehavior.DenyGet);
        }

    }
}