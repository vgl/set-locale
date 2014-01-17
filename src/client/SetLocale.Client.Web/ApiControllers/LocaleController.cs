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
         EnableCors(origins: "*", headers: "*", methods: "GET"),
         Route("locale/{lang}/{key}")]
        public async Task<IHttpActionResult> Get(string lang, string key)
        {
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
                model.Value = propInfo.GetValue(word).ToString();
            }

            return Ok(model);
        }
    }
}