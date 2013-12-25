using System.Collections.Generic;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Controllers
{
    public class TagController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            var langs = new List<LanguageModel>();

            langs.Add(new LanguageModel
            {
                Value = "Kayıt Ol",
                Language = "tr",
                FlagImageUrl = "/lang/tr"
            });

            langs.Add(new LanguageModel
            {
                Value = "Sign Up",
                Language = "en",
                FlagImageUrl = "/lang/en"
            });

            var model = new List<KeyModel>();
            model.Add(new KeyModel
            {
                Key = "sign_up",
                Description = "Kullanıcı üyelik açması için kullanılır.",
                IsTranslated = true,
                Tag = new List<TagModel>()
                {
                    new TagModel{ Name = "Membership", UrlName = "membership"}
                },
                Languages = langs
            });


            langs.Clear();

            langs.Add(new LanguageModel
            {
                Value = "Giriş",
                Language = "tr",
                FlagImageUrl = "/Public/img/tr.png"
            });

            langs.Add(new LanguageModel
            {
                Value = "Sign In",
                Language = "en",
                FlagImageUrl = "/Public/img/en.png"
            });

            model.Add(new KeyModel
            {
                Key = "sign_in",
                Description = "Kullanıcı girişi için kullanılır.",
                IsTranslated = true,
                Tag = new List<TagModel>()
                {
                    new TagModel{ Name = "Membership", UrlName = "membership"}
                },
                Languages = langs
            });

            return View(model);
        }
    }
}