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

        public TagController(
            ITagService tagService,
            IAppService appService)
        {
            _tagService = tagService;
            _appService = appService;
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


            ViewBag.Apps = new List<AppModel>();
            if (list.Any())
            {
                ViewBag.Apps = apps.Select(AppModel.Map).Where(x => x.Id != list.First().AppId);
            }

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
                return await _tagService.Copy(copyFrom, appIds, User.Identity.GetId(), force);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}