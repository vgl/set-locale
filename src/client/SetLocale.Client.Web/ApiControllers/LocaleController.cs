using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using System.Threading.Tasks;

using SetLocale.Client.Web.Helpers;
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
    }
}