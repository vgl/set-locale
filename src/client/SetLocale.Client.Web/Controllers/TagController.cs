using System.Web.Mvc;
using System.Web.UI.WebControls;
using Castle.Core.Resource;

namespace SetLocale.Client.Web.Controllers
{
    public class TagController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}