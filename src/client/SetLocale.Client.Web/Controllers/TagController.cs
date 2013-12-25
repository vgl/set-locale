using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Castle.Core.Resource;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Controllers
{
    public class TagController : Controller
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

            return View(model);
        }
    }
}