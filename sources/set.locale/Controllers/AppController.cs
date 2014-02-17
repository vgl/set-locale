using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class AppController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetId();
                //todo: id ile user yoksa sigout...
            }

            return View();
        }

    }
}