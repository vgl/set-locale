using SetLocale.Client.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class KeyController : BaseController
    {
        public KeyController(IFormsAuthenticationService formsAuthenticationService, IDemoDataService demoDataService) : base(formsAuthenticationService, demoDataService)
        {
        }

        [HttpGet]
        public ActionResult Detail()
        {
            var langs = new List<LanguageModel>();
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

            return View(new KeyModel
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
        }

        [HttpGet]
        public ActionResult My()
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

        [HttpGet]
        public ActionResult All()
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

        [HttpGet]
        public ActionResult NotTranslated()
        {

            var langs = new List<LanguageModel>();

            langs.Add(new LanguageModel
            {
                Language = "tr",
                FlagImageUrl = "/lang/tr"
            });

            langs.Add(new LanguageModel
            {
                Language = "en",
                FlagImageUrl = "/lang/en"
            });

            var model = new List<KeyModel>();
            model.Add(new KeyModel
            {
                Key = "save",
                Description = "kaydet butonu için",
                IsTranslated = true,
                Tag = new List<TagModel>()
                {
                    new TagModel{ Name = "Membership", UrlName = "membership"}
                },
                Languages = langs
            });

            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            string[] tags = { "genel" };

            var model = new NewKeyModel
            {
                Key = "Key",
                Tag = tags,
                Description = "Description"
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id, string lang)
        {
            var model = new TranslationModel
            {
                Key = "sign_up",
                Language = "Türkçe",
                LanguageImageUrl = "/public/img/tr.png"
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TranslationModel model)
        {
            if (model.IsValid())
            {
                
            }

            return View(model);
        }
    }
}