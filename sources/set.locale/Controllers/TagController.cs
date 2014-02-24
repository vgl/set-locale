using System.Text;
using ServiceStack.Text;
using set.locale.Data.Entities;
using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace set.locale.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        private readonly IAppService _appService;
        private readonly IWordService _wordService;

        public TagController(
            ITagService tagService,
            IAppService appService,
            IWordService wordService)
        {
            _tagService = tagService;
            _appService = appService;
            _wordService = wordService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ViewResult> Detail(string id = "set-locale", int page = 0)
        {
            ViewBag.ID = id;

            var words = await _tagService.GetWords(id, page);
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

            if (!User.Identity.IsAuthenticated) return View(model);

            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<JsonResult> JsonTags()
        {
            var tags = await _tagService.GetTags();
            var result = tags.Select(x => x.Name.Trim()).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<string> Copy(string copyFrom, string appIds, bool force)
        {
            try
            {
                int deletedCount = 0;
                StringBuilder result = new StringBuilder();
                var toAppIdList = JsonSerializer.DeserializeFromString<List<string>>(appIds);
                var fromWordsByTag = await _tagService.GetWords(copyFrom);
                foreach (var appId in toAppIdList)
                {
                    var app = await _appService.Get(appId);
                    var words = await _wordService.GetByAppId(appId);
                    int wordsCount = words.Count;

                    var createdBy = User.Identity.GetId();
                    if (force)
                    {
                        deletedCount = await _wordService.DeleteByAppId(appId, createdBy);
                    }
                    int createCount = await _wordService.CreateList(fromWordsByTag.Select(WordModel.Map).ToList(), appId, createdBy);

                    result.AppendFormat("<h4>{0}</h4>", app.Name);
                    result.AppendFormat("{0}: <span class='label label-info'>{1}</span>, ", "existing_words".Localize(), wordsCount);
                    result.AppendFormat("{0}: <span class='label label-danger'>{1}</span>, ", "deleted_words".Localize(), deletedCount);
                    result.AppendFormat("{0}: <span class='label label-success'>{1}</span>, ", "created_words".Localize(), createCount);
                    result.AppendFormat("{0}: <span class='label label-success'>{1}</span> </br>", "new_total".Localize(), (wordsCount - deletedCount) + createCount);
                }
                return result.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}