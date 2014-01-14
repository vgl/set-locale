using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(
            ITagService tagService,
            IUserService userService, 
            IFormsAuthenticationService formsAuthenticationService) 
            : base(userService, formsAuthenticationService)
        {
            _tagService = tagService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ViewResult> Detail(string id = "set-locale")
        {
            ViewBag.Key = id;

            var entities = await _tagService.GetWords(id);
            var model = entities.Select(WordModel.MapEntityToModel).ToList();

            return View(model);
        }
    }
}