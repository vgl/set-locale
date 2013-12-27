using System.Threading;
using System.Web.Mvc;

using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly IFormsAuthenticationService _formsAuthenticationService;
        public readonly IDemoDataService _demoDataService;
        
        public BaseController(
            IFormsAuthenticationService formsAuthenticationService,
            IDemoDataService demoDataService)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _demoDataService = demoDataService;
        }

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
                    _currentUser = _demoDataService.GetAUser();
                }
                else
                {
                    _formsAuthenticationService.SignOut();
                }

                return _currentUser;
            }
        }
    }
}