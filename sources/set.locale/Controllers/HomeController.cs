using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public HomeController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetId();
                //todo: id ile user yoksa sigout...
            }

            return View();
        }

        [HttpGet, AllowAnonymous]
        public ViewResult Contact()
        {
            var model = new ContactMessageModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Email = User.Identity.GetEmail();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Contact(ContactMessageModel model)
        {
            if (!model.IsValid())
            {
                SetPleaseTryAgain(model);
                return View(model);
            }

            model.IsOk = await _feedbackService.CreateContactMessage(model.Subject, model.Email, model.Message);
            if (model.IsOk)
            {
                model.Msg = "data_saved_successfully_msg".Localize();
            }

            return View(model);
        }
    }
}