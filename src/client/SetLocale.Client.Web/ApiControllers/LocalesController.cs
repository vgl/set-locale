using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using System.Threading.Tasks;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.ApiControllers
{
    [RoutePrefix("api")]
    public class LocalesController : BaseApiController
    {
        private readonly IWordService _wordService;
        private readonly ITagService _tagService;

        public LocalesController(
            IWordService wordService,
            ITagService tagService)
        {
            _wordService = wordService;
            _tagService = tagService;
        }

        [HttpGet,
        EnableCors(origins: "*", headers: "*", methods: "GET"),
        Route("locales/{lang}/{tag}/{page}")]
        public async Task<IHttpActionResult> Get(string lang, string tag = "", int page = 1)
        {
            var result = new List<WordItemModel>();
            if (!LanguageModel.IsValidLanguageKey(lang)) return Ok(result);

            PagedList<Word> items;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                items = await _tagService.GetWords(tag, page);
            }
            else
            {
                items = await _wordService.GetWords(page);
            }

            if (items == null || !items.Items.Any()) return Ok(result);

            if (lang == LanguageModel.TR().Key)
            {
                foreach (var item in items.Items)
                {
                    result.Add(new WordItemModel
                    {
                        Key = item.Key,
                        Value = item.Translation_TR
                    });
                }
            }
            else if (lang == LanguageModel.EN().Key)
            {
                foreach (var item in items.Items)
                {
                    result.Add(new WordItemModel
                    {
                        Key = item.Key,
                        Value = item.Translation_EN
                    });
                }
            }
            else if (lang == LanguageModel.IT().Key)
            {
                foreach (var item in items.Items)
                {
                    result.Add(new WordItemModel
                    {
                        Key = item.Key,
                        Value = item.Translation_IT
                    });
                }
            }

            return Ok(result);
        }
    }
}