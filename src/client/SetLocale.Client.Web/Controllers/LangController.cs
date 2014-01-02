using System.Web;
using System.Web.Mvc;

using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class LangController : BaseController
    {
        public LangController(IUserService userService, IFormsAuthenticationService formsAuthenticationService) : base(userService, formsAuthenticationService)
        {
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Change(string id)
        {
            Response.SetCookie(new HttpCookie(ConstHelper.__Lang, id));

            return HttpContext.Request.UrlReferrer != null ? Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri) : RedirectToHome();
        }
         
    }
}