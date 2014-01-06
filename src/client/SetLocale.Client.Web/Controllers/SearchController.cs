using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class SearchController :BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(
            ISearchService searchService,
            IUserService userService, 
            IFormsAuthenticationService formsAuthenticationService) 
            : base(userService, formsAuthenticationService)
        {
            _searchService = searchService;
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> Query(string text)
        {
            var model = new ResponseModel { Ok = false };
            if (string.IsNullOrEmpty(text))
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            var result = await _searchService.Query(text);
            if (result == null)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            model.Result = result;
            model.Ok = true;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}