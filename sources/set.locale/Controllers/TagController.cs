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
        public async Task<bool> Copy(string copyFromTag, string appIds, bool force)
        {
            var toAppIdList = JsonSerializer.DeserializeFromString<List<string>>(appIds);
            var fromWordsByTag = await _tagService.GetWords(copyFromTag);
            foreach (var appId in toAppIdList)
            {
                var words = await _wordService.GetByAppId(appId);
                if (force)
                {
                    await _wordService.DeleteList(words.Select(WordModel.Map).ToList());
                }
                await _wordService.CreateList(fromWordsByTag.Select(WordModel.Map).ToList(), appId, User.Identity.GetId());
            }
            return true;
        }
    }
}