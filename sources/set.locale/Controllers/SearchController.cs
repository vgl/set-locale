using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<JsonResult> Query(string text)
        {
            var model = new ResponseModel { IsOk = false };
            if (string.IsNullOrWhiteSpace(text))
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            var result = await _searchService.Query(text);
            if (result == null)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            model.Result = result;
            model.IsOk = result.Count > 0;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}