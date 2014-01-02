using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [HttpGet]
        public async Task<ViewResult> Detail(string id = "set-locale")
        {
            var entities = await _tagService.GetWords(id);
            var model = new List<WordModel>();
            foreach (var entity in entities)
            {
                model.Add(TagModel.MapEntityToModel(entity));
            }
            return View(model);
        }
    }
}