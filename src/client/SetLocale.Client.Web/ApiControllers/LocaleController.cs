using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using System.Threading.Tasks;

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
         Route("locale/{lang?}/{key?}"),
         EnableCors(origins: "*", headers: "*", methods: "GET")]
        public async Task<IHttpActionResult> Get(string key, string lang = "tr")
        {
            if (string.IsNullOrEmpty(key)) throw new HttpResponseException(HttpStatusCode.NotFound);

            var word = await _wordService.GetByKey(key);
            if (word == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var model = new LocaleModel { Key = word.Key, Lang = lang, Value = word.Key };

            var type = word.GetType();
            var translationFieldName = string.Format("Translation_{0}", lang.ToUpperInvariant());
            var propInfo = type.GetProperty(translationFieldName, new Type[0]);
            if (propInfo != null)
            {
                model.Value = propInfo.GetValue(word, null) != null ? propInfo.GetValue(word, null).ToString() : string.Empty;
            }

            return Ok(model);
        }
    }
}