using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.ApiControllers
{
    [RoutePrefix("api")]
    public class LocaleController : BaseApiController
    {
        private readonly IWordService _wordService;

        public LocaleController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpGet,
         EnableCors(origins: "*", headers: "*", methods: "GET"),
         Route("locale/{lang}/{key}")]
        public async Task<IHttpActionResult> Get(string lang, string key)
        {
            var word = await _wordService.GetByKey(key);
            if (word == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            switch (lang)
            {
                case ConstHelper.en:
                    return Ok(new { word.Key, Lang = ConstHelper.en, Value = word.Translation_EN });
                case ConstHelper.tr:
                    return Ok(new { word.Key, Lang = ConstHelper.tr, Value = word.Translation_TR });
            }

            return BadRequest(string.Format("word found but has no translation for {0}", lang));
        }

        [HttpGet,
         EnableCors(origins: "*", headers: "*", methods: "GET"),
         Route("locales/{lang}")]
        public async Task<IHttpActionResult> GetAll(string lang)
        {
            var result = new List<WordItemModel>();
            if (!LanguageModel.IsValidLanguageKey(lang))
            {
                return Ok(result);
            }

            var items = await _wordService.GetAll();
            if (lang == LanguageModel.TR().Key)
            {
                foreach (var item in items)
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
                foreach (var item in items)
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