using set.locale.Data.Services;
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

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
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


            return View(model);
        }


        [HttpGet, AllowAnonymous]
        public async Task<JsonResult> JsonTags()
        {
            var tags = await _tagService.GetTags();
            var result = tags.Select(x => x.Name.Trim()).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}