using System.Web.Http;
using System.Web.Http.Cors;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.ApiControllers
{
    public class LocaleController : BaseApiController
    {
        private readonly IWordService _wordService;

        public LocaleController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [Route("api/locale/{lang}/{key}")]
        public IHttpActionResult Get(string lang, string key)
        {
            //var word = await _wordService.GetByKey(key);

            return Ok(new
            {
                Key = key,
                Lang = lang,
                Value = "Deneme"
            });
        }

        [Route("api/locales/{lang}"),
         EnableCors(origins: "*", headers: "*", methods: "GET")]
        public IHttpActionResult GetAll(string lang)
        {
            var items = new { lang, items = new { Key = "a", Value = "tr a" } };

            return Ok(items);
        }
    }
}