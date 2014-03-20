using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using set.locale.Data.Entities;
using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    [AllowAnonymous]
    public class ApiController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly ITagService _tagService;
        private readonly IAppService _appService;
        private readonly IRequestLogService _requestLogService;

        public ApiController(
            IWordService wordService,
            ITagService tagService,
            IAppService appService,
            IRequestLogService requestLogService)
        {
            _wordService = wordService;
            _tagService = tagService;
            _appService = appService;
            _requestLogService = requestLogService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var headers = filterContext.RequestContext.HttpContext.Request.Headers;

            var authHeader = headers[ConstHelper.Authorization];
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

        [HttpGet]
        public async Task<JsonResult> Locales(string app, string lang = "tr", int page = 1)
        {
            var model = new List<NameValueModel>();

            if (!LanguageModel.IsValidLanguageKey(lang)) return Json(new { Error = "not valid language!" }, JsonRequestBehavior.AllowGet);

            PagedList<Word> items;
            if (!string.IsNullOrWhiteSpace(app))
            {
                var myApp = await _appService.GetByName(app);
                items = await _wordService.GetWords(myApp.Id, page);
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

        [HttpGet]
        public async Task<JsonResult> Locale(string app, string lang = "tr", string key = "name")
        {
            app = app.ToUrlSlug();

            if (!LanguageModel.IsValidLanguageKey(lang)) return Json(new { Error = "not valid language!" }, JsonRequestBehavior.AllowGet);

            var word = await _wordService.GetByKeyAndAppName(key, app);
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

        [HttpPost]
        public async Task<JsonResult> AddKey(string key, string tag, string desc)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new HttpException(400, "key argument null");
            }

            var item = await _wordService.Create(new WordModel { Key = key, Tag = tag, Description = desc });

            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<JsonResult> AddKeys(string keys, string tag)
        {
            if (string.IsNullOrEmpty(keys))
            {
                throw new HttpException(400, "keys argument null");
            }

            var app = await _appService.GetByName(tag);
            if (app == null)
            {
                throw new HttpException(400, "app is not found");
            }

            var returnValue = keys.Split(',');
            foreach (var key in returnValue)
            {
                try
                {
                    await _wordService.Create(new WordModel
                    {
                        CreatedBy = app.CreatedBy,
                        Key = key,
                        Tag = tag,
                        AppId = app.Id.ToString(CultureInfo.InvariantCulture)
                    });
                }
                catch (Exception ex)
                {
                }
            }

            return Json(true, JsonRequestBehavior.DenyGet);
        }
    }
}