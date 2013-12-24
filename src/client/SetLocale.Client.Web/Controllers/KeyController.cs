using SetLocale.Client.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class KeyController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            string[] tags = { "genel" };

            var langs = new List<LanguageModel>();

            langs.Add(new LanguageModel
                {
                    Value="Kayıt Ol",
                    Language="tr",
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
                Tag = new[] { "genel" },
                Languages = langs  
            });


            langs.Clear();

            langs.Add(new LanguageModel
            {
                Value="Giriş",
                Language="tr",
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
                Tag = tags,
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
        public ActionResult Edit()
        {
            return View();
        }
    }
}