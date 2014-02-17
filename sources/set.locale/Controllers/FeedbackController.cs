using System.Threading.Tasks;
using System.Web.Mvc;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;


namespace set.locale.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<JsonResult> New(string message)
        {
            var model = new ResponseModel { IsOk = false };
            SetPleaseTryAgain(model);

            if (string.IsNullOrWhiteSpace(message)) return Json(model, JsonRequestBehavior.DenyGet);

            var email = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                email = User.Identity.GetEmail();
            }

            model.IsOk = await _feedbackService.CreateFeedback(message, email);

            if (model.IsOk)
            {
                model.Msg = "data_saved_successfully_msg".Localize();
            }

            return Json(model, JsonRequestBehavior.DenyGet);
        }
    }
}