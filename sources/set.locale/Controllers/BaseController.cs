using System.Threading;
using System.Web.Mvc;

using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetLanguage();

            base.OnActionExecuting(filterContext);
        }

        public void SetLanguage()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = ConstHelper.CultureEN;
                Thread.CurrentThread.CurrentUICulture = ConstHelper.CultureEN;

                var langCookie = Request.Cookies[ConstHelper.__Lang];
                if (langCookie == null) return;

                var lang = langCookie.Value;
                if (lang != ConstHelper.tr) return;

                Thread.CurrentThread.CurrentCulture = ConstHelper.CultureTR;
                Thread.CurrentThread.CurrentUICulture = ConstHelper.CultureTR;
            }
            catch { }
        }

        public RedirectResult RedirectToHome()
        {
            return Redirect("/");
        }

        public void SetPleaseTryAgain(BaseModel model)
        {
            model.Msg = "please_check_the_fields_and_try_again".Localize();
        }
    }
}