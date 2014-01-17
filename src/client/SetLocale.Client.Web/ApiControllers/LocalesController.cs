using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using System.Threading.Tasks;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.ApiControllers
{
    [RoutePrefix("api")]
    public class LocalesController : BaseApiController
    {
        private readonly IWordService _wordService;

        public LocalesController(IWordService wordService, IAppService appService, IRequestLogService requestLogService)
                          : base(appService, requestLogService)
        {
            _wordService = wordService;
        }

        [HttpGet,
        EnableCors(origins: "*", headers: "*", methods: "GET"),
        Route("locales/{lang}/{page}")]
        public async Task<IHttpActionResult> Get(string lang, int page)
        {
            await base.IsTokenCheckedAndLoged();

            var result = new List<WordItemModel>();
            if (!LanguageModel.IsValidLanguageKey(lang))
            {
                return Ok(result);
            }

            var items = await _wordService.GetWords(page);
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

            return Ok(result);
        }
    }
}