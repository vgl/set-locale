using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using set.locale.Helpers;
using set.locale.Data.Entities;
using set.locale.Data.Services;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly IAppService _appService;
        public WordController(
            IWordService wordService,
            IAppService appService)
        {
            _wordService = wordService;
            _appService = appService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToHome();
            ViewBag.ID = id;

            var entity = await _wordService.GetById(id);
            if (entity == null) return RedirectToHome();
            var model = WordModel.Map(entity);

            if (!User.Identity.IsAuthenticated) return View(model);

            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map).Where(x => x.Id != model.AppId);

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<ViewResult> All(int id = 0)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = await _wordService.GetWords(pageNumber);
            var list = items.Items.Select(WordModel.Map).ToList();
            var model = new PageModel<WordModel>
            {
                Items = list,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
                Number = items.Number,
                TotalCount = items.TotalCount,
                TotalPageCount = items.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> NotTranslated(int id = 1)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var words = await _wordService.GetNotTranslated(pageNumber);
            var list = words.Items.Select(WordModel.Map).ToList();
            var model = new PageModel<WordModel>
            {
                Items = list,
                HasNextPage = words.HasNextPage,
                HasPreviousPage = words.HasPreviousPage,
                Number = words.Number,
                TotalCount = words.TotalCount,
                TotalPageCount = words.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> New()
        {
            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);
            var model = new WordModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(WordModel model)
        {
            SetPleaseTryAgain(model);
            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);

            if (model.IsNotValid())
            {
                return View(model);
            }

            model.CreatedBy = User.Identity.GetId();

            model.Tag = (await _appService.Get(model.AppId)).Name;

            var id = await _wordService.Create(model);
            if (id != null)
            {
                return Redirect("/word/detail/" + id);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Translate(string id, string language, string translation)
        {
            var model = new ResponseModel { IsOk = false };

            if (string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(language))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _wordService.AddTranslate(id, language, translation);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Tag(string key, string tag)
        {
            var model = new ResponseModel { IsOk = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(tag))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _wordService.Tag(key, tag);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Export()
        {
            var model = new ResponseModel { IsOk = false };
            var fileName = await _wordService.ExportWordsToExcel("all-words");

            model.IsOk = true;
            model.Result = fileName;

            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<string> Copy(string copyFrom, string appIds, bool force)
        {
            try
            {
                return await _wordService.Copy(copyFrom, appIds, User.Identity.GetId(), force);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}