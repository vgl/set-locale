using System.Threading;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Util;

namespace SetLocale.Client.Web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        public void SetLanguage()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = ConstHelper.CultureEN;
                Thread.CurrentThread.CurrentUICulture = ConstHelper.CultureEN;

                ViewBag.Txt = HttpContext.Application[ConstHelper.en_txt];

                var langCookie = Request.Cookies[ConstHelper.__Lang];
                if (langCookie != null)
                {
                    var lang = langCookie.Value;
                    if (lang == ConstHelper.tr)
                    {
                        ViewBag.Txt = HttpContext.Application[ConstHelper.tr_txt];

                        Thread.CurrentThread.CurrentCulture = ConstHelper.CultureTR;
                        Thread.CurrentThread.CurrentUICulture = ConstHelper.CultureTR;
                    }
                }
                else
                {
                    if (!User.Identity.IsAuthenticated) return;
                    if (CurrentUser.Language == ConstHelper.tr)
                    {
                        ViewBag.Txt = HttpContext.Application[ConstHelper.tr_txt];

                        Thread.CurrentThread.CurrentCulture = ConstHelper.CultureTR;
                        Thread.CurrentThread.CurrentUICulture = ConstHelper.CultureTR;
                    }
                }
            }
            catch { }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetLanguage();

            base.OnActionExecuting(filterContext);
        }

        private UserModel _currentUser;
        public UserModel CurrentUser
        {
            get
            {
                if (_currentUser != null) return _currentUser;

                if (User.Identity.IsAuthenticated)
                {
                    _currentUser = new UserModel
                    {
                        Language = ConstHelper.en,
                        Id = 1,
                        IsActive = true,
                        Email = "test@test.com",
                        Name = "Translator X",
                        Role = ConstHelper.User
                    };

                    // _currentUser = _userService.GetUserSync(User.Identity.GetUserId());
                }
                else
                {
                    // _formsAuthenticationService.SignOut();
                }

                return _currentUser;
            }
        }
    }
}