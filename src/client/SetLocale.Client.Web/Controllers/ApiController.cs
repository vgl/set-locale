using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class ApiController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly ITagService _tagService;
        private readonly IAppService _appService;
        private readonly IRequestLogService _requestLogService;

        public ApiController(
            IUserService userService,
            IWordService wordService,
            ITagService tagService,
            IAppService appService,
            IRequestLogService requestLogService,
            IFormsAuthenticationService formsAuthenticationService)
            : base(userService, formsAuthenticationService)
        {
            _wordService = wordService;
            _tagService = tagService;
            _appService = appService;
            _requestLogService = requestLogService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var headers = filterContext.RequestContext.HttpContext.Request.Headers;

            var authHeader = headers["Authorization"];
            if (authHeader == null) ReturnNotAuthenticated(filterContext);

            var token = authHeader;
            if (string.IsNullOrWhiteSpace(token)) ReturnNotAuthenticated(filterContext);

            var isTokenValidTask = _appService.IsTokenValid(token);
            isTokenValidTask.Wait();

            if (!isTokenValidTask.Result) ReturnNotAuthenticated(filterContext);

            try
            {
                _requestLogService.Log(token, Request.UserHostAddress, Request.Url.AbsolutePath);
            }
            catch { }

            base.OnActionExecuting(filterContext);
        }

        private static void ReturnNotAuthenticated(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.Clear();
            filterContext.RequestContext.HttpContext.Response.Write(
                "your request is not authenticated with valid token!<br/>" +
                "please look at the api documentation for authenticating this request ... <br/>" +
                "<a href='/api/docs/auth'>/api/docs/auth</a>");
            filterContext.RequestContext.HttpContext.Response.End();
        }

        [HttpGet, AllowAnonymous]
        public async Task<JsonResult> Locales(string tag, string lang = "tr", int page = 1)
        {
            var model = new List<NameValueModel>();

            if (!LanguageModel.IsValidLanguageKey(lang)) return Json(new { Error = "not valid language!" }, JsonRequestBehavior.AllowGet);

            PagedList<Word> items;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                items = await _tagService.GetWords(tag, page);
            }
            else
            {
                items = await _wordService.GetWords(page);
            }

            if (items == null || !items.Items.Any()) return Json(model, JsonRequestBehavior.AllowGet);

            if (lang == LanguageModel.TR().Key)
            {
                model.AddRange(
                    items.Items.Select(
                        item => new NameValueModel
                        {
                            Name = item.Key,
                            Value = item.Translation_TR
                        }));
            }
            else if (lang == LanguageModel.EN().Key)
            {
                model.AddRange(
                    items.Items.Select(
                        item => new NameValueModel
                        {
                            Name = item.Key,
                            Value = item.Translation_EN
                        }));
            }
            else if (lang == LanguageModel.IT().Key)
            {
                model.AddRange(
                    items.Items.Select(
                        item => new NameValueModel
                        {
                            Name = item.Key,
                            Value = item.Translation_IT
                        }));
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AllowAnonymous]
        public async Task<JsonResult> Locale(string lang = "tr", string key = "name")
        {
            if (!LanguageModel.IsValidLanguageKey(lang)) return Json(new { Error = "not valid language!" }, JsonRequestBehavior.AllowGet);

            var word = await _wordService.GetByKey(key);
            if (word == null)
            {
                return Json(new { Info = "word not found!" }, JsonRequestBehavior.AllowGet);
            }

            var model = new LocaleModel { Key = word.Key, Lang = lang, Value = word.Key };

            var type = word.GetType();
            var translationFieldName = string.Format("Translation_{0}", lang.ToUpperInvariant());
            var propInfo = type.GetProperty(translationFieldName, new Type[0]);
            if (propInfo != null)
            {
                model.Value = propInfo.GetValue(word, null) != null ? propInfo.GetValue(word, null).ToString() : string.Empty;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}