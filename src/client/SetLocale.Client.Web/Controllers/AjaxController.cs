using System;
using System.Web.Mvc;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Controllers
{
    public class AjaxController : BaseController
    {
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult NewToken(int appId)
        {
            var token = new TokenModel
            {
                CreationDate = DateTime.Now,
                CreationDateStr = DateTime.Now.ToString("f"),
                UsageCount = 1,
                Token = Guid.NewGuid().ToString().Replace("-", "")
            };

            return Json(token, JsonRequestBehavior.DenyGet);
        }
    }
}